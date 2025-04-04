// -----------------------------------------------------------------------
// <copyright file="NotificationsManager.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using DynamicData.Binding;
using MyNet.Utilities.Collections;

namespace MyNet.UI.Notifications;

public sealed class NotificationsManager : INotificationsManager, IDisposable
{
    private readonly OptimizedObservableCollection<IClosableNotification> _notifications = [];
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

    public INotificationsManager AddHandler<T>()
        where T : INotificationHandler, new()
        => AddHandler(new T());

    private void RemoveNotifications(Func<IClosableNotification, bool> predicate) => _notifications.RemoveMany([.. _notifications.Where(predicate)]);

    private void AddNotification(IClosableNotification notification)
    {
        if (!_notifications.Contains(notification))
            _notifications.Add(notification);
    }

    public void Clear() => _notifications.Clear();

    public void Dispose() => _handlers.ForEach(x => x.Dispose());
}
