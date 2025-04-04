// -----------------------------------------------------------------------
// <copyright file="NavigatingEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.UI.Navigation.Models;

namespace MyNet.UI.Navigation;

public class NavigatingEventArgs(INavigationPage? oldPage, INavigationPage newPage, NavigationMode mode, NavigationParameters? navigationParameters = null) : NavigationEventArgs(oldPage, newPage, mode, navigationParameters)
{
    public bool Cancel { get; set; }
}
