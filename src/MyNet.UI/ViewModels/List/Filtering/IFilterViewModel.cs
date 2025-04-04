// -----------------------------------------------------------------------
// <copyright file="IFilterViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using MyNet.Observable.Collections.Filters;
using MyNet.Utilities;

namespace MyNet.UI.ViewModels.List.Filtering;

public interface IFilterViewModel : IFilter, INotifyPropertyChanged, ICloneable, ISettable, ISimilar<IFilterViewModel>
{
    void Reset();

    bool IsEmpty();
}
