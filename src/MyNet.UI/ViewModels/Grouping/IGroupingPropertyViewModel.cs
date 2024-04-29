// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;

namespace MyNet.UI.ViewModels.Grouping
{
    public interface IGroupingPropertyViewModel : INotifyPropertyChanged, ICloneable
    {
        string PropertyName { get; }

        string SortingPropertyName { get; }

        bool IsEnabled { get; set; }

        int Order { get; set; }
    }
}
