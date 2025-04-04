// -----------------------------------------------------------------------
// <copyright file="INotificationsManager.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace MyNet.UI.Notifications;

public interface INotificationsManager
{
    ReadOnlyObservableCollection<IClosableNotification> Notifications { get; }

    void Clear();

    INotificationsManager AddHandler(INotificationHandler handler);

    INotificationsManager AddHandler<T>()
        where T : INotificationHandler, new();
}
