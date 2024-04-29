// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using MyNet.Observable.Collections;
using MyNet.UI.ViewModels.Filters;

namespace MyNet.UI.ViewModels.List.Filtering
{
    public class FiltersCollection : ThreadSafeObservableCollection<ICompositeFilterViewModel>
    {
        public FiltersCollection() { }

        public FiltersCollection(IEnumerable<ICompositeFilterViewModel> filters) : base(filters) { }

        public FiltersCollection(IEnumerable<IFilterViewModel> properties) : base(properties.Select(x => new CompositeFilterViewModel(x))) { }

        public void Add(IFilterViewModel filter) => Add(new CompositeFilterViewModel(filter));

        public void AddRange(IEnumerable<IFilterViewModel> filters) => AddRange(filters.Select(x => new CompositeFilterViewModel(x)));
    }
}
