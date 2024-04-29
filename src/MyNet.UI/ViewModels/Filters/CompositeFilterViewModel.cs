// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using MyNet.Utilities;
using MyNet.Utilities.Comparaison;

namespace MyNet.UI.ViewModels.Filters
{
    public interface ICompositeFilterViewModel : IWrapper<IFilterViewModel>, INotifyPropertyChanged, ICloneable
    {
        bool IsEnabled { get; set; }

        LogicalOperator Operator { get; set; }

        void Reset();
    }
}
