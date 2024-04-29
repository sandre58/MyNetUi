// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using MyNet.UI.ViewModels.Filters;
using MyNet.UI.ViewModels.Grouping;
using MyNet.UI.ViewModels.Paging;
using MyNet.UI.ViewModels.Sorting;

namespace MyNet.UI.ViewModels
{
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
}
