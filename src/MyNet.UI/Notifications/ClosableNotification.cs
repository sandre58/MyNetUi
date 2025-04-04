// -----------------------------------------------------------------------
// <copyright file="ClosableNotification.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.Threading.Tasks;

namespace MyNet.UI.Notifications;

public class ClosableNotification(string message, string title, NotificationSeverity severity, bool isClosable = true) : MessageNotification(message, title, severity), IClosableNotification
{
    public bool IsClosable { get; } = isClosable;

    public event CancelEventHandler? CloseRequest;

    public Task<bool> CanCloseAsync() => Task.FromResult(true);

    public void Close() => CloseRequest?.Invoke(this, new CancelEventArgs());
}
