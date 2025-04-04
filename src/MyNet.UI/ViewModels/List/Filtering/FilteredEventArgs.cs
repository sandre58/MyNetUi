// -----------------------------------------------------------------------
// <copyright file="FilteredEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MyNet.Observable.Collections.Filters;

namespace MyNet.UI.ViewModels.List.Filtering;

public class FilteredEventArgs(IEnumerable<CompositeFilter> filters) : EventArgs
{
    public IEnumerable<CompositeFilter> Filters { get; } = filters;
}
