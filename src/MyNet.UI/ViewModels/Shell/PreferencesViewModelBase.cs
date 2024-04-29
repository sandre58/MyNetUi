﻿// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using MyNet.UI.Resources;
using MyNet.UI.Services;
using MyNet.UI.ViewModels.Edition;

namespace MyNet.UI.ViewModels.Shell
{
    public class PreferencesViewModelBase : EditionViewModel
    {
        private readonly IPersistentPreferencesService _preferencesService;

        public PreferencesViewModelBase(IPersistentPreferencesService preferencesService, IEnumerable<INavigableWorkspaceViewModel> subWorkspaces)
        {
            _preferencesService = preferencesService;

            AddSubWorkspaces(subWorkspaces);
        }

        protected override string CreateTitle() => UiResources.Preferences;

        protected override void ResetCore() => _preferencesService.Reset();

        protected override void RefreshCore() => _preferencesService.Reload();

        protected override void SaveCore() => _preferencesService.Save();
    }
}
