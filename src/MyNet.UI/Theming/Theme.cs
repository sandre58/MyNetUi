// -----------------------------------------------------------------------
// <copyright file="Theme.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.UI.Theming;

public class Theme
{
    public ThemeBase? Base { get; set; }

    public string? PrimaryColor { get; set; }

    public string? PrimaryForegroundColor { get; set; }

    public string? AccentColor { get; set; }

    public string? AccentForegroundColor { get; set; }
}
