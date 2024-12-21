// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using MyNet.UI.Commands;
using MyNet.UI.Notifications;
using MyNet.UI.Resources;

namespace MyNet.UI.Services.Handlers
{
    public class FileNotification : ClosableNotification
    {
        public static readonly string FileCategory = "File";

        public string FilePath { get; }

        public ICommand OpenFileCommand { get; }

        public FileNotification(string filePath, Action<string> openAction, string? message = null, string? title = null, NotificationSeverity severity = NotificationSeverity.Success)
            : base(message ?? MessageResources.DownloadFileSuccess, title ?? UiResources.DownloadFile, severity, FileCategory, true)
        {
            FilePath = filePath;
            OpenFileCommand = CommandsManager.Create(() => openAction(FilePath));
        }

        public override bool Equals(object? obj) => obj is FileNotification other && Equals(FilePath, other.FilePath) && Equals(Title, other.Title) && Equals(Category, other.Category);

        public override int GetHashCode() => FilePath.GetHashCode();

        public override string ToString() => FilePath;
    }
}
