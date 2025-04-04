// -----------------------------------------------------------------------
// <copyright file="StringFilterViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Utilities.Comparison;

namespace MyNet.UI.ViewModels.List.Filtering.Filters;

public class StringFilterViewModel(string propertyName, StringOperator filterMode = StringOperator.Contains, bool caseSensitive = false) : FilterViewModel(propertyName)
{
    public StringOperator Operator { get; set; } = filterMode;

    public string? Value { get; set; }

    public bool CaseSensitive { get; set; } = caseSensitive;

    protected override bool IsMatchProperty(object? toCompare)
    {
        if (toCompare is null)
            return Operator == StringOperator.Is && Value is null;

        var toStringCompare = toCompare.ToString() ?? string.Empty;

        if (Value is null)
            return false;

        var value = Value;
        if (CaseSensitive)
        {
            return Operator switch
            {
                StringOperator.Contains => toStringCompare.Contains(value, StringComparison.OrdinalIgnoreCase),

                StringOperator.StartsWith => toStringCompare.StartsWith(value, StringComparison.InvariantCultureIgnoreCase),

                StringOperator.EndsWith => toStringCompare.EndsWith(value, StringComparison.InvariantCultureIgnoreCase),

                StringOperator.Is => toStringCompare.Equals(value, StringComparison.OrdinalIgnoreCase),

                StringOperator.IsNot => !toStringCompare.Equals(value, StringComparison.OrdinalIgnoreCase),

                _ => throw new NotImplementedException()
            };
        }

        value = Value.ToUpperInvariant();
        toStringCompare = toStringCompare.ToUpperInvariant();

        return Operator switch
        {
            StringOperator.Contains => toStringCompare.Contains(value, StringComparison.OrdinalIgnoreCase),

            StringOperator.StartsWith => toStringCompare.StartsWith(value, StringComparison.InvariantCultureIgnoreCase),

            StringOperator.EndsWith => toStringCompare.EndsWith(value, StringComparison.InvariantCultureIgnoreCase),

            StringOperator.Is => toStringCompare.Equals(value, StringComparison.OrdinalIgnoreCase),

            StringOperator.IsNot => !toStringCompare.Equals(value, StringComparison.OrdinalIgnoreCase),

            _ => throw new NotImplementedException()
        };
    }

    public override bool IsEmpty() => string.IsNullOrEmpty(Value);

    public override void Reset() => Value = string.Empty;

    public override bool Equals(object? obj) => obj is StringFilterViewModel o && base.Equals(obj) && Operator == o.Operator && CaseSensitive == o.CaseSensitive;

    public override int GetHashCode() => base.GetHashCode();

    public override void SetFrom(object? from)
    {
        if (from is not StringFilterViewModel other)
            return;
        Operator = other.Operator;
        CaseSensitive = other.CaseSensitive;
        Value = other.Value;
    }

    protected override FilterViewModel CreateCloneInstance() => new StringFilterViewModel(PropertyName, Operator, CaseSensitive);
}
