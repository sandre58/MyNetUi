// -----------------------------------------------------------------------
// <copyright file="FiltersChangedEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace MyNet.UI.ViewModels.List.Filtering;

public class FiltersChangedEventArgs(IEnumerable<ICompositeFilterViewModel> filters) : EventArgs
{
    public IEnumerable<ICompositeFilterViewModel> Filters { get; } = filters;
}
