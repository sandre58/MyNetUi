// -----------------------------------------------------------------------
// <copyright file="SpeedFiltersViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace MyNet.UI.ViewModels.List.Filtering;

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
                var similarSpeedFilter = CompositeFilters.FirstOrDefault(x => filter.IsSimilar(x.Item));

                similarSpeedFilter?.Item.SetFrom(filter);
            }
        }
    }
}
