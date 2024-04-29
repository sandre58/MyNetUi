// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using DynamicData.Binding;

namespace MyNet.UI.Notifications
{
    public sealed class NotificationsManager : INotificationsManager, IDisposable
    {
        private readonly ObservableCollection<IClosableNotification> _notifications = [];
        private readonly List<INotificationHandler> _handlers = [];

        public ReadOnlyObservableCollection<IClosableNotification> Notifications { get; }

        public NotificationsManager()
        {
            Notifications = new(_notifications);

            _ = _notifications.ToObservableChangeSet(x => x.Id)
                              .DisposeMany()
                              .OnItemAdded(x => x.CloseRequest += Notification_CloseRequest)
                              .OnItemRemoved(x => x.CloseRequest -= Notification_CloseRequest)
                              .Subscribe();
        }

        private void Notification_CloseRequest(object? sender, System.ComponentModel.CancelEventArgs e) => _notifications.Remove((IClosableNotification)sender!);

        public INotificationsManager AddHandler(INotificationHandler handler)
        {
            _handlers.Add(handler);
            handler.Subscribe(AddNotification);
            handler.Unsubscribe(RemoveNotifications);

            return this;
        }

        public INotificationsManager AddHandler<T>() where T : INotificationHandler, new() => AddHandler(new T());

        private void RemoveNotifications(Func<IClosableNotification, bool> predicate) => _notifications.RemoveMany(_notifications.Where(predicate).ToList());

        private void AddNotification(IClosableNotification notification)
        {
            if (!_notifications.Contains(notification))
                _notifications.Add(notification);
        }

        public void Clear() => _notifications.Clear();

        public void Dispose()
        {
            _handlers.ForEach(x => x.Dispose());
            GC.SuppressFinalize(this);
        }
    }
}
