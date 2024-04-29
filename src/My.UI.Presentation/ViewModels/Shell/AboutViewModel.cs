// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.UI.Presentation.Helpers;
using MyNet.UI.ViewModels.Workspace;
using My.Utilities;
using My.Utilities.Resources;

namespace MyNet.UI.Presentation.ViewModels.Shell
{
    public class AboutViewModel : WorkspaceViewModel
    {
        public string? Version { get; private set; } = ApplicationHelper.GetVersion();

        public string? Message { get; private set; }

        public string? Copyright { get; private set; } = ApplicationHelper.GetCopyright();

        public string? Product { get; private set; } = ApplicationHelper.GetProductName();

        public string? Company { get; private set; } = ApplicationHelper.GetCompany();

        public string? Description { get; private set; } = ApplicationHelper.GetDescription();

        public AboutViewModel() => UpdateTitle();

        protected override string CreateTitle() => MyResources.AboutX.FormatWith(Product ?? string.Empty);
    }
}
