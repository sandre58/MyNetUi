// -----------------------------------------------------------------------
// <copyright file="FileNameFilterViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.IO;
using MyNet.Utilities.Comparison;

namespace MyNet.UI.ViewModels.List.Filtering.Filters;

public class FileNameFilterViewModel(string propertyName, StringOperator filterMode = StringOperator.Contains, bool caseSensitive = false) : StringFilterViewModel(propertyName, filterMode, caseSensitive)
{
    protected override bool IsMatchProperty(object? toCompare) => base.IsMatchProperty(Path.GetFileName(toCompare?.ToString()) ?? string.Empty);

    protected override FilterViewModel CreateCloneInstance() => new FileNameFilterViewModel(PropertyName, Operator, CaseSensitive);
}
