// -----------------------------------------------------------------------
// <copyright file="CompositeFilterViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Observable;
using MyNet.Observable.Translatables;
using MyNet.Utilities.Comparison;

namespace MyNet.UI.ViewModels.List.Filtering;

public class CompositeFilterViewModel(IProvideValue<string> displayName, IFilterViewModel filter, LogicalOperator logicalOperator = LogicalOperator.And) : DisplayWrapper<IFilterViewModel>(filter, displayName), ICompositeFilterViewModel
{
    public bool IsEnabled { get; set; } = true;

    public LogicalOperator Operator { get; set; } = logicalOperator;

    public CompositeFilterViewModel(IFilterViewModel filter, LogicalOperator logicalOperator = LogicalOperator.And)
        : this(filter.PropertyName, filter, logicalOperator) { }

    public CompositeFilterViewModel(string resourceKey, IFilterViewModel filter, LogicalOperator logicalOperator = LogicalOperator.And)
        : this(new StringTranslatable(resourceKey), filter, logicalOperator) { }

    public void Reset() => Item.Reset();

    protected override Wrapper<IFilterViewModel> CreateCloneInstance(IFilterViewModel item) => new CompositeFilterViewModel(DisplayName, Item, Operator) { IsEnabled = IsEnabled };
}
