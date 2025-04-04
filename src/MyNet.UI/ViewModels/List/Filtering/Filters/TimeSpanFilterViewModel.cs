// -----------------------------------------------------------------------
// <copyright file="TimeSpanFilterViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Utilities;
using MyNet.Utilities.Comparison;
using MyNet.Utilities.Units;

namespace MyNet.UI.ViewModels.List.Filtering.Filters;

public class TimeSpanFilterViewModel : IntegerFilterViewModel
{
    public TimeSpanFilterViewModel(string propertyName)
        : base(propertyName) { }

    public TimeSpanFilterViewModel(string propertyName, ComplexComparableOperator comparison, int? from, int? to, TimeUnit unit)
        : base(propertyName, comparison, from, to)
    {
        Unit = unit;
        Minimum = 1;
        Maximum = int.MaxValue;
    }

    public TimeUnit Unit { get; set; }

    protected override FilterViewModel CreateCloneInstance() => new TimeSpanFilterViewModel(PropertyName, Operator, From, To, Unit) { Minimum = Minimum, Maximum = Maximum };

    protected override bool IsMatchProperty(object? toCompare) => toCompare is TimeSpan toComparable && toComparable.Compare(From?.ToTimeSpan(Unit), To?.ToTimeSpan(Unit), Operator);
}
