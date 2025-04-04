// -----------------------------------------------------------------------
// <copyright file="NavigatingContext.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.UI.Navigation.Models;

public class NavigatingContext(INavigationPage? oldPage, INavigationPage newPage, NavigationMode mode, NavigationParameters? navigationParameters = null) : NavigationContext(oldPage, newPage, mode, navigationParameters)
{
    public bool Cancel { get; set; }
}
