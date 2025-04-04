// -----------------------------------------------------------------------
// <copyright file="WorkspaceNavigationService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.UI.Navigation.Models;
using MyNet.UI.ViewModels;
using MyNet.UI.ViewModels.Workspace;
using MyNet.Utilities.Suspending;

namespace MyNet.UI.Navigation;

public class WorkspaceNavigationService : NavigationService
{
    private readonly Suspender _subWorkspaceNavigationSuspender = new();

    protected override void OnNavigated(NavigationContext navigatingContext)
    {
        using (_subWorkspaceNavigationSuspender.Suspend())
        {
            base.OnNavigated(navigatingContext);

            if (navigatingContext.OldPage is IWorkspaceViewModel oldNavigable)
            {
                oldNavigable.NavigationService.Navigated -= OnSubWorkspaceNavigatedCallback;
            }

            if (navigatingContext.Page is IWorkspaceViewModel newNavigable)
            {
                newNavigable.NavigationService.Navigated += OnSubWorkspaceNavigatedCallback;
            }
        }
    }

    private void OnSubWorkspaceNavigatedCallback(object? sender, NavigationEventArgs e)
    {
        if (CurrentContext is null || _subWorkspaceNavigationSuspender.IsSuspended)
            return;
        if (e.OldPage is not null)
            UpdateJournal(NavigationMode.Normal, new SubWorskpaceNavigationContext(CurrentContext.Page, e.OldPage, e.OldPage, e.Mode, CurrentContext.Parameters));
        UpdateCurrentContext(new SubWorskpaceNavigationContext(CurrentContext.Page, e.OldPage, e.NewPage, e.Mode, CurrentContext.Parameters));
    }
}

internal sealed class SubWorskpaceNavigationContext : NavigatingContext
{
    public SubWorskpaceNavigationContext(INavigationPage page, INavigationPage? oldTab, INavigationPage newTab, NavigationMode mode, NavigationParameters? navigationParameters = null)
        : base(page, page, mode, navigationParameters?.Clone() ?? [])
    {
        OldTab = oldTab;
        NewTab = newTab;

        Parameters?.AddOrUpdate(WorkspaceViewModel.TabParameterKey, newTab);
    }

    public INavigationPage? OldTab { get; set; }

    public INavigationPage NewTab { get; set; }
}
