// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MyNet.Observable.Collections.Sorting;

namespace MyNet.UI.ViewModels.List.Sorting
{
    public class SortedEventArgs(IEnumerable<SortingProperty> properties) : EventArgs
    {
        public IEnumerable<SortingProperty> SortProperties { get; } = properties;
    }
}
