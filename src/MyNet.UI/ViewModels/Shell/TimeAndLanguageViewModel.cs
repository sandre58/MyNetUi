﻿// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using DynamicData.Binding;
using MyNet.UI.Resources;
using MyNet.UI.ViewModels.Workspace;
using MyNet.Utilities;
using MyNet.Utilities.Localization;

namespace MyNet.UI.ViewModels.Shell
{
    public class TimeAndLanguageViewModel : NavigableWorkspaceViewModel
    {
        public virtual CultureInfo? SelectedCulture { get; set; }

        public virtual bool AutomaticCulture { get; set; }

        public virtual TimeZoneInfo? SelectedTimeZone { get; set; }

        public virtual bool AutomaticTimeZone { get; set; }

        public ObservableCollection<CultureInfo> Cultures { get; private set; } = [];

        public ObservableCollection<TimeZoneInfo> TimeZones { get; private set; } = [];

        public TimeAndLanguageViewModel()
        {
            Cultures.AddRange(GlobalizationService.Current.SupportedCultures);
            TimeZones.AddRange(GlobalizationService.Current.SupportedTimeZones);

            Disposables.AddRange(
                [
                    this.WhenAnyPropertyChanged(nameof(AutomaticCulture), nameof(SelectedCulture)).Subscribe(_ => ApplyCulture()),
                    this.WhenAnyPropertyChanged(nameof(AutomaticTimeZone), nameof(SelectedTimeZone)).Subscribe(_ => ApplyTimeZone())
                ]);

            using (IsModifiedSuspender.Suspend())
            {
                RefreshCulture();
                RefreshTimeZone();
            }
        }

        protected override string CreateTitle() => UiResources.TimeAndLanguage;

        #region Culture

        private CultureInfo? GetSelectedCulture(CultureInfo culture) => Cultures.Contains(culture) ? culture : culture.Parent is not null ? GetSelectedCulture(culture.Parent) : null;

        private void RefreshCulture() => SelectedCulture = GetSelectedCulture(GlobalizationService.Current.Culture);

        protected override void OnCultureChanged()
        {
            base.OnCultureChanged();

            using (IsModifiedSuspender.Suspend())
                RefreshCulture();
        }

        private void ApplyCulture()
        {
            if (IsModifiedSuspender.IsSuspended) return;

            var culture = SelectedCulture is null || AutomaticCulture ? CultureInfo.InstalledUICulture.ToString() : SelectedCulture.ToString();

            if (culture != GlobalizationService.Current.Culture.Name && culture != GlobalizationService.Current.Culture.Parent?.Name)
                GlobalizationService.Current.SetCulture(culture);
        }

        #endregion

        #region TimeZone

        private void RefreshTimeZone() => SelectedTimeZone = GlobalizationService.Current.TimeZone;

        protected override void OnTimeZoneChanged()
        {
            base.OnTimeZoneChanged();

            using (IsModifiedSuspender.Suspend())
                RefreshTimeZone();
        }

        private void ApplyTimeZone()
        {
            if (IsModifiedSuspender.IsSuspended) return;

            var timeZone = SelectedTimeZone is null || AutomaticTimeZone ? TimeZoneInfo.Local : SelectedTimeZone;

            if (timeZone != GlobalizationService.Current.TimeZone)
                GlobalizationService.Current.SetTimeZone(timeZone);
        }

        #endregion
    }
}
