// -----------------------------------------------------------------------
// <copyright file="SortedEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MyNet.Observable.Collections.Sorting;

namespace MyNet.UI.ViewModels.List.Sorting;

public class SortedEventArgs(IEnumerable<SortingProperty> properties) : EventArgs
{
    public IEnumerable<SortingProperty> SortProperties { get; } = properties;
}
