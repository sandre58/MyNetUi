// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.UI.Busy;
using MyNet.UI.Notifications;
using MyNet.UI.Services;

namespace MyNet.UI.ViewModels.Shell
{
    public class MainWindowWithFileMenuViewModel : MainWindowViewModelBase
    {
        public FileMenuViewModelBase FileMenuViewModel { get; }

        public MainWindowWithFileMenuViewModel(
            FileMenuViewModelBase fileMenuViewModel,
            INotificationsManager notificationsManager,
            IAppCommandsService appCommandsService,
            IBusyService mainBusyService)
            : base(notificationsManager, appCommandsService, mainBusyService) => FileMenuViewModel = fileMenuViewModel;

        protected override void Cleanup()
        {
            FileMenuViewModel.Dispose();
            base.Cleanup();
        }
    }
}
