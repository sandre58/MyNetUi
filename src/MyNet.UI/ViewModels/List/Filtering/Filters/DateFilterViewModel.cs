// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.Utilities.Comparaison;
using System;

namespace MyNet.UI.ViewModels.List.Filtering.Filters
{
    public class DateFilterViewModel : ComparableFilterViewModel<DateTime>
    {
        public DateFilterViewModel(string propertyName) : base(propertyName) { }

        public DateFilterViewModel(string propertyName, ComplexComparableOperator comparaison, DateTime? from, DateTime? to) : base(propertyName, comparaison, from, to) { }

        protected override FilterViewModel CreateCloneInstance() => new DateFilterViewModel(PropertyName, Operator, From, To) { Minimum = Minimum, Maximum = Maximum };
    }
}
