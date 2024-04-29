// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.UI.Notifications
{
    public class ActionNotification : ClosableNotification
    {
        public Action<INotification>? Action { get; set; }

        public ActionNotification(string message, string title, NotificationSeverity severity, string category = "", bool isClosable = true, Action<INotification>? action = null) : base(message, title, severity, category, isClosable)
            => Action = action;

    }
}
