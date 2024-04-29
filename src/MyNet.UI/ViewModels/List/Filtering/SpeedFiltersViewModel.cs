// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using MyNet.UI.ViewModels.Filters;

namespace MyNet.UI.ViewModels.List.Filtering
{
    public class SpeedFiltersViewModel : FiltersViewModel
    {
        public SpeedFiltersViewModel() { }

        public SpeedFiltersViewModel(IEnumerable<IFilterViewModel> filters) => CompositeFilters.AddRange(filters.Select(x => CreateCompositeFilter(x)));

        public override void Clear() => Reset();

        public override void Set(IEnumerable<IFilterViewModel> filters)
        {
            using (Defer())
            {
                Clear();

                foreach (var filter in filters)
                {
                    if (filter is null) continue;

                    var similarSpeedFilter = CompositeFilters.FirstOrDefault(x => filter.IsSimilar(x.Item));

                    similarSpeedFilter?.Item.SetFrom(filter);
                }
            }
        }
    }
}
