// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.UI.Theming
{
    public class ThemeChangedEventArgs(Theme theme) : EventArgs
    {
        public Theme CurrentTheme { get; } = theme;
    }
}
