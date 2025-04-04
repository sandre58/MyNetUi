// -----------------------------------------------------------------------
// <copyright file="GroupingPropertiesCollection.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using MyNet.Utilities.Collections;

namespace MyNet.UI.ViewModels.List.Grouping;

public class GroupingPropertiesCollection : ObservableKeyedCollection<string, IGroupingPropertyViewModel>
{
    public GroupingPropertiesCollection() { }

    public GroupingPropertiesCollection(IDictionary<string, string> properties)
        : base(properties.Select(x => new GroupingPropertyViewModel(x.Key, x.Value, x.Value))) { }

    public GroupingPropertiesCollection(IEnumerable<string> properties)
        : base(properties.Select(x => new GroupingPropertyViewModel(x, x, x))) { }

    protected override string GetKeyForItem(IGroupingPropertyViewModel item) => item.PropertyName;
}
