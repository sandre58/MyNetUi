// -----------------------------------------------------------------------
// <copyright file="ThemeManager.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.UI.Theming;

public static class ThemeManager
{
    private static IThemeService? _themeService;

    public static Theme? CurrentTheme => _themeService?.CurrentTheme;

    public static void Initialize(IThemeService themeService) => _themeService = themeService;

    public static event EventHandler<ThemeChangedEventArgs> ThemeChanged
    {
        add
        {
            if (_themeService is not null)
                _themeService.ThemeChanged += value;
        }

        remove
        {
            if (_themeService is not null)
                _themeService.ThemeChanged -= value;
        }
    }

    public static void ApplyBase(ThemeBase themeBase) => ApplyTheme(new Theme { Base = themeBase });

    public static void ApplyPrimaryColor(string? color) => ApplyPrimaryColor(color, null);

    public static void ApplyPrimaryColor(string? color, string? foreground) => ApplyTheme(new Theme { PrimaryColor = color, PrimaryForegroundColor = foreground });

    public static void ApplyAccentColor(string? color) => ApplyAccentColor(color, null);

    public static void ApplyAccentColor(string? color, string? foreground) => ApplyTheme(new Theme { AccentColor = color, AccentForegroundColor = foreground });

    public static void ApplyTheme(Theme theme) => _themeService?.ApplyTheme(theme);
}
