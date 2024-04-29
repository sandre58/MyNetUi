// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace MyNet.UI.ViewModels.Grouping
{
    public class GroupingChangedEventArgs(IEnumerable<IGroupingPropertyViewModel> groupProperties) : EventArgs
    {
        public IEnumerable<IGroupingPropertyViewModel> GroupProperties { get; } = groupProperties;
    }
}
