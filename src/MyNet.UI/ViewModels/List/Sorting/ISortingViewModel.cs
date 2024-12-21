// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;

namespace MyNet.UI.ViewModels.List.Sorting
{
    public interface ISortingViewModel
    {
        ICommand ApplyCommand { get; }

        void Reset();

        event EventHandler<SortingChangedEventArgs>? SortingChanged;
    }
}
