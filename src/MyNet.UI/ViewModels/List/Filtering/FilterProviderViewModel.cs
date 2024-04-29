// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using MyNet.Observable.Translatables;
using MyNet.UI.ViewModels.Filters;

namespace MyNet.UI.ViewModels.List.Filtering
{
    public class FilterProviderViewModel : DisplayWrapper<Func<IFilterViewModel>>
    {
        public FilterProviderViewModel(string resourceKey, Func<IFilterViewModel> item) : base(item, resourceKey) { }
    }
}
