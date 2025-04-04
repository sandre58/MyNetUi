// -----------------------------------------------------------------------
// <copyright file="ISortingViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Windows.Input;

namespace MyNet.UI.ViewModels.List.Sorting;

public interface ISortingViewModel
{
    ICommand ApplyCommand { get; }

    void Reset();

    event EventHandler<SortingChangedEventArgs>? SortingChanged;
}
