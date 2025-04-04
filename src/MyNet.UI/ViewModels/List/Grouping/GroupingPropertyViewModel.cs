// -----------------------------------------------------------------------
// <copyright file="GroupingPropertyViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Observable;
using MyNet.Observable.Translatables;

namespace MyNet.UI.ViewModels.List.Grouping;

public class GroupingPropertyViewModel(IProvideValue<string> displayName, string propertyName, string sortingPropertyName, int order = -1) : DisplayWrapper<string>(propertyName, displayName), IGroupingPropertyViewModel
{
    public GroupingPropertyViewModel(string resourceKey, string propertyName, string? sortingPropertyName = null, int order = -1)
        : this(new StringTranslatable(resourceKey), propertyName, sortingPropertyName ?? propertyName, order) { }

    public string PropertyName => Item;

    public string SortingPropertyName { get; } = sortingPropertyName;

    public bool IsEnabled { get; set; }

    public int Order { get; set; } = order;
}
