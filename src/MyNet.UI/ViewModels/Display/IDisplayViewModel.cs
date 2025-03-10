﻿// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MyNet.UI.ViewModels.Display
{
    public interface IDisplayViewModel : INotifyPropertyChanged
    {
        ObservableCollection<IDisplayMode> AllowedModes { get; }

        IDisplayMode? Mode { get; }

        void SetMode<T>() where T : IDisplayMode;

        void SetMode(Type type);

        void SetMode(string key);
    }
}
