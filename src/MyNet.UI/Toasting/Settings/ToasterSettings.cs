// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.UI.Toasting.Settings
{
    /// <summary>
    /// Settings for WinMessageBox.
    /// </summary>
    public class ToasterSettings
    {

        public static ToasterSettings Default => new();

        /// <summary>
        /// Gets or sets duration.
        /// </summary>
        public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(3.5);

        /// <summary>
        /// Gets or sets max items.
        /// </summary>
        public int MaxItems { get; set; } = int.MaxValue;

        /// <summary>
        /// Gets or sets offset.
        /// </summary>
        public ToasterPosition Position { get; set; } = ToasterPosition.BottomRight;

        /// <summary>
        /// Gets or sets offset.
        /// </summary>
        public double OffsetX { get; set; } = 10;

        /// <summary>
        /// Gets or sets offset.
        /// </summary>
        public double OffsetY { get; set; } = 10;

        public double Width { get; set; } = 300;
    }
}
