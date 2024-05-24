// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using MyNet.Observable.Attributes;
using MyNet.Observable.Collections.Providers;
using MyNet.Observable.Threading;
using MyNet.UI.Commands;
using MyNet.UI.Selection;
using MyNet.UI.Selection.Models;
using MyNet.Utilities;
using MyNet.Utilities.Providers;
using PropertyChanged;

namespace MyNet.UI.ViewModels.List
{
    [CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
    [CanBeValidatedForDeclaredClassOnly(false)]
    public abstract class SelectionListViewModel<T, TCollection> : WrapperListViewModel<T, SelectedWrapper<T>, TCollection>
                where TCollection : SelectableCollection<T>
                where T : notnull
    {
        protected SelectionListViewModel(TCollection collection,
                                         IListParametersProvider? parametersProvider = null,
                                         SelectionMode? selectionMode = null) : base(collection, parametersProvider)
        {
            SelectCommand = CommandsManager.CreateNotNull<T>(Collection.Select, CanSelect);
            SelectItemsCommand = CommandsManager.CreateNotNull<IEnumerable<T>>(Collection.Select, CanSelect);
            UnselectCommand = CommandsManager.CreateNotNull<T>(Collection.Unselect, CanUnselect);
            UnselectItemsCommand = CommandsManager.CreateNotNull<IEnumerable<T>>(Collection.Unselect, CanUnselect);
            SelectAllCommand = CommandsManager.Create(SelectAll, () => Wrappers.Any(x => CanChangeSelectedState(x, true)));
            UnselectAllCommand = CommandsManager.Create(UnselectAll, () => Wrappers.Any(x => CanChangeSelectedState(x, false)));
            ClearSelectionCommand = CommandsManager.Create(ClearSelection, () => WrappersSource.Any(x => CanChangeSelectedState(x, false)));
            OpenSelectedItemCommand = CommandsManager.Create(() => Open(SelectedItem), () => CanOpenItem(SelectedItem) && SelectedItems.Count() == 1);
            OpenTabSelectedItemCommand = CommandsManager.CreateNotNull<object>(x => Open(SelectedItem, (int)x), x => CanOpenItem(SelectedItem) && SelectedItems.Count() == 1);
            EditSelectedItemCommand = CommandsManager.Create(async () => await EditAsync(SelectedItem).ConfigureAwait(false), () => CanEditItem(SelectedItem) && SelectedItems.Count() == 1);
            EditSelectedItemsCommand = CommandsManager.Create(async () => await EditRangeAsync(SelectedItems), () => CanEditItems(SelectedItems) && SelectedWrappers.Count > 0);
            RemoveSelectedItemCommand = CommandsManager.Create(async () => await RemoveAsync(SelectedItem).ConfigureAwait(false), () => SelectedItems.Count() == 1 && CanRemoveItems(SelectedItems));
            RemoveSelectedItemsCommand = CommandsManager.Create(async () => await RemoveRangeAsync(SelectedItems), () => CanRemoveItems(SelectedItems) && SelectedWrappers.Count > 0);

            Disposables.AddRange(
            [
                System.Reactive.Linq.Observable.FromEventPattern(x => Collection.SelectionChanged += x, x => Collection.SelectionChanged -= x).Subscribe(_ => OnSelectionChanged()),
                Collection.Wrappers.ToObservableChangeSet().OnItemRemoved(x => x.IsSelected = false).Subscribe()
            ]);
        }

        public SelectionMode SelectionMode
        {
            get => Collection.SelectionMode;
            set => Collection.SelectionMode = value;
        }

        public IDictionary<string, ICommand> PresetSelections { get; } = new Dictionary<string, ICommand>();

        public ReadOnlyObservableCollection<SelectedWrapper<T>> SelectedWrappers => Collection.SelectedWrappers;

        public IEnumerable<T> SelectedItems => Collection.SelectedItems;

        public SelectedWrapper<T>? SelectedWrapper => SelectedWrappers.FirstOrDefault();

        public new T? SelectedItem => SelectedItems.FirstOrDefault();

        public bool? AreAllSelected
        {
            get
            {
                var selected = Wrappers.Select(item => item.IsSelected).Distinct().ToList();
                return selected.Count == 1 ? selected.Single() : null;
            }
            set
            {
                if (value.HasValue)
                    Collection.SelectAll(value.Value);
            }
        }

        public ICommand OpenSelectedItemCommand { get; protected set; }

        public ICommand OpenTabSelectedItemCommand { get; protected set; }

        public ICommand EditSelectedItemCommand { get; protected set; }

        public ICommand EditSelectedItemsCommand { get; protected set; }

        public ICommand RemoveSelectedItemCommand { get; protected set; }

        public ICommand RemoveSelectedItemsCommand { get; protected set; }

        public ICommand SelectCommand { get; private set; }

        public ICommand SelectItemsCommand { get; private set; }

        public ICommand UnselectCommand { get; private set; }

        public ICommand UnselectItemsCommand { get; private set; }

        public ICommand SelectAllCommand { get; private set; }

        public ICommand UnselectAllCommand { get; private set; }

        public ICommand ClearSelectionCommand { get; private set; }

        protected virtual bool SelectionIsAvailable(Func<T, bool> predicate) => SelectedWrappers.Count > 0 && SelectedItems.All(predicate);

        protected virtual bool CanChangeSelectedState(SelectedWrapper<T>? item, bool value) => item is not null && item.IsSelectable && item.IsSelected != value;

        protected virtual bool CanChangeSelectedState(T item, bool value) => CanChangeSelectedState(WrappersSource.First(x => ReferenceEquals(x, item)), value);

        protected virtual bool CanSelect(T item) => CanChangeSelectedState(item, true);

        protected virtual bool CanSelect(IEnumerable<T> items) => items.Any(CanSelect);

        protected virtual bool CanUnselect(T item) => CanChangeSelectedState(item, false);

        protected virtual bool CanUnselect(IEnumerable<T> items) => items.Any(CanUnselect);

        public void UpdateSelection(IEnumerable<T> items) => Collection.SetSelection(items);

        protected virtual void SelectAll() => Collection.SelectAll(true);

        protected virtual void UnselectAll() => Collection.SelectAll(false);

        protected virtual void ClearSelection() => Collection.ClearSelection();

        protected virtual void ApplyOnSelection(Action<T> action) => SelectedItems.ForEach(action);

        [SuppressPropertyChangedWarnings]
        protected virtual void OnSelectionChanged()
        {
            RaisePropertyChanged(nameof(SelectedItems));
            RaisePropertyChanged(nameof(SelectedItem));
            RaisePropertyChanged(nameof(SelectedWrapper));
            RaisePropertyChanged(nameof(AreAllSelected));

            var selectedItemsNotInFilteredItems = SelectedItems.Where(x => !Items.Contains(x)).ToList();

            if (selectedItemsNotInFilteredItems.Count != 0)
                Filters.Clear();
        }
    }

    [CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
    [CanBeValidatedForDeclaredClassOnly(false)]
    public class SelectionListViewModel<T> : SelectionListViewModel<T, SelectableCollection<T>>
        where T : notnull
    {
        public SelectionListViewModel(ICollection<T> source, IListParametersProvider? parametersProvider = null, SelectionMode selectionMode = SelectionMode.Multiple)
            : base(new SelectableCollection<T>(source, selectionMode: selectionMode, scheduler: Scheduler.GetUIOrCurrent()), parametersProvider) { }

        public SelectionListViewModel(IItemsProvider<T> source, IListParametersProvider? parametersProvider = null, SelectionMode selectionMode = SelectionMode.Multiple)
            : base(new SelectableCollection<T>(source, selectionMode: selectionMode, scheduler: Scheduler.GetUIOrCurrent()), parametersProvider) { }

        public SelectionListViewModel(ISourceProvider<T> source, IListParametersProvider? parametersProvider = null, SelectionMode selectionMode = SelectionMode.Multiple)
            : base(new SelectableCollection<T>(source, selectionMode: selectionMode, scheduler: Scheduler.GetUIOrCurrent()), parametersProvider) { }

        public SelectionListViewModel(IObservable<IChangeSet<T>> source, IListParametersProvider? parametersProvider = null, SelectionMode selectionMode = SelectionMode.Multiple)
            : base(new SelectableCollection<T>(source, selectionMode: selectionMode, scheduler: Scheduler.GetUIOrCurrent()), parametersProvider) { }

        public SelectionListViewModel(IListParametersProvider? parametersProvider = null, SelectionMode selectionMode = SelectionMode.Multiple)
            : base(new SelectableCollection<T>(selectionMode: selectionMode, scheduler: Scheduler.GetUIOrCurrent()), parametersProvider) { }

        protected SelectionListViewModel(
            SelectableCollection<T> collection,
            IListParametersProvider? parametersProvider = null) : base(collection, parametersProvider) { }
    }
}
