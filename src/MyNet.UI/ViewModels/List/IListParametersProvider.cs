// -----------------------------------------------------------------------
// <copyright file="IListParametersProvider.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.UI.ViewModels.Display;
using MyNet.UI.ViewModels.List.Filtering;
using MyNet.UI.ViewModels.List.Grouping;
using MyNet.UI.ViewModels.List.Sorting;

namespace MyNet.UI.ViewModels.List;

public interface IListParametersProvider
{
    IFiltersViewModel ProvideFilters();

    ISortingViewModel ProvideSorting();

    IGroupingViewModel ProvideGrouping();

    IPagingViewModel ProvidePaging();

    IDisplayViewModel ProvideDisplay();
}
