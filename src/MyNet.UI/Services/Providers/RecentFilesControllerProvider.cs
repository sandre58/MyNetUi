// -----------------------------------------------------------------------
// <copyright file="RecentFilesControllerProvider.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Windows.Input;
using MyNet.Observable;
using MyNet.UI.ViewModels.FileHistory;
using MyNet.UI.ViewModels.List;
using MyNet.UI.ViewModels.List.Filtering;
using MyNet.UI.ViewModels.List.Filtering.Filters;
using MyNet.UI.ViewModels.List.Sorting;
using MyNet.Utilities.Comparison;

namespace MyNet.UI.Services.Providers;

public class RecentFilesControllerProvider : ListParametersProvider
{
    public override IFiltersViewModel ProvideFilters() => new RecentFilesFilters();

    public override ISortingViewModel ProvideSorting() => new RecentFilesSorting();
}

public class RecentFilesFilters : ObservableObject, IFiltersViewModel
{
    public event EventHandler<FiltersChangedEventArgs>? FiltersChanged;

    public string Text { get; set; } = string.Empty;

    public void Refresh() => OnTextChanged();

    public void Clear() => Text = string.Empty;

    public void Reset() => Text = string.Empty;

    protected virtual void OnTextChanged()
        => FiltersChanged?.Invoke(this, new(
        [
            new CompositeFilterViewModel(new StringFilterViewModel(nameof(RecentFileViewModel.Name)) { Value = Text }),
            new CompositeFilterViewModel(new FileNameFilterViewModel(nameof(RecentFileViewModel.Path)) { Value = Text }, LogicalOperator.Or)
        ]));
}

public class RecentFilesSorting : ObservableObject, ISortingViewModel
{
    public RecentFilesSortingProperty SortingProperty { get; set; } = RecentFilesSortingProperty.LastAccessDate;

    public bool IsAscending { get; set; }

    ICommand ISortingViewModel.ApplyCommand => throw new NotImplementedException();

    public event EventHandler<SortingChangedEventArgs>? SortingChanged;

    protected virtual void OnSortingPropertyChanged() => Sort();

    protected virtual void OnIsAscendingChanged() => Sort();

    private void Sort()
        => SortingChanged?.Invoke(this, new([new SortingPropertyViewModel(SortingProperty.ToString(), IsAscending ? ListSortDirection.Ascending : ListSortDirection.Descending)]));

    public void Reset()
    {
        SortingProperty = RecentFilesSortingProperty.LastAccessDate;
        IsAscending = false;
        Sort();
    }
}

public enum RecentFilesSortingProperty
{
    Name,

    LastAccessDate
}
