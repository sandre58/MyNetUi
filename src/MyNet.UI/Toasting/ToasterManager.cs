// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using MyNet.UI.Notifications;
using MyNet.UI.Resources;
using MyNet.UI.Toasting.Settings;
using MyNet.Utilities;

namespace MyNet.UI.Toasting
{
    public static class ToasterManager
    {
        private static IToasterService? _toasterService;

        public static void Initialize(IToasterService toasterService) => _toasterService = toasterService;

        /// <summary>
        /// Show a toast notification.
        /// </summary>
        public static void ShowSuccess(string? message,
            ToastClosingStrategy closingStrategy = ToastClosingStrategy.AutoClose,
            bool isUnique = false,
            Action<INotification>? onClick = null,
            Action? onClose = null) => ShowMessage(message, UiResources.Success, NotificationSeverity.Success, closingStrategy, isUnique, onClick, onClose);

        /// <summary>
        /// Show a toast notification.
        /// </summary>
        public static void ShowError(string? message,
            ToastClosingStrategy closingStrategy = ToastClosingStrategy.AutoClose,
            bool isUnique = false,
            Action<INotification>? onClick = null,
            Action? onClose = null) => ShowMessage(message, UiResources.Error, NotificationSeverity.Error, closingStrategy, isUnique, onClick, onClose);

        /// <summary>
        /// Show a toast notification.
        /// </summary>
        public static void ShowInformation(string? message,
            ToastClosingStrategy closingStrategy = ToastClosingStrategy.AutoClose,
            bool isUnique = false,
            Action<INotification>? onClick = null,
            Action? onClose = null) => ShowMessage(message, UiResources.Information, NotificationSeverity.Information, closingStrategy, isUnique, onClick, onClose);

        /// <summary>
        /// Show a toast notification.
        /// </summary>
        public static void ShowWarning(string? message,
            ToastClosingStrategy closingStrategy = ToastClosingStrategy.AutoClose,
            bool isUnique = false,
            Action<INotification>? onClick = null,
            Action? onClose = null) => ShowMessage(message, UiResources.Warning, NotificationSeverity.Warning, closingStrategy, isUnique, onClick, onClose);

        /// <summary>
        /// Show a toast notification.
        /// </summary>
        public static void ShowMessage(string? message,
            string title = "",
            NotificationSeverity severity = NotificationSeverity.Information,
            ToastClosingStrategy closingStrategy = ToastClosingStrategy.AutoClose,
            bool isUnique = false,
            Action<INotification>? onClick = null,
            Action? onClose = null) => Show(new MessageNotification(message.OrEmpty(), title, severity), SettingsFromSeverity(severity, closingStrategy), isUnique, onClick, onClose);

        /// <summary>
        /// Show a toast notification.
        /// </summary>
        public static void Show(INotification notification,
            ToastSettings? settings = null,
            bool isUnique = false,
            Action<INotification>? onClick = null,
            Action? onClose = null) => _toasterService?.Show(notification, settings ?? ToastSettings.Default, isUnique, onClick, onClose);

        /// <summary>
        /// Hides all toast.
        /// </summary>
        public static void Clear() => _toasterService?.Clear();

        /// <summary>
        ///  Hide a toast.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toast"></param>
        public static void Hide(INotification toast) => _toasterService?.Hide(toast);

        public static ToastSettings SettingsFromSeverity(NotificationSeverity severity, ToastClosingStrategy toastClosingStrategy)
        {
            var settings = new ToastSettings() { ClosingStrategy = toastClosingStrategy };

            switch (severity)
            {
                case NotificationSeverity.Error:
                    settings.Style.Add("Background", "My.Brushes.Negative");
                    settings.Style.Add("BorderBrush", "My.Brushes.Negative");
                    break;

                case NotificationSeverity.Success:
                    settings.Style.Add("Background", "My.Brushes.Positive");
                    settings.Style.Add("BorderBrush", "My.Brushes.Positive");
                    break;

                case NotificationSeverity.Warning:
                    settings.Style.Add("Background", "My.Brushes.Warning");
                    settings.Style.Add("BorderBrush", "My.Brushes.Warning");
                    break;

                case NotificationSeverity.Information:
                    settings.Style.Add("Background", "My.Brushes.Information");
                    settings.Style.Add("BorderBrush", "My.Brushes.Information");
                    break;
                default:
                    break;
            }

            return settings;
        }
    }
}
