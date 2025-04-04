// -----------------------------------------------------------------------
// <copyright file="FileNotification.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Windows.Input;
using MyNet.UI.Commands;
using MyNet.UI.Notifications;
using MyNet.UI.Resources;

namespace MyNet.UI.Services.Handlers;

public class FileNotification : ClosableNotification
{
    public string FilePath { get; }

    public ICommand OpenFileCommand { get; }

    public FileNotification(string filePath, Action<string> openAction, string? message = null, string? title = null, NotificationSeverity severity = NotificationSeverity.Success)
        : base(message ?? MessageResources.DownloadFileSuccess, title ?? UiResources.DownloadFile, severity)
    {
        FilePath = filePath;
        OpenFileCommand = CommandsManager.Create(() => openAction(FilePath));
    }

    public override bool Equals(object? obj) => obj is FileNotification other && Equals(FilePath, other.FilePath) && Equals(Title, other.Title);

    public override int GetHashCode() => FilePath.GetHashCode(StringComparison.OrdinalIgnoreCase);

    public override string ToString() => FilePath;
}
