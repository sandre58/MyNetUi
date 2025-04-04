// -----------------------------------------------------------------------
// <copyright file="SelectedValueFilterViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MyNet.Utilities.Comparison;

namespace MyNet.UI.ViewModels.List.Filtering.Filters;

public class SelectedValueFilterViewModel(string propertyName, IEnumerable<object> allowedValues) : SelectedValueFilterViewModel<object, object>(propertyName, allowedValues)
{
    protected override FilterViewModel CreateCloneInstance() => new SelectedValueFilterViewModel(PropertyName, AvailableValues ?? []);
}

public class SelectedValueFilterViewModel<TValue, TAllowedValues>(string propertyName, IEnumerable<TAllowedValues> allowedValues) : SelectableFilterViewModel<TAllowedValues>(propertyName, allowedValues)
{
    public TValue? Value { get; set; }

    protected override bool IsMatchProperty(object? toCompare)
    {
        var result = (toCompare is null && Value is null) || (Value is not null && Value.Equals(toCompare));

        return Operator switch
        {
            BinaryOperator.Is => result,

            BinaryOperator.IsNot => !result,

            _ => throw new NotImplementedException()
        };
    }

    public override bool IsEmpty() => Value is null;

    public override void Reset() => Value = default;

    public override void SetFrom(object? from)
    {
        base.SetFrom(from);

        if (from is SelectedValueFilterViewModel<TValue, TAllowedValues> other)
            Value = other.Value;
    }

    protected override FilterViewModel CreateCloneInstance() => new SelectedValueFilterViewModel<TValue, TAllowedValues>(PropertyName, AvailableValues ?? []);
}
