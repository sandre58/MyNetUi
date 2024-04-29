// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using MyNet.Observable.Collections.Filters;
using MyNet.Utilities;

namespace MyNet.UI.ViewModels.List.Filtering
{
    public interface IFilterViewModel : IFilter, INotifyPropertyChanged, ICloneable, ISettable, ISimilar<IFilterViewModel>
    {
        void Reset();

        bool IsEmpty();
    }
}
