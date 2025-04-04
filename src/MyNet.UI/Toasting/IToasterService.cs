// -----------------------------------------------------------------------
// <copyright file="IToasterService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.UI.Notifications;
using MyNet.UI.Toasting.Settings;

namespace MyNet.UI.Toasting;

/// <summary>
/// Interface abstracting the interaction with toast notification.
/// </summary>
public interface IToasterService
{
    /// <summary>
    /// Displays a modal dialog of a type that is determined by the dialog type locator.
    /// </summary>
    void Show(INotification notification, ToastSettings settings, bool isUnique = false, Action<INotification>? onClick = null, Action? onClose = null);

    /// <summary>
    /// Hide all messages.
    /// </summary>
    void Clear();

    /// <summary>
    /// Hide a message if is displayed.
    /// </summary>
    void Hide(INotification notification);
}
