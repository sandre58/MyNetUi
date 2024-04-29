// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;

namespace MyNet.UI.ViewModels.List.Sorting
{
    public interface ISortingPropertyViewModel : INotifyPropertyChanged, ICloneable
    {
        string PropertyName { get; }

        ListSortDirection Direction { get; set; }

        bool IsEnabled { get; set; }

        int Order { get; set; }
    }
}
