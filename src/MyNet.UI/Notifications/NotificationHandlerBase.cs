// -----------------------------------------------------------------------
// <copyright file="NotificationHandlerBase.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reactive.Subjects;

namespace MyNet.UI.Notifications;

public abstract class NotificationHandlerBase : INotificationHandler
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Dispose in Cleanup")]
    private readonly Subject<IClosableNotification> _notify = new();
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Dispose in Cleanup")]
    private readonly Subject<Func<IClosableNotification, bool>> _unnotify = new();
    private bool _disposedValue;

    protected void Notify(IClosableNotification notification) => _notify.OnNext(notification);

    protected void Unnotify(Func<IClosableNotification, bool> canRemove) => _unnotify.OnNext(canRemove);

    public void Subscribe(Action<IClosableNotification> action) => _notify.Subscribe(action);

    public void Unsubscribe(Action<Func<IClosableNotification, bool>> action) => _unnotify.Subscribe(action);

    protected virtual void Cleanup()
    {
        _notify.Dispose();
        _unnotify.Dispose();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue)
            return;
        if (disposing)
        {
            Cleanup();
        }

        _disposedValue = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
