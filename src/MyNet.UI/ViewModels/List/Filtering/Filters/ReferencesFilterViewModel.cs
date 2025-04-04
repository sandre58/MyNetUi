// -----------------------------------------------------------------------
// <copyright file="ReferencesFilterViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using MyNet.Utilities.Comparison;

namespace MyNet.UI.ViewModels.List.Filtering.Filters;

public class ReferencesFilterViewModel<T>(string propertyName, IEnumerable<T> allowedValues, string selectedValuePath = "") : SelectedValuesFilterViewModel<T>(propertyName, allowedValues, selectedValuePath)
{
    protected override bool IsMatchProperty(object? toCompare)
    {
        var result = Values is not null && Values.Cast<object>().Any(x => ReferenceEquals(x, toCompare));

        return Operator switch
        {
            BinaryOperator.Is => result,

            BinaryOperator.IsNot => !result,

            _ => throw new NotImplementedException()
        };
    }
}
