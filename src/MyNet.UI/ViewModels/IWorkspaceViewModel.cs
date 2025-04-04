// -----------------------------------------------------------------------
// <copyright file="IWorkspaceViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MyNet.UI.Loading;
using MyNet.UI.Navigation;
using MyNet.Utilities;

namespace MyNet.UI.ViewModels;

public interface IWorkspaceViewModel : INotifyPropertyChanged, IIdentifiable<Guid>
{
    bool IsLoaded { get; }

    bool IsEnabled { get; set; }

    string? Title { get; set; }

    ScreenMode Mode { get; set; }

    INavigationService NavigationService { get; }

    IBusyService? BusyService { get; }

    ReadOnlyObservableCollection<INavigableWorkspaceViewModel> SubWorkspaces { get; }

    INavigableWorkspaceViewModel? SelectedWorkspace { get; }

    int SelectedWorkspaceIndex { get; }

    void GoToTab(object indexOrSubWorkspace);

    void Refresh();

    void Reset();

    event EventHandler? RefreshCompleted;
}
