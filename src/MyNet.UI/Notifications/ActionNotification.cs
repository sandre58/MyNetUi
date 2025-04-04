// -----------------------------------------------------------------------
// <copyright file="ActionNotification.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.UI.Notifications;

public class ActionNotification(string message, string title, NotificationSeverity severity, bool isClosable = true, Action<INotification>? action = null) : ClosableNotification(message, title, severity, isClosable)
{
    public Action<INotification>? Action { get; set; } = action;
}
