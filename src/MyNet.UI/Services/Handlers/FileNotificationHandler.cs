// -----------------------------------------------------------------------
// <copyright file="FileNotificationHandler.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.UI.Messages;
using MyNet.UI.Notifications;
using MyNet.UI.Toasting;
using MyNet.UI.Toasting.Settings;
using MyNet.Utilities.Messaging;

namespace MyNet.UI.Services.Handlers;

public sealed class FileNotificationHandler : NotificationHandlerBase
{
    public static readonly string ExportFileCategory = "ExportFile";

    public FileNotificationHandler() => Messenger.Default.Register<FileExportedMessage>(this, OnFileExportedMessage);

    private void OnFileExportedMessage(FileExportedMessage obj)
    {
        var notification = new FileNotification(obj.FilePath, obj.OpenAction);
        ToasterManager.Show(notification, new ToastSettings { ClosingStrategy = ToastClosingStrategy.AutoClose }, true, _ => obj.OpenAction(obj.FilePath));

        Notify(notification);
    }

    protected override void Cleanup() => Messenger.Default.Unregister(this);
}
