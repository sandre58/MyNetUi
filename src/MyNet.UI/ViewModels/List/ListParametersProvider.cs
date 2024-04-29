// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MyNet.UI.ViewModels;
using MyNet.UI.ViewModels.Display;
using MyNet.UI.ViewModels.List.Filtering;
using MyNet.UI.ViewModels.List.Grouping;
using MyNet.UI.ViewModels.List.Paging;
using MyNet.UI.ViewModels.List.Sorting;

namespace MyNet.UI.ViewModels.List
{
    public class ListParametersProvider : IListParametersProvider
    {
        public static ListParametersProvider Default => new();

        private readonly IDictionary<string, ListSortDirection> _defaultSortProperties;

        public ListParametersProvider() : this([]) { }

        public ListParametersProvider(string defaultSortingProperty) : this(new[] { defaultSortingProperty }) { }

        public ListParametersProvider(IEnumerable<string> defaultSortingProperties) : this(defaultSortingProperties.ToDictionary(x => x, _ => ListSortDirection.Ascending)) { }

        public ListParametersProvider(IDictionary<string, ListSortDirection> defaultSortProperties) => _defaultSortProperties = defaultSortProperties;

        public virtual IFiltersViewModel ProvideFilters() => new FiltersViewModel();

        public virtual ISortingViewModel ProvideSorting() => new SortingViewModel(_defaultSortProperties);

        public virtual IGroupingViewModel ProvideGrouping() => new GroupingViewModel();

        public virtual IPagingViewModel ProvidePaging() => new PagingViewModel();

        public virtual IDisplayViewModel ProvideDisplay() => new DisplayViewModel();
    }
}
