// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using MyNet.UI.Resources;
using MyNet.UI.ViewModels.Workspace;
using MyNet.Utilities;
using MyNet.Utilities.Localization;

namespace MyNet.UI.ViewModels.Shell
{
    public class TimeAndLanguageViewModel : NavigableWorkspaceViewModel
    {
        public CultureInfo? SelectedCulture { get; set; }

        public TimeZoneInfo? SelectedTimeZone { get; set; }

        public ObservableCollection<CultureInfo?> Cultures { get; private set; } = [];

        public ObservableCollection<TimeZoneInfo?> TimeZones { get; private set; } = [];

        public TimeAndLanguageViewModel()
        {
            Cultures.AddRange(GlobalizationService.Current.SupportedCultures);
            TimeZones.AddRange(GlobalizationService.Current.SupportedTimeZones);

            using (IsModifiedSuspender.Suspend())
            {
                UpdateSelectedCulture();
                UpdateSelectedTimeZone();
            }
        }

        protected override string CreateTitle() => UiResources.TimeAndLanguage;

        #region Culture

        private CultureInfo? GetSelectedCulture(CultureInfo culture) => Cultures.Contains(culture) ? culture : culture.Parent is not null ? GetSelectedCulture(culture.Parent) : null;

        private void UpdateSelectedCulture() => SelectedCulture = GetSelectedCulture(GlobalizationService.Current.Culture);

        protected virtual void OnSelectedCultureChanged() => GlobalizationService.Current.SetCulture(SelectedCulture?.ToString() ?? CultureInfo.InstalledUICulture.ToString());

        protected override void OnCultureChanged()
        {
            base.OnCultureChanged();
            UpdateSelectedCulture();
        }

        #endregion

        #region TimeZone

        private void UpdateSelectedTimeZone() => SelectedTimeZone = GlobalizationService.Current.TimeZone;

        protected virtual void OnSelectedTimeZoneChanged() => GlobalizationService.Current.SetTimeZone(SelectedTimeZone ?? TimeZoneInfo.Local);

        protected override void OnTimeZoneChanged()
        {
            base.OnTimeZoneChanged();
            UpdateSelectedTimeZone();
        }

        #endregion
    }
}
