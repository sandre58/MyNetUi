// -----------------------------------------------------------------------
// <copyright file="DisplayViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MyNet.Observable;
using MyNet.UI.Commands;

namespace MyNet.UI.ViewModels.Display;

public class DisplayViewModel : ObservableObject, IDisplayViewModel
{
    public ObservableCollection<IDisplayMode> AllowedModes { get; }

    public IDisplayMode? Mode { get; set; }

    public ICommand SetModeCommand { get; }

    public DisplayViewModel()
        : this([]) { }

    public DisplayViewModel(IEnumerable<IDisplayMode> allowedModes, IDisplayMode? defaultMode = null)
    {
        AllowedModes = [.. allowedModes];
        Mode = defaultMode;
        SetModeCommand = CommandsManager.CreateNotNull<IDisplayMode>(x => Mode = x);
    }

    public void SetMode<T>()
        where T : IDisplayMode
        => Mode = AllowedModes.OfType<T>().FirstOrDefault();

    public void SetMode(Type type) => Mode = AllowedModes.FirstOrDefault(x => x.GetType() == type);

    public void SetMode(string key) => Mode = AllowedModes.FirstOrDefault(x => x.Key == key);

    public DisplayViewModel AddMode(IDisplayMode mode, bool isDefault = false)
    {
        AllowedModes.Add(mode);

        if (isDefault)
            Mode = mode;

        return this;
    }

    public DisplayViewModel AddMode<T>(bool isDefault = false, Action<T>? action = null)
        where T : IDisplayMode, new()
    {
        var mode = new T();
        action?.Invoke(mode);

        return AddMode(mode, isDefault);
    }
}
