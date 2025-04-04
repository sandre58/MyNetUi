// -----------------------------------------------------------------------
// <copyright file="SubWorkspaceNavigationService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Reactive.Disposables;
using DynamicData.Binding;
using MyNet.UI.Navigation.Models;
using MyNet.UI.ViewModels;
using MyNet.UI.ViewModels.Workspace;
using MyNet.Utilities;

namespace MyNet.UI.Navigation;

public class SubWorkspaceNavigationService : NavigationService, IDisposable
{
    private readonly IWorkspaceViewModel _workspaceViewModel;
    private readonly CompositeDisposable _disposables = [];
    private bool _disposedValue;

    public SubWorkspaceNavigationService(IWorkspaceViewModel workspaceViewModel)
    {
        _workspaceViewModel = workspaceViewModel;

        _disposables.Add(_workspaceViewModel.SubWorkspaces.ToObservableChangeSet().Subscribe(_ => CheckSelectedWorkspace()));
    }

    internal bool NavigateTo(object indexOrSubWorkspace)
    {
        var subWorkspace = GetNavigableWorkspace(indexOrSubWorkspace);

        return subWorkspace is not null && _workspaceViewModel.SubWorkspaces.Contains(subWorkspace) && NavigateTo(page: subWorkspace);
    }

    protected override bool Navigate(INavigationPage? oldPage, INavigationPage newPage, NavigationMode mode, NavigationParameters? navigationParameters)
        => _workspaceViewModel.SubWorkspaces.Contains(newPage)
            ? base.Navigate(oldPage, newPage, mode, navigationParameters)
            : CheckSelectedWorkspace();

    private INavigableWorkspaceViewModel? GetNavigableWorkspace(object indexOrSubWorkspace)
    {
        switch (indexOrSubWorkspace)
        {
            case Enum:
                return _workspaceViewModel.SubWorkspaces.GetByIndex((int)indexOrSubWorkspace);
            default:
                if (int.TryParse(indexOrSubWorkspace.ToString(), out var index))
                    return _workspaceViewModel.SubWorkspaces.GetByIndex(index);
                if (indexOrSubWorkspace is INavigableWorkspaceViewModel workspace)
                    return workspace;
                break;
        }

        return null;
    }

    public bool CheckSelectedWorkspace()
        => _workspaceViewModel.SubWorkspaces.Any() && (CurrentContext?.Page is not NavigableWorkspaceViewModel navigableWorkspaceViewModel || !_workspaceViewModel.SubWorkspaces.Contains(navigableWorkspaceViewModel))
                                                   && NavigateTo(_workspaceViewModel.SubWorkspaces[0]);

    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue)
            return;
        if (disposing)
        {
            _disposables.Dispose();
        }

        _disposedValue = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
