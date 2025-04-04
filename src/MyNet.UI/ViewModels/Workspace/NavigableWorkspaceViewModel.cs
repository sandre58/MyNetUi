// -----------------------------------------------------------------------
// <copyright file="NavigableWorkspaceViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using MyNet.Observable.Attributes;
using MyNet.UI.Navigation.Models;

namespace MyNet.UI.ViewModels.Workspace;

[CanBeValidatedForDeclaredClassOnly(false)]
[CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
public class NavigableWorkspaceViewModel : WorkspaceViewModel, INavigableWorkspaceViewModel
{
    public IWorkspaceViewModel? ParentPage { get; private set; }

    public virtual void SetParentPage(IWorkspaceViewModel parentPage) => ParentPage = parentPage;

    public virtual Type? GetParentPageType() => null;

    public virtual void LoadParameters(INavigationParameters? parameters)
    {
        if (parameters?.Get<object>(TabParameterKey) is { } tab)
            GoToTab(tab);
    }

    public virtual void OnNavigated(NavigationContext navigationContext)
    {
        var canRefresh = CanRefreshOnNavigatedTo(navigationContext);
        LoadParameters(navigationContext.Parameters);

        if (!IsLoaded || canRefresh)
            Refresh();
    }

    public virtual void OnNavigatingFrom(NavigatingContext navigatingContext) { }

    public virtual void OnNavigatingTo(NavigatingContext navigatingContext) => navigatingContext.Cancel = !CanNavigateTo(navigatingContext);

    [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "Used by children classes")]
    protected virtual bool CanRefreshOnNavigatedTo(NavigationContext navigationContext) => false;

    protected virtual bool CanNavigateTo(NavigatingContext navigatingContext) => navigatingContext.OldPage is null || !Equals(navigatingContext.OldPage, navigatingContext.Page) || (!navigatingContext.Parameters?.Equals(NavigationService.CurrentContext) ?? false);
}
