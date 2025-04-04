// -----------------------------------------------------------------------
// <copyright file="SortingPropertiesCollection.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using MyNet.Utilities.Collections;

namespace MyNet.UI.ViewModels.List.Sorting;

public class SortingPropertiesCollection : ObservableKeyedCollection<string, ISortingPropertyViewModel>
{
    public SortingPropertiesCollection() { }

    public SortingPropertiesCollection(IDictionary<string, string> properties)
        : base(properties.Select(x => new SortingPropertyViewModel(x.Key, x.Value) { IsEnabled = false })) { }

    public SortingPropertiesCollection(IEnumerable<string> properties)
        : base(properties.Select(x => new SortingPropertyViewModel(x) { IsEnabled = false })) { }

    protected override string GetKeyForItem(ISortingPropertyViewModel item) => item.PropertyName;
}
