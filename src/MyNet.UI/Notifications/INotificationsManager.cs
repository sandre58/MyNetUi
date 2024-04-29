// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;

namespace MyNet.UI.Notifications
{
    public interface INotificationsManager
    {
        ReadOnlyObservableCollection<IClosableNotification> Notifications { get; }

        void Clear();

        INotificationsManager AddHandler(INotificationHandler handler);

        INotificationsManager AddHandler<T>() where T : INotificationHandler, new();

    }
}
