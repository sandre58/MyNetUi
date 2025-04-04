// -----------------------------------------------------------------------
// <copyright file="IThemeService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.UI.Theming;

public interface IThemeService
{
    Theme CurrentTheme { get; }

    void ApplyTheme(Theme theme);

    IThemeService AddBaseExtension(IThemeExtension extension);

    IThemeService AddPrimaryExtension(IThemeExtension extension);

    IThemeService AddAccentExtension(IThemeExtension extension);

    event EventHandler<ThemeChangedEventArgs>? ThemeChanged;
}
