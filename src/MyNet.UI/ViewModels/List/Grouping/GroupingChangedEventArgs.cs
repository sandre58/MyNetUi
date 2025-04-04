// -----------------------------------------------------------------------
// <copyright file="GroupingChangedEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace MyNet.UI.ViewModels.List.Grouping;

public class GroupingChangedEventArgs(IEnumerable<IGroupingPropertyViewModel> groupProperties) : EventArgs
{
    public IEnumerable<IGroupingPropertyViewModel> GroupProperties { get; } = groupProperties;
}
