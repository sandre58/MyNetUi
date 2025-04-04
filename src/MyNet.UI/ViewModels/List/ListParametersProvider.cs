// -----------------------------------------------------------------------
// <copyright file="ListParametersProvider.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MyNet.UI.ViewModels.Display;
using MyNet.UI.ViewModels.List.Filtering;
using MyNet.UI.ViewModels.List.Grouping;
using MyNet.UI.ViewModels.List.Paging;
using MyNet.UI.ViewModels.List.Sorting;

namespace MyNet.UI.ViewModels.List;

public class ListParametersProvider(IDictionary<string, ListSortDirection> defaultSortProperties) : IListParametersProvider
{
    public static ListParametersProvider Default => new();

    public ListParametersProvider()
        : this([]) { }

    public ListParametersProvider(string defaultSortingProperty, ListSortDirection direction = ListSortDirection.Ascending)
        : this(new Dictionary<string, ListSortDirection> { { defaultSortingProperty, direction } }) { }

    public ListParametersProvider(IEnumerable<string> defaultSortingProperties)
        : this(defaultSortingProperties.ToDictionary(x => x, _ => ListSortDirection.Ascending)) { }

    public virtual IFiltersViewModel ProvideFilters() => new FiltersViewModel();

    public virtual ISortingViewModel ProvideSorting() => new SortingViewModel(defaultSortProperties);

    public virtual IGroupingViewModel ProvideGrouping() => new GroupingViewModel();

    public virtual IPagingViewModel ProvidePaging() => new PagingViewModel();

    public virtual IDisplayViewModel ProvideDisplay() => new DisplayViewModel();
}
