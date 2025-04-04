// -----------------------------------------------------------------------
// <copyright file="ICompositeFilterViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using MyNet.Utilities;
using MyNet.Utilities.Comparison;

namespace MyNet.UI.ViewModels.List.Filtering;

public interface ICompositeFilterViewModel : IWrapper<IFilterViewModel>, INotifyPropertyChanged, ICloneable
{
    bool IsEnabled { get; set; }

    LogicalOperator Operator { get; set; }

    void Reset();
}
