// -----------------------------------------------------------------------
// <copyright file="ThemeChangedEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.UI.Theming;

public class ThemeChangedEventArgs(Theme theme) : EventArgs
{
    public Theme CurrentTheme { get; } = theme;
}
