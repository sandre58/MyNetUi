﻿// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.UI.Toasting.Settings
{
    /// <summary>
    /// Settings for WinMessageBox.
    /// </summary>
    public class ToastSettings
    {
        public static ToastSettings Default => new();

        public ToastClosingStrategy ClosingStrategy { get; set; } = ToastClosingStrategy.AutoClose;

        public bool FreezeOnMouseEnter { get; set; } = true;
    }
}
