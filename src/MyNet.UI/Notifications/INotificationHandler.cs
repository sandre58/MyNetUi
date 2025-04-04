// -----------------------------------------------------------------------
// <copyright file="INotificationHandler.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.UI.Notifications;

public interface INotificationHandler : IDisposable
{
    void Subscribe(Action<IClosableNotification> action);

    void Unsubscribe(Action<Func<IClosableNotification, bool>> action);
}
