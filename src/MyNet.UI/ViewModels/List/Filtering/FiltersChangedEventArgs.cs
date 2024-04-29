// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace MyNet.UI.ViewModels.List.Filtering
{
    public class FiltersChangedEventArgs(IEnumerable<ICompositeFilterViewModel> filters) : EventArgs
    {
        public IEnumerable<ICompositeFilterViewModel> Filters { get; } = filters;
    }
}
