// -----------------------------------------------------------------------
// <copyright file="IntegerFilterViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Utilities.Comparison;
using MyNet.Utilities.Sequences;

namespace MyNet.UI.ViewModels.List.Filtering.Filters;

public class IntegerFilterViewModel : ComparableFilterViewModel<int>
{
    public IntegerFilterViewModel(string propertyName)
        : base(propertyName) { }

    public IntegerFilterViewModel(string propertyName, ComplexComparableOperator comparison, int? from, int? to)
        : base(propertyName, comparison, from, to) { }

    public IntegerFilterViewModel(string propertyName, ComplexComparableOperator comparison, AcceptableValueRange<int> range)
        : base(propertyName, comparison, range.Min, range.Max) => (Minimum, Maximum) = (range.Min, range.Max);

    protected override FilterViewModel CreateCloneInstance() => new IntegerFilterViewModel(PropertyName, Operator, From, To) { Maximum = Maximum, Minimum = Minimum };
}
