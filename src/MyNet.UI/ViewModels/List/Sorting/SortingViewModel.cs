// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using DynamicData;
using DynamicData.Binding;
using MyNet.DynamicData.Extensions;
using MyNet.Observable;
using MyNet.Observable.Attributes;
using MyNet.UI.Commands;
using MyNet.Utilities;
using MyNet.Utilities.Deferring;
using PropertyChanged;

namespace MyNet.UI.ViewModels.List.Sorting
{

    [CanBeValidated(false)]
    [CanSetIsModified(false)]
    public class SortingViewModel : EditableObject, ISortingViewModel, ICollection<ISortingPropertyViewModel>, INotifyCollectionChanged
    {
        private readonly IReadOnlyDictionary<string, ListSortDirection> _defaultSortingProperties;
        private readonly Deferrer _sortingChangedDeferrer;

        protected SortingPropertiesCollection SortingProperties { get; } = [];

        public ISortingPropertyViewModel? ActiveSortingProperty => SortingProperties.OrderBy(x => x.Order).FirstOrDefault(x => x.IsEnabled);

        public int ActiveCount => SortingProperties.Count(x => x.IsEnabled);

        public ICommand AddCommand { get; }

        public ICommand SwitchCommand { get; }

        public ICommand ToggleCommand { get; }

        public ICommand RemoveCommand { get; }

        public ICommand ResetCommand { get; }

        public ICommand ClearCommand { get; }

        public ICommand ApplyCommand { get; }

        public event EventHandler<SortingChangedEventArgs>? SortingChanged;

        public SortingViewModel() : this([]) { }

        public SortingViewModel(string defaultProperty, ListSortDirection listSortDirection = ListSortDirection.Ascending) : this(new[] { defaultProperty }) { }

        public SortingViewModel(IList<string> defaultProperties) : this(defaultProperties.ToDictionary(x => x, _ => ListSortDirection.Ascending)) { }

        public SortingViewModel(IDictionary<string, ListSortDirection> defaultProperties)
        {
            _sortingChangedDeferrer = new Deferrer(OnSortChanged);
            _defaultSortingProperties = defaultProperties.AsReadOnly();

            ClearCommand = CommandsManager.Create(Clear);
            AddCommand = CommandsManager.CreateNotNull<string>(x => Add(x));
            ToggleCommand = CommandsManager.CreateNotNull<string>(Toggle);
            SwitchCommand = CommandsManager.CreateNotNull<string>(Switch);
            ApplyCommand = CommandsManager.CreateNotNull<List<(string, ListSortDirection)>>(x => Set(x.Select((y, index) => CreateSortingProperty(y.Item1, y.Item2, index + 1))));
            RemoveCommand = CommandsManager.CreateNotNull<string>(Remove);
            ResetCommand = CommandsManager.Create(Reset);

            Reset();

            Disposables.Add(SortingProperties.ToObservableChangeSet(x => x.PropertyName).SubscribeAll(() => _sortingChangedDeferrer.DeferOrExecute()));

            SortingProperties.CollectionChanged += new NotifyCollectionChangedEventHandler(HandleCollectionChanged);
        }

        protected IDisposable DeferChanged() => _sortingChangedDeferrer.Defer();

        protected virtual ISortingPropertyViewModel CreateSortingProperty(string propertyName, ListSortDirection listSortDirection = ListSortDirection.Ascending, int? order = null)
            => new SortingPropertyViewModel(propertyName, listSortDirection, order ?? ActiveCount + 1);

        public virtual void Add(string propertyName, ListSortDirection listSortDirection = ListSortDirection.Ascending, int? order = null) => SortingProperties.TryAdd(CreateSortingProperty(propertyName, listSortDirection, order));

        public virtual void Remove(string propertyName) => SortingProperties.Remove(propertyName);

        public virtual void Switch(string propertyName)
        {
            if (SortingProperties[propertyName] is ISortingPropertyViewModel property)
            {
                using (_sortingChangedDeferrer.Defer())
                {
                    if (!property.IsEnabled)
                        property.Order = ActiveCount + 1;
                    property.IsEnabled = true;
                    property.Direction = property.Direction == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
                }
            }
        }

        public void Toggle(string propertyName)
        {
            using (_sortingChangedDeferrer.Defer())
            {
                var switchDirection = SortingProperties[propertyName] is ISortingPropertyViewModel property && property.IsEnabled && property.Direction == ListSortDirection.Ascending;
                Clear();
                Add(propertyName);

                if (switchDirection)
                    Switch(propertyName);
            }
        }

        public virtual void Set(IEnumerable<ISortingPropertyViewModel> properties)
        {
            using (_sortingChangedDeferrer.Defer())
                SortingProperties.Set(properties);
        }

        public virtual void Clear() => SortingProperties.Clear();

        public void Reset() => Set(_defaultSortingProperties.Select((x, index) => CreateSortingProperty(x.Key, x.Value, index + 1)));

        [SuppressPropertyChangedWarnings]
        protected virtual void OnSortChanged()
        {
            RaisePropertyChanged(nameof(Count));
            RaisePropertyChanged(nameof(ActiveCount));
            RaisePropertyChanged(nameof(ActiveSortingProperty));
            SortingChanged?.Invoke(this, new SortingChangedEventArgs(SortingProperties));
        }

        #region ICollection

        public int Count => SortingProperties.Count;

        public virtual bool IsReadOnly => false;

        public virtual void Add(ISortingPropertyViewModel item) => IsReadOnly.IfFalse(() => SortingProperties.Add(item));

        public virtual bool Remove(ISortingPropertyViewModel item) => !IsReadOnly && SortingProperties.Remove(item);

        public bool Contains(ISortingPropertyViewModel item) => SortingProperties.Contains(item);

        public void CopyTo(ISortingPropertyViewModel[] array, int arrayIndex) => SortingProperties.CopyTo(array, arrayIndex);

        public IEnumerator<ISortingPropertyViewModel> GetEnumerator() => SortingProperties.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => SortingProperties.GetEnumerator();

        #endregion

        #region INotifyCollectionChanged

        event NotifyCollectionChangedEventHandler? INotifyCollectionChanged.CollectionChanged
        {
            add => CollectionChanged += value;
            remove => CollectionChanged -= value;
        }

        protected event NotifyCollectionChangedEventHandler? CollectionChanged;

        [SuppressPropertyChangedWarnings]
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args) => CollectionChanged?.Invoke(this, args);

        private void HandleCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => OnCollectionChanged(e);

        #endregion INotifyCollectionChanged

        protected override void Cleanup()
        {
            SortingProperties.CollectionChanged -= new NotifyCollectionChangedEventHandler(HandleCollectionChanged);
            base.Cleanup();
        }
    }
}
