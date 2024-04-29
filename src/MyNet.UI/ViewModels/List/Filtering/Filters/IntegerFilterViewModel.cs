// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.Utilities.Comparaison;
using MyNet.Utilities.Sequences;

namespace MyNet.UI.ViewModels.List.Filtering.Filters
{
    /// <summary>
    ///
    /// </summary>
    public class IntegerFilterViewModel : ComparableFilterViewModel<int>
    {
        public IntegerFilterViewModel(string propertyName) : base(propertyName) { }

        public IntegerFilterViewModel(string propertyName, ComplexComparableOperator comparaison, int? from, int? to) : base(propertyName, comparaison, from, to) { }

        public IntegerFilterViewModel(string propertyName, ComplexComparableOperator comparaison, AcceptableValueRange<int> range) : base(propertyName, comparaison, range.Min, range.Max) => (Minimum, Maximum) = (range.Min, range.Max);

        protected override FilterViewModel CreateCloneInstance() => new IntegerFilterViewModel(PropertyName, Operator, From, To) { Maximum = Maximum, Minimum = Minimum };

    }
}
