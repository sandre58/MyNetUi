// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using MyNet.UI.Messages;
using MyNet.UI.Notifications;
using MyNet.UI.Toasting;
using MyNet.UI.Toasting.Settings;
using MyNet.Utilities;
using MyNet.Utilities.Exceptions;
using MyNet.Utilities.Extensions;
using MyNet.Utilities.Logging;
using MyNet.Utilities.Messaging;

namespace MyNet.UI.Extensions
{
    public static class ExceptionExtensions
    {
        public static void ShowInToaster(this Exception exception, bool showInTaskBar = false, bool autoClose = true, Action<INotification>? onClick = null)
        {
            var innerException = exception.InnerException ?? exception;
            LogManager.Error(innerException);

            Action? action = null;
            if (showInTaskBar)
            {
                Messenger.Default.Send(new UpdateTaskBarInfoMessage(TaskbarProgressState.Error, 1));
                action = () => Messenger.Default.Send(new UpdateTaskBarInfoMessage(TaskbarProgressState.None, 0));
            }

            if (innerException is TranslatableException translatableException)
                ToasterManager.ShowError(translatableException.Parameters is not null ? translatableException.ResourceKey.Translate().FormatWith(translatableException.Parameters) : translatableException.ResourceKey.Translate(), autoClose ? ToastClosingStrategy.AutoClose : ToastClosingStrategy.CloseButton, false, onClick, action);
            else
                ToasterManager.ShowError(exception.Message, autoClose ? ToastClosingStrategy.AutoClose : ToastClosingStrategy.CloseButton, false, onClick, action);
        }
    }
}
