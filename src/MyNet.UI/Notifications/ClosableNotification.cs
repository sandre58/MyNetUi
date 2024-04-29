// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Threading.Tasks;

namespace MyNet.UI.Notifications
{
    public class ClosableNotification : MessageNotification, IClosableNotification
    {
        public bool IsClosable { get; }

        public ClosableNotification(string message, string title, NotificationSeverity severity, string category = "", bool isClosable = true) : base(message, title, severity)
            => (IsClosable, Category) = (isClosable, category);

        public event CancelEventHandler? CloseRequest;

        public Task<bool> CanCloseAsync() => Task.FromResult(true);

        public void Close() => CloseRequest?.Invoke(this, new CancelEventArgs());
    }
}
