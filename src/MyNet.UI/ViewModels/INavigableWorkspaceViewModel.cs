// -----------------------------------------------------------------------
// <copyright file="INavigableWorkspaceViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.UI.Navigation.Models;

namespace MyNet.UI.ViewModels;

public interface INavigableWorkspaceViewModel : IWorkspaceViewModel, INavigationPage
{
    public IWorkspaceViewModel? ParentPage { get; }

    void SetParentPage(IWorkspaceViewModel parentPage);
}
