// -----------------------------------------------------------------------
// <copyright file="NavigationEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.UI.Navigation.Models;

namespace MyNet.UI.Navigation;

public class NavigationEventArgs(INavigationPage? oldPage, INavigationPage newPage, NavigationMode mode, NavigationParameters? navigationParameters = null) : EventArgs
{
    public INavigationPage? OldPage { get; } = oldPage;

    public INavigationPage NewPage { get; } = newPage;

    public NavigationParameters? Parameters { get; } = navigationParameters;

    public NavigationMode Mode { get; } = mode;
}
