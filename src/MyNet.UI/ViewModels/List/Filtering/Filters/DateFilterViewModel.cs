// -----------------------------------------------------------------------
// <copyright file="DateFilterViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Utilities.Comparison;

namespace MyNet.UI.ViewModels.List.Filtering.Filters;

public class DateFilterViewModel : ComparableFilterViewModel<DateTime>
{
    public DateFilterViewModel(string propertyName)
        : base(propertyName) { }

    public DateFilterViewModel(string propertyName, ComplexComparableOperator comparison, DateTime? from, DateTime? to)
        : base(propertyName, comparison, from, to) { }

    protected override FilterViewModel CreateCloneInstance() => new DateFilterViewModel(PropertyName, Operator, From, To) { Minimum = Minimum, Maximum = Maximum };
}
