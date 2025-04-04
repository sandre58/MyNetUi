// -----------------------------------------------------------------------
// <copyright file="ISortingPropertyViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

namespace MyNet.UI.ViewModels.List.Sorting;

public interface ISortingPropertyViewModel : INotifyPropertyChanged, ICloneable
{
    string PropertyName { get; }

    ListSortDirection Direction { get; set; }

    bool IsEnabled { get; set; }

    int Order { get; set; }
}
