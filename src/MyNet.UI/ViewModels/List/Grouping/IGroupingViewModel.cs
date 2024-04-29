// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;

namespace MyNet.UI.ViewModels.List.Grouping
{
    public interface IGroupingViewModel : INotifyPropertyChanged
    {
        void Reset();

        event EventHandler<GroupingChangedEventArgs>? GroupingChanged;
    }
}
