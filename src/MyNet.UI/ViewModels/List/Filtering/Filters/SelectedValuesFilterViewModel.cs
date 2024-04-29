﻿// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyNet.Utilities.Comparaison;

namespace MyNet.UI.ViewModels.List.Filtering.Filters
{
    public class SelectedValuesFilterViewModel : SelectedValuesFilterViewModel<object>
    {
        public SelectedValuesFilterViewModel(string propertyName, IEnumerable<object> allowedValues, string selectedValuePath = "") : base(propertyName, allowedValues, selectedValuePath) { }

        protected override FilterViewModel CreateCloneInstance() => new SelectedValuesFilterViewModel(PropertyName, AvailableValues ?? [], SelectedValuePath);
    }

    public class SelectedValuesFilterViewModel<TAllowedValues> : SelectableFilterViewModel<TAllowedValues>
    {
        public SelectedValuesFilterViewModel(string propertyName, IEnumerable<TAllowedValues> allowedValues, string selectedValuePath = "") : base(propertyName, allowedValues) => SelectedValuePath = selectedValuePath;

        public IEnumerable? Values { get; set; }

        public string SelectedValuePath { get; set; }

        protected override bool IsMatchProperty(object toCompare)
        {
            var result = Values is not null && Values.Cast<object>().Any(x => x is not null && x.Equals(toCompare) || x is null && toCompare is null);

            return Operator switch
            {
                BinaryOperator.Is => result,

                BinaryOperator.IsNot => !result,

                _ => throw new NotImplementedException(),
            };
        }

        public override bool IsEmpty() => Values is null || !Values.Cast<object>().Any();

        public override void Reset() => Values = default;

        public override void SetFrom(object? from)
        {
            base.SetFrom(from);

            if (from is SelectedValuesFilterViewModel<TAllowedValues> other)
            {
                Values = other.Values;
            }
        }

        protected override FilterViewModel CreateCloneInstance() => new SelectedValuesFilterViewModel<TAllowedValues>(PropertyName, AvailableValues ?? [], SelectedValuePath);
    }
}
