// -----------------------------------------------------------------------
// <copyright file="ToasterSettings.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.UI.Toasting.Settings;

public class ToasterSettings
{
    public static ToasterSettings Default => new();

    public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(3.5);

    public int MaxItems { get; set; } = int.MaxValue;

    public ToasterPosition Position { get; set; } = ToasterPosition.BottomRight;

    public double OffsetX { get; set; } = 10;

    public double OffsetY { get; set; } = 10;

    public double Width { get; set; } = 300;
}
