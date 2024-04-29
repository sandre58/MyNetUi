// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using My.DynamicData.Extensions;
using MyNet.Observable.Collections;
using MyNet.Observable.Collections.Providers;
using MyNet.UI.Selection.Models;
using MyNet.Utilities;
using MyNet.Utilities.Deferring;

namespace MyNet.UI.Selection
{
    public class SelectableCollection<T> : ExtendedWrapperCollection<T, SelectedWrapper<T>>
        where T : notnull
    {
        private readonly Deferrer _selectionChangedDeferrer;
        private readonly ReadOnlyObservableCollection<SelectedWrapper<T>> _selectedWrappers;
        private readonly IObservable<IChangeSet<SelectedWrapper<T>>> _observableSelectedWrappers;

        public SelectionMode SelectionMode { get; set; }

        public ReadOnlyObservableCollection<SelectedWrapper<T>> SelectedWrappers => _selectedWrappers;

        public IEnumerable<T> SelectedItems => SelectedWrappers.Select(x => x.Item);

        public event EventHandler? SelectionChanged;

        public SelectableCollection(ICollection<T> source, SelectionMode? selectionMode = null, IScheduler? scheduler = null, Func<T, SelectedWrapper<T>>? createWrapper = null)
            : this(new SourceList<T>(), source.IsReadOnly, selectionMode, scheduler, createWrapper) => AddRange(source);

        public SelectableCollection(ISourceProvider<T> source, SelectionMode? selectionMode = null, IScheduler? scheduler = null, Func<T, SelectedWrapper<T>>? createWrapper = null)
            : this(source.Connect(), selectionMode, scheduler, createWrapper) { }

        public SelectableCollection(IObservable<IChangeSet<T>> source, SelectionMode? selectionMode = null, IScheduler? scheduler = null, Func<T, SelectedWrapper<T>>? createWrapper = null)
            : this(new SourceList<T>(source), true, selectionMode, scheduler, createWrapper) { }

        public SelectableCollection(SelectionMode? selectionMode = null, IScheduler? scheduler = null, Func<T, SelectedWrapper<T>>? createWrapper = null)
            : this(new SourceList<T>(), false, selectionMode, scheduler, createWrapper) { }

        protected SelectableCollection(
            SourceList<T> sourceList,
            bool isReadOnly,
            SelectionMode? selectionMode = null,
            IScheduler? scheduler = null,
            Func<T, SelectedWrapper<T>>? createWrapper = null)
            : base(sourceList, isReadOnly, scheduler, createWrapper ?? new Func<T, SelectedWrapper<T>>(x => new(x)))
        {
            _selectionChangedDeferrer = new Deferrer(() => SelectionChanged?.Invoke(this, EventArgs.Empty));

            SelectionMode = selectionMode ?? SelectionMode.Multiple;

            var obs = ConnectWrappersSource();

            Disposables.AddRange(
            [
                obs.AutoRefresh(x => x.IsSelected)
                   .Filter(y => y.IsSelectable && y.IsSelected)
                   .ObserveOnOptional(scheduler)
                   .Bind(out _selectedWrappers)
                   .Subscribe(_ => _selectionChangedDeferrer.DeferOrExecute()),
                obs.WhenPropertyChanged(x => x.IsSelected).Subscribe(x => UpdateSelection(x.Sender))
            ]);

            _observableSelectedWrappers = _selectedWrappers.ToObservableChangeSet();
        }

        public IObservable<IChangeSet<SelectedWrapper<T>>> ConnectSelectedWrappers() => _observableSelectedWrappers;

        #region Selection

        public virtual void ChangeSelectState(T item, bool value)
        {
            var original = GetOrCreate(item);
            if (original is not null && original.IsSelectable)
                original.IsSelected = value;
        }

        public virtual void Select(T item) => ChangeSelectState(item, true);

        public virtual void Select(IEnumerable<T> items)
        {
            using (_selectionChangedDeferrer.Defer())
                items?.ToList().ForEach(Select);
        }

        public virtual void Unselect(T item) => ChangeSelectState(item, false);

        public virtual void Unselect(IEnumerable<T> items)
        {
            using (_selectionChangedDeferrer.Defer())
                items?.ToList().ForEach(Unselect);
        }

        public virtual void SelectAll(bool value)
        {
            using (_selectionChangedDeferrer.Defer())
                this.ForEach(x => ChangeSelectState(x, value));
        }

        public virtual void ClearSelection()
        {
            using (_selectionChangedDeferrer.Defer())
                WrappersSource.ForEach(x => x.IsSelected = false);
        }

        public void SetSelection(IEnumerable<T> items)
        {
            using (_selectionChangedDeferrer.Defer())
            {
                ClearSelection();
                Select(items);
            }
        }

        private void UpdateSelection(SelectedWrapper<T> wrapper)
        {
            if (SelectionMode == SelectionMode.Single && wrapper.IsSelected)
            {
                using (_selectionChangedDeferrer.Defer())
                    WrappersSource.Where(x => x.IsSelected && !x.Equals(wrapper)).ToList().ForEach(x => ChangeSelectState(x.Item, false));
            }
        }

        #endregion Selection
    }
}
