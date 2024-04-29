// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Subjects;

namespace MyNet.UI.Notifications
{
    public abstract class NotificationHandlerBase : INotificationHandler
    {
        private readonly Subject<IClosableNotification> _notify = new();
        private readonly Subject<Func<IClosableNotification, bool>> _unnotify = new();
        private bool _disposedValue;

        protected NotificationHandlerBase() { }

        protected void Notify(IClosableNotification notification) => _notify.OnNext(notification);

        protected void Unnotify(Func<IClosableNotification, bool> canRemove) => _unnotify.OnNext(canRemove);

        public void Subscribe(Action<IClosableNotification> action) => _notify.Subscribe(action);

        public void Unsubscribe(Action<Func<IClosableNotification, bool>> action) => _unnotify.Subscribe(action);

        protected virtual void Cleanup()
        {
            // Method intentionally left empty.
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Cleanup();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
