// -----------------------------------------------------------------------
// <copyright file="SortingPropertyViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using MyNet.Observable;
using MyNet.Observable.Translatables;

namespace MyNet.UI.ViewModels.List.Sorting;

public class SortingPropertyViewModel(IProvideValue<string> displayName, string propertyName, ListSortDirection direction = ListSortDirection.Ascending, int order = -1) : DisplayWrapper<string>(propertyName, displayName), ISortingPropertyViewModel
{
    public SortingPropertyViewModel(string propertyName, ListSortDirection direction = ListSortDirection.Ascending, int order = -1)
        : this(propertyName, propertyName, direction, order) { }

    public SortingPropertyViewModel(string resourceKey, string propertyName, ListSortDirection direction = ListSortDirection.Ascending, int order = -1)
        : this(new StringTranslatable(resourceKey), propertyName, direction, order) { }

    public string PropertyName => Item;

    public ListSortDirection Direction { get; set; } = direction;

    public bool IsEnabled { get; set; } = true;

    public int Order { get; set; } = order;

    protected override DisplayWrapper<string> CreateCloneInstance(string item) => new SortingPropertyViewModel(DisplayName, item, Direction) { IsEnabled = IsEnabled, Order = Order };
}
