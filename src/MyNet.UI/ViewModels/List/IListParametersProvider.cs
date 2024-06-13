// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.UI.ViewModels.List.Filtering;
using MyNet.UI.ViewModels.List.Grouping;
using MyNet.UI.ViewModels.List.Sorting;

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
