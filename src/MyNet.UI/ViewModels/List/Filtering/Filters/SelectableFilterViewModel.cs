// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using MyNet.Utilities.Comparaison;

namespace MyNet.UI.ViewModels.List.Filtering.Filters
{
    public abstract class SelectableFilterViewModel<TAllowedValues> : FilterViewModel
    {
        protected SelectableFilterViewModel(string propertyName, IEnumerable<TAllowedValues>? allowedValues = null) : base(propertyName)
        {
            Operator = BinaryOperator.Is;
            AvailableValues = allowedValues;
        }

        public BinaryOperator Operator { get; set; }

        public IEnumerable<TAllowedValues>? AvailableValues { get; set; }

        public override void SetFrom(object? from)
        {
            if (from is SelectableFilterViewModel<TAllowedValues> other)
                Operator = other.Operator;
        }
    }
}
