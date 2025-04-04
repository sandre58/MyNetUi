// -----------------------------------------------------------------------
// <copyright file="NavigationContext.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.UI.Navigation.Models;

/// <summary>
/// Encapsulates information about a navigation request.
/// </summary>
public class NavigationContext(INavigationPage? oldPage, INavigationPage newPage, NavigationMode mode, NavigationParameters? navigationParameters = null)
{
    public INavigationPage? OldPage { get; } = oldPage;

    public INavigationPage Page { get; } = newPage;

    public NavigationParameters? Parameters { get; } = navigationParameters;

    public NavigationMode Mode { get; } = mode;
}
