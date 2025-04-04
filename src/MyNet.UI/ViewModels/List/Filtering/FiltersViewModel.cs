// -----------------------------------------------------------------------
// <copyright file="FiltersViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using DynamicData.Binding;
using MyNet.DynamicData.Extensions;
using MyNet.Observable;
using MyNet.Observable.Attributes;
using MyNet.UI.Commands;
using MyNet.Utilities;
using MyNet.Utilities.Comparison;
using MyNet.Utilities.Deferring;
using PropertyChanged;

namespace MyNet.UI.ViewModels.List.Filtering;

[CanBeValidated(false)]
[CanSetIsModified(false)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "It's a viewModel")]
public class FiltersViewModel : EditableObject, IFiltersViewModel, ICollection<ICompositeFilterViewModel>, INotifyCollectionChanged
{
    private readonly Deferrer _filtersChangedDeferrer;
    private ICollection<ICompositeFilterViewModel>? _currentFilters;

    protected FiltersCollection CompositeFilters { get; } = [];

    public bool AutoFilter { get; set; } = true;

    public ICommand AddCommand { get; }

    public ICommand RemoveCommand { get; }

    public ICommand ClearCommand { get; }

    public ICommand ClearDirtyFiltersCommand { get; }

    public ICommand RefreshCommand { get; }

    public ICommand ResetCommand { get; }

    public ICommand ApplyCommand { get; }

    public int ActiveCount => CompositeFilters.Count(x => x.IsEnabled && !x.Item.IsEmpty());

    public event EventHandler<FiltersChangedEventArgs>? FiltersChanged;

    public FiltersViewModel()
    {
        _filtersChangedDeferrer = new Deferrer(OnFiltersChanged);

        ClearCommand = CommandsManager.Create(Clear, () => Count > 0);
        ClearDirtyFiltersCommand = CommandsManager.Create(Clear, () => Count > 0);
        AddCommand = CommandsManager.Create(Add);
        RefreshCommand = CommandsManager.Create(Refresh);
        ApplyCommand = CommandsManager.Create(Apply);
        ResetCommand = CommandsManager.Create(Reset);
        RemoveCommand = CommandsManager.CreateNotNull<ICompositeFilterViewModel>(x => Remove(x));

        Disposables.AddRange(
        [
            CompositeFilters.ToObservableChangeSet().SubscribeAll(() => _filtersChangedDeferrer.DeferOrExecute()),
            this.WhenPropertyChanged(x => x.AutoFilter).Subscribe(_ => _filtersChangedDeferrer.DeferOrExecute())
        ]);

        CompositeFilters.CollectionChanged += HandleCollectionChanged;
    }

    protected IDisposable Defer() => _filtersChangedDeferrer.Defer();

    protected void DeferOrExecute() => _filtersChangedDeferrer.DeferOrExecute();

    protected virtual ICompositeFilterViewModel CreateCompositeFilter(IFilterViewModel filter, LogicalOperator logicalOperator = LogicalOperator.And)
        => new CompositeFilterViewModel(filter, logicalOperator);

    public virtual void Add() => throw new NotImplementedException();

    public virtual void Add(IFilterViewModel filter, LogicalOperator logicalOperator = LogicalOperator.And) => CompositeFilters.Add(CreateCompositeFilter(filter, logicalOperator));

    public virtual void AddRange(IEnumerable<IFilterViewModel> filters) => CompositeFilters.AddRange(filters);

    public virtual void Clear()
    {
        CompositeFilters.Clear();

        if (!AutoFilter) Apply();
    }

    public virtual void ClearDirtyFilters() => CompositeFilters.Clear();

    public virtual void Refresh() => CompositeFilters.Set(_currentFilters);

    public virtual void Set(IEnumerable<IFilterViewModel> filters)
    {
        using (_filtersChangedDeferrer.Defer())
            CompositeFilters.Set(filters.Select(x => CreateCompositeFilter(x)));
    }

    public virtual void Reset()
    {
        using (_filtersChangedDeferrer.Defer())
            CompositeFilters.ForEach(x => x.Reset());
    }

    private void Apply() => ApplyFilters(CompositeFilters);

    protected virtual void ApplyFilters(IEnumerable<ICompositeFilterViewModel> compositeFilters)
    {
        var list = compositeFilters.ToList();
        _currentFilters = [.. list];
        FiltersChanged?.Invoke(this, new FiltersChangedEventArgs(list));
    }

    [SuppressPropertyChangedWarnings]
    protected virtual void OnFiltersChanged()
    {
        OnPropertyChanged(nameof(ActiveCount));
        OnPropertyChanged(nameof(Count));

        if (AutoFilter) Apply();
    }

    #region ICollection

    public int Count => CompositeFilters.Count;

    public virtual bool IsReadOnly => false;

    public virtual void Add(ICompositeFilterViewModel item) => IsReadOnly.IfFalse(() => CompositeFilters.Add(item));

    public virtual bool Remove(ICompositeFilterViewModel item) => !IsReadOnly && CompositeFilters.Remove(item);

    public bool Contains(ICompositeFilterViewModel item) => CompositeFilters.Contains(item);

    public void CopyTo(ICompositeFilterViewModel[] array, int arrayIndex) => CompositeFilters.CopyTo(array, arrayIndex);

    public IEnumerator<ICompositeFilterViewModel> GetEnumerator() => CompositeFilters.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => CompositeFilters.GetEnumerator();

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
}
