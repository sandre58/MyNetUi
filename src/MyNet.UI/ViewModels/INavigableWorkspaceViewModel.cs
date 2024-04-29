// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.UI.Navigation.Models;

namespace MyNet.UI.ViewModels
{

    public interface INavigableWorkspaceViewModel : IWorkspaceViewModel, INavigationPage
    {
        public IWorkspaceViewModel? ParentPage { get; }

        void SetParentPage(IWorkspaceViewModel parentPage);
    }
}
