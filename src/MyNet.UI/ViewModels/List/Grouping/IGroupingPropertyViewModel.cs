// -----------------------------------------------------------------------
// <copyright file="IGroupingPropertyViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

namespace MyNet.UI.ViewModels.List.Grouping;

public interface IGroupingPropertyViewModel : INotifyPropertyChanged, ICloneable
{
    string PropertyName { get; }

    string SortingPropertyName { get; }

    bool IsEnabled { get; set; }

    int Order { get; set; }
}
