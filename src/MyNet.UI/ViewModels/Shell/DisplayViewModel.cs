// -----------------------------------------------------------------------
// <copyright file="DisplayViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.UI.Resources;
using MyNet.UI.Theming;
using MyNet.UI.ViewModels.Workspace;
using MyNet.Utilities;

namespace MyNet.UI.ViewModels.Shell;

public class DisplayViewModel : NavigableWorkspaceViewModel
{
    protected override string CreateTitle() => UiResources.Display;

    public ThemeBase ThemeBase { get; set; }

    public string? PrimaryColor { get; set; }

    public string? AccentColor { get; set; }

    public string? PrimaryForegroundColor { get; set; }

    public string? AccentForegroundColor { get; set; }

    public bool AutoPrimaryForegroundColor { get; set; }

    public bool AutoAccentForegroundColor { get; set; }

    public float ContrastRatio { get; set; }

    public bool EnableContrast { get; set; }

    public DisplayViewModel()
    {
        if (ThemeManager.CurrentTheme is not null)
            UpdateFromTheme(ThemeManager.CurrentTheme);

        ThemeManager.ThemeChanged += ThemeService_ThemeChanged;
    }

    private void UpdateFromTheme(Theme theme)
    {
        if (ThemeBase != theme.Base) ThemeBase = theme.Base ?? ThemeBase.Inherit;
        if (PrimaryColor != theme.PrimaryColor) PrimaryColor = theme.PrimaryColor;
        if (AccentColor != theme.AccentColor) AccentColor = theme.AccentColor;
        if (PrimaryForegroundColor != theme.PrimaryForegroundColor) PrimaryForegroundColor = theme.PrimaryForegroundColor;
        if (AccentForegroundColor != theme.AccentForegroundColor) AccentForegroundColor = theme.AccentForegroundColor;
    }

    private void ThemeService_ThemeChanged(object? sender, ThemeChangedEventArgs e) => UpdateFromTheme(e.CurrentTheme);

    protected void OnThemeBaseChanged() => (ThemeBase != ThemeManager.CurrentTheme?.Base).IfTrue(() => ThemeManager.ApplyBase(ThemeBase));

    protected void OnPrimaryColorChanged()
        => (!string.IsNullOrEmpty(PrimaryColor) && PrimaryColor != ThemeManager.CurrentTheme?.PrimaryColor).IfTrue(() => ThemeManager.ApplyPrimaryColor(PrimaryColor, AutoPrimaryForegroundColor ? null : PrimaryForegroundColor));

    protected void OnAccentColorChanged()
        => (!string.IsNullOrEmpty(PrimaryColor) && AccentColor != ThemeManager.CurrentTheme?.AccentColor).IfTrue(() => ThemeManager.ApplyAccentColor(AccentColor, AutoAccentForegroundColor ? null : AccentForegroundColor));

    protected void OnPrimaryForegroundColorChanged()
        => (!string.IsNullOrEmpty(PrimaryForegroundColor) && PrimaryForegroundColor != ThemeManager.CurrentTheme?.PrimaryForegroundColor).IfTrue(() => ThemeManager.ApplyPrimaryColor(PrimaryColor, AutoPrimaryForegroundColor ? null : PrimaryForegroundColor));

    protected void OnAccentForegroundColorChanged()
        => (!string.IsNullOrEmpty(AccentForegroundColor) && AccentForegroundColor != ThemeManager.CurrentTheme?.AccentForegroundColor).IfTrue(() => ThemeManager.ApplyAccentColor(AccentColor, AutoAccentForegroundColor ? null : AccentForegroundColor));

    protected void OnAutoPrimaryForegroundColorChanged() => ThemeManager.ApplyPrimaryColor(PrimaryColor, AutoPrimaryForegroundColor ? null : PrimaryForegroundColor);

    protected void OnAutoAccentForegroundColorChanged() => ThemeManager.ApplyAccentColor(AccentColor, AutoAccentForegroundColor ? null : AccentForegroundColor);

    protected override void Cleanup()
    {
        base.Cleanup();

        ThemeManager.ThemeChanged -= ThemeService_ThemeChanged;
    }
}
