// -----------------------------------------------------------------------
// <copyright file="ToastSettings.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.UI.Toasting.Settings;

/// <summary>
/// Settings for WinMessageBox.
/// </summary>
public class ToastSettings
{
    public static ToastSettings Default => new();

    public ToastClosingStrategy ClosingStrategy { get; set; } = ToastClosingStrategy.AutoClose;

    public bool FreezeOnMouseEnter { get; set; } = true;
}
