// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using DynamicData.Binding;
using MyNet.Observable;
using MyNet.Observable.Collections;
using MyNet.Observable.Collections.Extensions;
using MyNet.Observable.Threading;
using MyNet.UI.Commands;
using MyNet.UI.Dialogs.Messages;
using MyNet.UI.Messages;
using MyNet.UI.Notifications;
using MyNet.Utilities;
using MyNet.Utilities.Messaging;

namespace MyNet.UI.ViewModels.Shell
{
    public class NotificationsViewModel : ObservableObject
    {
        public bool IsVisible { get; set; }

        public ExtendedCollection<IClosableNotification> Notifications { get; }

        public NotificationSeverity? MaxSeverity { get; private set; }

        public ICommand ExecuteActionCommand { get; }

        public ICommand CloseCommand { get; }

        public ICommand ClearCommand { get; }

        public NotificationsViewModel(INotificationsManager notificationsManager)
        {
            Notifications = new ExtendedCollection<IClosableNotification>(notificationsManager.Notifications.ToObservableChangeSet(), Scheduler.UI);

            ExecuteActionCommand = CommandsManager.CreateNotNull<ActionNotification>(ExecuteAction, x => x.Action is not null);
            CloseCommand = CommandsManager.CreateNotNull<IClosableNotification>(x => x.Close());
            ClearCommand = CommandsManager.Create(() => Notifications.Where(x => x.IsClosable).ToList().ForEach(x => x.Close()), () => Notifications.Any(x => x.IsClosable));

            Disposables.AddRange(
            [
                Notifications.ToObservableChangeSet().Subscribe(_ =>
                {
                    MaxSeverity = Notifications.Count != 0 ? Notifications.OfType<MessageNotification>().Max(x => x.Severity) : null;

                    if(Notifications.Count == 0)
                        UpdateVisibility(VisibilityAction.Hide);
                })
            ]);

            Messenger.Default.Register<UpdateNotificationsVisibilityRequestedMessage>(this, x => UpdateVisibility(x.VisibilityAction));
            Messenger.Default.Register<OpenDialogMessage>(this, OnOpenDialog);
        }

        private void ExecuteAction(ActionNotification notification) => notification.Action?.Invoke(notification);

        private void UpdateVisibility(VisibilityAction visibilityAction)
            => IsVisible = visibilityAction == VisibilityAction.Toggle ? !IsVisible : visibilityAction != VisibilityAction.Hide;

        private void OnOpenDialog(OpenDialogMessage message)
        {
            if (message.Type != DialogType.FileDialog)
            {
                UpdateVisibility(VisibilityAction.Hide);
            }
        }
    }
}
