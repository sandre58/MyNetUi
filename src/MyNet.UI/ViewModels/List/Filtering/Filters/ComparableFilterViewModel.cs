// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using MyNet.Utilities;
using MyNet.Utilities.Comparaison;

namespace MyNet.UI.ViewModels.List.Filtering.Filters
{
    public class ComparableFilterViewModel<T> : FilterViewModel where T : struct, IComparable<T>
    {
        public ComparableFilterViewModel(string propertyName) : base(propertyName) { }

        public ComparableFilterViewModel(string propertyName, ComplexComparableOperator comparableOperator, T? from, T? to) : this(propertyName)
        {
            To = to;
            From = from;
            Operator = comparableOperator;
        }

        public ComplexComparableOperator Operator { get; set; }

        public T? From { get; set; }

        public T? To { get; set; }

        public T? Minimum { get; set; }

        public T? Maximum { get; set; }

        protected override bool IsMatchProperty(object toCompare) => toCompare is IComparable<T> toComparable && toComparable.Compare(From, To, Operator);

        public override bool IsEmpty() => (From is null || From.Equals(default(T))) && (To is null || To.Equals(default(T)));

        public override void Reset()
        {
            From = default;
            To = default;
        }

        protected void OnToChanged()
        {
            if (To is not null && To.CompareTo(From) < 0)
                From = To;
        }

        protected void OnFromChanged()
        {
            if (From is not null && From.CompareTo(To) > 0)
                To = From;
        }

        public override void SetFrom(object? from)
        {
            if (from is ComparableFilterViewModel<T> other)
            {
                Operator = other.Operator;
                From = other.From;
                To = other.To;
                Minimum = other.Minimum;
                Maximum = other.Maximum;
            }
        }

        protected override FilterViewModel CreateCloneInstance() => new ComparableFilterViewModel<T>(PropertyName, Operator, From, To);
    }
}
