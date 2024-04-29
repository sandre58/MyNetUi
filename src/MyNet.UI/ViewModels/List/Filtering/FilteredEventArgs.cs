// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MyNet.Observable.Collections.Filters;

namespace MyNet.UI.ViewModels.List.Filtering
{
    public class FilteredEventArgs(IEnumerable<CompositeFilter> filters) : EventArgs
    {
        public IEnumerable<CompositeFilter> Filters { get; } = filters;
    }
}
