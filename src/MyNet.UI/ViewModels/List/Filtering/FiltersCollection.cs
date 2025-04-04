// -----------------------------------------------------------------------
// <copyright file="FiltersCollection.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using MyNet.UI.Collections;

namespace MyNet.UI.ViewModels.List.Filtering;

public class FiltersCollection : UiObservableCollection<ICompositeFilterViewModel>
{
    public FiltersCollection() { }

    public FiltersCollection(IEnumerable<ICompositeFilterViewModel> filters)
        : base(filters) { }

    public FiltersCollection(IEnumerable<IFilterViewModel> properties)
        : base(properties.Select(x => new CompositeFilterViewModel(x))) { }

    public void Add(IFilterViewModel filter) => Add(new CompositeFilterViewModel(filter));

    public void AddRange(IEnumerable<IFilterViewModel> filters) => AddRange(filters.Select(x => new CompositeFilterViewModel(x)));
}
