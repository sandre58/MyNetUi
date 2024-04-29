// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using MyNet.Utilities.Comparaison;

namespace MyNet.UI.ViewModels.List.Filtering.Filters
{
    public class StringFilterViewModel : FilterViewModel
    {
        public StringFilterViewModel(string propertyName, StringOperator filterMode = StringOperator.Contains, bool caseSensitive = false) : base(propertyName)
        {
            Operator = filterMode;
            CaseSensitive = caseSensitive;
        }

        public StringOperator Operator { get; set; }

        public string? Value { get; set; }

        public bool CaseSensitive { get; set; }

        protected override bool IsMatchProperty(object toCompare)
        {
            if (toCompare is null)
                return Operator == StringOperator.Is && Value is null;

            var toStringCompare = toCompare.ToString() ?? string.Empty;

            if (Value is null)
                return false;

            var value = Value;
            if (!CaseSensitive)
            {
                value = Value.ToUpperInvariant();
                toStringCompare = toStringCompare?.ToUpperInvariant() ?? string.Empty;
            }

            return Operator switch
            {
                StringOperator.Contains => toStringCompare.Contains(value),

                StringOperator.StartsWith => toStringCompare.StartsWith(value, StringComparison.InvariantCultureIgnoreCase),

                StringOperator.EndsWith => toStringCompare.EndsWith(value, StringComparison.InvariantCultureIgnoreCase),

                StringOperator.Is => toStringCompare.Equals(value, StringComparison.InvariantCultureIgnoreCase),

                StringOperator.IsNot => !toStringCompare.Equals(value, StringComparison.InvariantCultureIgnoreCase),

                _ => throw new NotImplementedException(),
            };
        }

        public override bool IsEmpty() => string.IsNullOrEmpty(Value);

        public override void Reset() => Value = string.Empty;

        public override bool Equals(object? obj) => obj is StringFilterViewModel o && base.Equals(obj) && Operator == o.Operator && CaseSensitive == o.CaseSensitive;

        public override int GetHashCode() => base.GetHashCode();

        public override void SetFrom(object? from)
        {
            if (from is StringFilterViewModel other)
            {
                Operator = other.Operator;
                CaseSensitive = other.CaseSensitive;
                Value = other.Value;
            }
        }

        protected override FilterViewModel CreateCloneInstance() => new StringFilterViewModel(PropertyName, Operator, CaseSensitive);
    }
}
