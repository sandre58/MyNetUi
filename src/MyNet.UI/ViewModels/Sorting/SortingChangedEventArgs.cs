// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace MyNet.UI.ViewModels.Sorting
{
    public class SortingChangedEventArgs(IEnumerable<ISortingPropertyViewModel> properties) : EventArgs
    {
        public IEnumerable<ISortingPropertyViewModel> SortingProperties { get; } = properties;
    }
}
