// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.UI.Notifications
{
    public interface INotificationHandler : IDisposable
    {
        void Subscribe(Action<IClosableNotification> action);

        void Unsubscribe(Action<Func<IClosableNotification, bool>> action);
    }

}
