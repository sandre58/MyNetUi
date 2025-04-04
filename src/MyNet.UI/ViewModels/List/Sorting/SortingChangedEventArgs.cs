// -----------------------------------------------------------------------
// <copyright file="SortingChangedEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace MyNet.UI.ViewModels.List.Sorting;

public class SortingChangedEventArgs(IEnumerable<ISortingPropertyViewModel> properties) : EventArgs
{
    public IEnumerable<ISortingPropertyViewModel> SortingProperties { get; } = properties;
}
