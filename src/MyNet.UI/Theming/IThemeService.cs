// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.UI.Theming
{
    public interface IThemeService
    {
        Theme CurrentTheme { get; }

        void ApplyTheme(Theme theme);

        IThemeService AddBaseExtension(IThemeExtension extension);

        IThemeService AddPrimaryExtension(IThemeExtension extension);

        IThemeService AddAccentExtension(IThemeExtension extension);

        event EventHandler<ThemeChangedEventArgs>? ThemeChanged;
    }
}
