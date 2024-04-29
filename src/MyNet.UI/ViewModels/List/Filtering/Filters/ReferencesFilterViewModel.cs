// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using MyNet.Utilities.Comparaison;

namespace MyNet.UI.ViewModels.List.Filtering.Filters
{
    public class ReferencesFilterViewModel<T> : SelectedValuesFilterViewModel<T>
    {
        public ReferencesFilterViewModel(string propertyName, IEnumerable<T> allowedValues, string selectedValuePath = "") : base(propertyName, allowedValues, selectedValuePath)
        {
        }

        protected override bool IsMatchProperty(object toCompare)
        {
            var result = Values is not null && Values.Cast<object>().Any(x => ReferenceEquals(x, toCompare));

            return Operator switch
            {
                BinaryOperator.Is => result,

                BinaryOperator.IsNot => !result,

                _ => throw new NotImplementedException(),
            };
        }
    }
}
