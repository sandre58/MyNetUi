// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Globalization;
using MyNet.UI.ViewModels.Workspace;
using My.Utilities;
using My.Utilities.Localization;
using My.Utilities.Resources;

namespace MyNet.UI.Presentation.ViewModels.Shell
{
    public class LanguageViewModel : NavigableWorkspaceViewModel
    {
        public CultureInfo? SelectedCulture { get; set; }

        public ObservableCollection<CultureInfo?> Cultures { get; private set; } = [];

        public LanguageViewModel()
        {
            Cultures.AddRange(CultureInfoService.Current.SupportedCultures);

            using (IsModifiedSuspender.Suspend())
                UpdateSelectedCulture();

            CultureInfoService.Current.CultureChanged += Current_CultureChanged;
        }

        protected override string CreateTitle() => MyResources.Language;

        private CultureInfo? GetSelectedCulture(CultureInfo culture) => Cultures.Contains(culture) ? culture : culture.Parent is not null ? GetSelectedCulture(culture.Parent) : null;

        private void UpdateSelectedCulture() => SelectedCulture = GetSelectedCulture(CultureInfo.CurrentCulture);

        protected virtual void OnSelectedCultureChanged() => CultureInfoService.Current.SetCulture(SelectedCulture?.ToString() ?? CultureInfo.InstalledUICulture.ToString());

        private void Current_CultureChanged(object? sender, System.EventArgs e) => UpdateSelectedCulture();

        protected override void Cleanup()
        {
            base.Cleanup();
            CultureInfoService.Current.CultureChanged -= Current_CultureChanged;
        }
    }
}
