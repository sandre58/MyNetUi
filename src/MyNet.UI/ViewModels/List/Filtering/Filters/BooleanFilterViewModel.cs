// -----------------------------------------------------------------------
// <copyright file="BooleanFilterViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.UI.ViewModels.List.Filtering.Filters;

public class BooleanFilterViewModel(string propertyName) : FilterViewModel(propertyName)
{
    public bool? Value { get; set; }

    public bool AllowNullValue { get; set; }

    protected override bool IsMatchProperty(object? toCompare) => (bool?)toCompare == Value;

    public override bool IsEmpty() => Value is null;

    public override void Reset() => Value = AllowNullValue ? null : false;

    public override void SetFrom(object? from)
    {
        if (from is not BooleanFilterViewModel other)
            return;
        AllowNullValue = other.AllowNullValue;
        Value = other.Value;
    }

    protected override FilterViewModel CreateCloneInstance() => new BooleanFilterViewModel(PropertyName) { AllowNullValue = AllowNullValue };
}
