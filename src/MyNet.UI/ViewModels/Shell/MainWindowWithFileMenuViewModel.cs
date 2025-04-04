// -----------------------------------------------------------------------
// <copyright file="MainWindowWithFileMenuViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.UI.Loading;
using MyNet.UI.Notifications;
using MyNet.UI.Services;

namespace MyNet.UI.ViewModels.Shell;

public class MainWindowWithFileMenuViewModel(
    FileMenuViewModelBase fileMenuViewModel,
    INotificationsManager notificationsManager,
    IAppCommandsService appCommandsService,
    IBusyService mainBusyService) : MainWindowViewModelBase(notificationsManager, appCommandsService, mainBusyService)
{
    public FileMenuViewModelBase FileMenuViewModel { get; } = fileMenuViewModel;

    protected override void Cleanup()
    {
        FileMenuViewModel.Dispose();
        base.Cleanup();
    }
}
