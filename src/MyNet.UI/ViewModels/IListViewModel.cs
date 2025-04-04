// -----------------------------------------------------------------------
// <copyright file="IListViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.ObjectModel;
using MyNet.UI.ViewModels.Display;
using MyNet.UI.ViewModels.List;
using MyNet.UI.ViewModels.List.Filtering;
using MyNet.UI.ViewModels.List.Grouping;
using MyNet.UI.ViewModels.List.Sorting;
using MyNet.Utilities;

namespace MyNet.UI.ViewModels;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "It's a viewModel")]
public interface IListViewModel : IDisposable, ICollection
{
    int SourceCount { get; }

    IFiltersViewModel Filters { get; }

    ICollection CurrentFilters { get; }

    bool CanFilter { get; }

    bool IsFiltered { get; }

    ISortingViewModel Sorting { get; }

    bool CanSort { get; }

    bool IsSorted { get; }

    ICollection CurrentSorting { get; }

    IGroupingViewModel Grouping { get; }

    ICollection CurrentGroups { get; }

    bool CanGroup { get; }

    bool IsGrouped { get; }

    IPagingViewModel Paging { get; }

    bool CanPage { get; }

    bool IsPaged { get; }

    IDisplayViewModel Display { get; }
}

public interface IListViewModel<T> : IListViewModel
{
    ReadOnlyObservableCollection<T> Items { get; }

    ReadOnlyObservableCollection<T> Source { get; }

    void Refresh();
}

public interface IWrapperListViewModel : IListViewModel;

public interface IWrapperListViewModel<T, TWrapper> : IWrapperListViewModel, IListViewModel<T>
    where TWrapper : IWrapper<T>
    where T : notnull
{
    ReadOnlyObservableCollection<TWrapper> Wrappers { get; }
}
