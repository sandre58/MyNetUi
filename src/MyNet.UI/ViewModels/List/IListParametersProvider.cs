// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.UI.ViewModels;
using MyNet.UI.ViewModels.Filters;
using MyNet.UI.ViewModels.Grouping;
using MyNet.UI.ViewModels.Paging;
using MyNet.UI.ViewModels.Sorting;

namespace MyNet.UI.ViewModels.List
{
    public interface IListParametersProvider
    {
        IFiltersViewModel ProvideFilters();

        ISortingViewModel ProvideSorting();

        IGroupingViewModel ProvideGrouping();

        IPagingViewModel ProvidePaging();

        IDisplayViewModel ProvideDisplay();
    }
}
