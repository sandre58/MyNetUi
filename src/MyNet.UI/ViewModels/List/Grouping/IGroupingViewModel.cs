// -----------------------------------------------------------------------
// <copyright file="IGroupingViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

namespace MyNet.UI.ViewModels.List.Grouping;

public interface IGroupingViewModel : INotifyPropertyChanged
{
    void Reset();

    event EventHandler<GroupingChangedEventArgs>? GroupingChanged;
}
