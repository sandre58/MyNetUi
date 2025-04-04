// -----------------------------------------------------------------------
// <copyright file="IDisplayViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MyNet.UI.ViewModels.Display;

public interface IDisplayViewModel : INotifyPropertyChanged
{
    ObservableCollection<IDisplayMode> AllowedModes { get; }

    IDisplayMode? Mode { get; }

    void SetMode<T>()
        where T : IDisplayMode;

    void SetMode(Type type);

    void SetMode(string key);
}
