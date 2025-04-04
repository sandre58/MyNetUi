// -----------------------------------------------------------------------
// <copyright file="SelectableFilterViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using MyNet.Utilities.Comparison;

namespace MyNet.UI.ViewModels.List.Filtering.Filters;

public abstract class SelectableFilterViewModel<TAllowedValues>(string propertyName, IEnumerable<TAllowedValues>? allowedValues = null) : FilterViewModel(propertyName)
{
    public BinaryOperator Operator { get; set; } = BinaryOperator.Is;

    public IEnumerable<TAllowedValues>? AvailableValues { get; set; } = allowedValues;

    public override void SetFrom(object? from)
    {
        if (from is SelectableFilterViewModel<TAllowedValues> other)
            Operator = other.Operator;
    }
}
