// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
using DynamicData.Operators;
using MyNet.Humanizer;
using MyNet.Observable.Attributes;
using MyNet.Observable.Collections;
using MyNet.Observable.Collections.Filters;
using MyNet.Observable.Collections.Sorting;
using MyNet.UI.Collections;
using MyNet.UI.Commands;
using MyNet.UI.Dialogs;
using MyNet.UI.Dialogs.Models;
using MyNet.UI.Resources;
using MyNet.UI.ViewModels.List.Filtering;
using MyNet.UI.ViewModels.List.Grouping;
using MyNet.UI.ViewModels.List.Sorting;
using MyNet.UI.ViewModels.Workspace;
using MyNet.Utilities;
using PropertyChanged;

namespace MyNet.UI.ViewModels.List
{
    [CanBeValidatedForDeclaredClassOnly(false)]
    [CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
    public abstract class ListViewModelBase<T, TCollection> : NavigableWorkspaceViewModel, IListViewModel<T>, IEnumerable<T>
        where TCollection : ExtendedCollection<T>
        where T : notnull
    {
        private readonly BehaviorSubject<PageRequest> _pager = new(new PageRequest(1, int.MaxValue));
        private readonly UiObservableCollection<T> _pagedItems = [];
        private IDisposable? _pagedDisposable;

        protected ListViewModelBase(
            TCollection collection,
            IListParametersProvider? parametersProvider = null)
        {
            var parameters = parametersProvider ?? ListParametersProvider.Default;
            Filters = parameters.ProvideFilters();
            Sorting = parameters.ProvideSorting();
            Display = parameters.ProvideDisplay();
            Grouping = parameters.ProvideGrouping();
            Paging = parameters.ProvidePaging();
            Collection = collection;
            PagedItems = new(_pagedItems);

            ShowFiltersCommand = CommandsManager.Create(ToggleFilters);
            OpenCommand = CommandsManager.Create<T>(x => Open(x), CanOpenItem);
            ClearCommand = CommandsManager.Create(async () => await ClearAsync().ConfigureAwait(false), () => CanRemoveItems(Collection.Source));
            AddCommand = CommandsManager.Create(async () => await AddAsync().ConfigureAwait(false), () => CanAdd);
            EditCommand = CommandsManager.Create<T>(async x => await EditAsync(x).ConfigureAwait(false), CanEditItem);
            EditRangeCommand = CommandsManager.CreateNotNull<IEnumerable<T>>(async x => await EditRangeAsync(x).ConfigureAwait(false), CanEditItems);
            RemoveCommand = CommandsManager.Create<T>(async x => await RemoveAsync(x).ConfigureAwait(false), x => CanRemoveItems(new[] { x }.NotNull()));
            RemoveRangeCommand = CommandsManager.CreateNotNull<IEnumerable<T>>(async x => await RemoveRangeAsync(x).ConfigureAwait(false), CanRemoveItems);
            PreviousCommand = CommandsManager.Create(() => SelectedItem = GetPrevious(), () => GetPrevious() is not null);
            NextCommand = CommandsManager.Create(() => SelectedItem = GetNext(), () => GetNext() is not null);
            FirstCommand = CommandsManager.Create(() => SelectedItem = Items.FirstOrDefault(), () => Items.FirstOrDefault() is not null);
            LastCommand = CommandsManager.Create(() => SelectedItem = Items.LastOrDefault(), () => Items.LastOrDefault() is not null);

            Disposables.AddRange(
            [
                System.Reactive.Linq.Observable.FromEventPattern<FiltersChangedEventArgs>(x => Filters.FiltersChanged += x, x => Filters.FiltersChanged -= x).Subscribe(x => OnFiltersChanged(x.EventArgs.Filters)),
                System.Reactive.Linq.Observable.FromEventPattern<SortingChangedEventArgs>(x => Sorting.SortingChanged += x, x => Sorting.SortingChanged -= x).Subscribe(x => OnSortChanged(x.EventArgs.SortingProperties)),
                System.Reactive.Linq.Observable.FromEventPattern<GroupingChangedEventArgs>(x => Grouping.GroupingChanged += x, x => Grouping.GroupingChanged -= x).Subscribe(x => OnGroupChanged(x.EventArgs.GroupProperties)),
                System.Reactive.Linq.Observable.FromEventPattern<PagingChangedEventArgs>(x => Paging.PagingChanged += x, x => Paging.PagingChanged -= x).Subscribe(x => OnPagingChanged(x.EventArgs.Page, x.EventArgs.PageSize)),
                Collection.WhenPropertyChanged(x => x.Count).Subscribe(_ => RaisePropertyChanged(nameof(Count))),
                Collection.WhenPropertyChanged(x => x.Count).Subscribe(_ => RaisePropertyChanged(nameof(SourceCount))),
                Collection,
            ]);

            if (Filters is IDialogViewModel dialog)
                dialog.CloseRequest += (sender, e) => ShowFilters = false;

            Filters.Reset();
            Sorting.Reset();
            Grouping.Reset();
        }

        [CanSetIsModified(true)]
        [CanBeValidated(true)]
        protected TCollection Collection { get; }

        public ReadOnlyObservableCollection<T> Items => Collection.Items;

        public ReadOnlyObservableCollection<T> Source => Collection.Source;

        public ReadOnlyObservableCollection<T> PagedItems { get; }

        public int SourceCount => Source.Count;

        [CanSetIsModified(true)]
        [DoNotCheckEquality]
        public T? SelectedItem { get; set; }

        [CanNotify(false)]
        public IFiltersViewModel Filters { get; }

        [CanNotify(false)]
        public ISortingViewModel Sorting { get; }

        [CanNotify(false)]
        public IGroupingViewModel Grouping { get; }

        [CanNotify(false)]
        public IPagingViewModel Paging { get; }

        [CanNotify(false)]
        public IDisplayViewModel Display { get; }

        public ICollection CurrentFilters => Collection.Filters;

        public ICollection CurrentSorting => Collection.SortingProperties;

        public UiObservableCollection<IGroupingPropertyViewModel> CurrentGroups { get; private set; } = [];

        ICollection IListViewModel.CurrentGroups => CurrentGroups;

        public bool IsFiltered => Collection.Filters.Any();

        public bool IsSorted => Collection.SortingProperties.Any();

        public bool IsGrouped => CurrentGroups.Count > 0;

        public bool IsPaged => CanPage && Paging.TotalPages > 1;

        public bool ShowFilters { get; set; }

        public virtual bool CanFilter { get; set; } = true;

        public virtual bool CanSort { get; set; } = true;

        public virtual bool CanGroup { get; set; } = true;

        public virtual bool CanPage { get; set; } = false;

        public virtual bool CanOpen { get; set; } = true;

        public virtual bool CanAdd { get; set; } = true;

        public virtual bool CanEdit { get; set; } = true;

        public virtual bool CanRemove { get; set; } = true;

        public ICommand ShowFiltersCommand { get; protected set; }

        public ICommand OpenCommand { get; protected set; }

        public ICommand AddCommand { get; protected set; }

        public ICommand EditCommand { get; protected set; }

        public ICommand EditRangeCommand { get; protected set; }

        public ICommand RemoveCommand { get; protected set; }

        public ICommand RemoveRangeCommand { get; protected set; }

        public ICommand ClearCommand { get; }

        public ICommand PreviousCommand { get; private set; }

        public ICommand NextCommand { get; private set; }

        public ICommand FirstCommand { get; private set; }

        public ICommand LastCommand { get; private set; }

        public event EventHandler<SortedEventArgs>? Sorted;

        public event EventHandler<FilteredEventArgs>? Filtered;

        public event EventHandler<EventArgs>? Grouped;

        public event EventHandler<EventArgs>? Paged;

        [SuppressPropertyChangedWarnings]
        protected virtual void OnFiltersChanged(IEnumerable<ICompositeFilterViewModel> filters)
        {
            if (!CanFilter) return;

            Collection.Filters.Set(filters.Where(x => x.IsEnabled && !x.Item.IsEmpty()).Select(x => new CompositeFilter(x.Item, x.Operator)));
            RaisePropertyChanged(nameof(IsFiltered));

            Filtered?.Invoke(this, new FilteredEventArgs(Collection.Filters));
        }

        [SuppressPropertyChangedWarnings]
        private void OnSortChanged(IEnumerable<ISortingPropertyViewModel> sort)
        {
            if (!CanSort) return;

            Collection.SortingProperties.Set(sort.Where(x => x.IsEnabled).OrderBy(x => x.Order).Select(x => new SortingProperty(x.PropertyName, x.Direction)));
            RaisePropertyChanged(nameof(IsSorted));

            Sorted?.Invoke(this, new SortedEventArgs(Collection.SortingProperties));
        }

        protected virtual void ToggleFilters() => ShowFilters = !ShowFilters;

        [SuppressPropertyChangedWarnings]
        private void OnGroupChanged(IEnumerable<IGroupingPropertyViewModel> groupProperties)
        {
            if (!CanGroup) return;

            var activeGroupProperties = groupProperties.Where(x => x.IsEnabled && !string.IsNullOrEmpty(x.PropertyName)).OrderBy(x => x.Order).ToList();

            if (CurrentGroups.Count == 0 && activeGroupProperties.Count == 0) return;

            OnSortChanged(activeGroupProperties.Select((x, index) => new SortingPropertyViewModel(x.SortingPropertyName, order: index)).Concat(Collection.SortingProperties.Select((x, index) => new SortingPropertyViewModel(x.PropertyName, x.Direction, index + activeGroupProperties.Count))));
            CurrentGroups.Set(activeGroupProperties);
            RaisePropertyChanged(nameof(IsGrouped));

            Grouped?.Invoke(this, EventArgs.Empty);
        }

        [SuppressPropertyChangedWarnings]
        private void OnPagingChanged(int page, int pageSize)
        {
            if (!CanPage) return;

            _pager.OnNext(new PageRequest(page, pageSize));
            RaisePropertyChanged(nameof(IsPaged));

            Paged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdatePaging(IPageResponse response) => Paging.Update(new(response.TotalSize == 0 ? 1 : response.TotalSize, response.Pages, response.Page));

        protected virtual void OnCanPageChanged()
        {
            _pagedDisposable?.Dispose();

            if (CanPage)
            {
                _pagedItems.Clear();
                _pagedDisposable = SubscribePager(_pager);
                OnPagingChanged(Paging.CurrentPage, Paging.PageSize);
            }
        }

        protected virtual IDisposable SubscribePager(IObservable<PageRequest> pager) => Collection.Connect().Page(pager).Do(x => UpdatePaging(x.Response)).Bind(_pagedItems).Subscribe();

        #region Navigation

        public T? GetPrevious()
        {
            if (SelectedItem is null) return default;

            var currentIndex = Items.IndexOf(SelectedItem);

            return Items.GetByIndex(currentIndex - 1);
        }

        public T? GetNext()
        {
            if (SelectedItem is null) return default;

            var currentIndex = Items.IndexOf(SelectedItem);

            return Items.GetByIndex(currentIndex + 1);
        }

        #endregion

        #region Open

        protected virtual bool CanOpenItem(T? item) => CanOpen && item is not null;

        public void Open(T? item, int? selectedTab = null)
        {
            if (!CanOpen || item is null) return;

            OpenCore(item, selectedTab);
        }

        protected virtual void OpenCore(T item, int? selectedTab = null) { }

        #endregion

        #region Add

        public virtual async Task AddAsync()
        {
            if (!CanAdd) return;

            var item = await CreateNewItemAsync().ConfigureAwait(false);

            if (item is not null)
            {
                await ExecuteAsync(() =>
                {
                    AddItemCore(item);
                    OnAddCompleted(item);
                });
            }
        }

        protected virtual async Task<T?> CreateNewItemAsync() => await Task.FromResult(Activator.CreateInstance<T>());

        protected virtual void AddItemCore(T item) => Collection.Add(item);

        protected virtual void OnAddCompleted(T item) { }

        #endregion

        #region Edit

        protected virtual bool CanEditItem(T? item) => CanEdit && item is not null;

        protected virtual bool CanEditItems(IEnumerable<T> items) => CanEdit && items.Any();

        public virtual async Task EditAsync(T? oldItem)
        {
            if (!CanEditItem(oldItem) || oldItem is null) return;

            var newItem = await UpdateItemAsync(oldItem).ConfigureAwait(false);

            if (newItem is not null)
            {
                await ExecuteAsync(() =>
                {
                    EditItemCore(oldItem, newItem);
                    OnEditCompleted(oldItem, newItem);
                });
            }
        }

        protected virtual void EditItemCore(T oldItem, T newItem)
        {
            Collection.Remove(oldItem);
            Collection.Add(newItem);
        }

        protected virtual async Task<T?> UpdateItemAsync(T oldItem) => await Task.FromResult(oldItem).ConfigureAwait(false);

        protected virtual void OnEditCompleted(T oldItem, T newItem) { }

        public virtual async Task EditRangeAsync(IEnumerable<T> oldItems)
        {
            if (!CanEditItems(oldItems)) return;

            var newItems = await UpdateRangeAsync(oldItems).ConfigureAwait(false);

            if (newItems.Any())
            {
                await ExecuteAsync(() => OnEditRangeCompleted(oldItems, newItems));
            }
        }

        protected virtual async Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> oldItems) => await Task.FromResult(oldItems).ConfigureAwait(false);

        protected virtual void OnEditRangeCompleted(IEnumerable<T> oldItems, IEnumerable<T> newItems) { }

        #endregion

        #region Remove

        protected virtual bool CanRemoveItems(IEnumerable<T> items) => CanRemove && items.Any();

        protected virtual async Task ClearAsync() => await RemoveRangeAsync(Collection.Source).ConfigureAwait(false);

        public async Task RemoveAsync(T? item)
        {
            var items = new[] { item }.NotNull();

            if (!CanRemoveItems(items)) return;

            await RemoveRangeAsync(items).ConfigureAwait(false);
        }

        public virtual async Task RemoveRangeAsync(IEnumerable<T> oldItems)
        {
            if (!CanRemove) return;

            var cancelEventArgs = new CancelEventArgs(false);
            await OnRemovingRequestedAsync(oldItems, cancelEventArgs).ConfigureAwait(false);

            if (!cancelEventArgs.Cancel && oldItems.Any())
            {
                await ExecuteAsync(() =>
                {
                    RemoveItemsCore(oldItems);
                    OnRemoveCompleted(oldItems);
                });
            }
        }

        protected virtual void RemoveItemsCore(IEnumerable<T> oldItems) => Collection.RemoveMany(oldItems);

        protected virtual async Task OnRemovingRequestedAsync(IEnumerable<T> oldItems, CancelEventArgs cancelEventArgs)
            => cancelEventArgs.Cancel = await DialogManager.ShowQuestionAsync(nameof(MessageResources.XItemsRemovingQuestion).TranslateWithCountAndOptionalFormat(oldItems.Count())!, UiResources.Removing).ConfigureAwait(false) != MessageBoxResult.Yes;

        protected virtual void OnRemoveCompleted(IEnumerable<T> oldItems) { }

        #endregion

        #region ICollection

        public int Count => Items.Count;

        public bool IsSynchronized => true;

        public object SyncRoot { get; } = new();

        public void CopyTo(Array array, int index) => Collection.CopyTo((T[])array, index);

        public IEnumerator GetEnumerator() => Collection.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => Collection.GetEnumerator();

        #endregion

        #region Refresh

        #endregion

        protected override void Cleanup()
        {
            _pagedDisposable?.Dispose();
            base.Cleanup();
        }
    }
}
