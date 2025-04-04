// -----------------------------------------------------------------------
// <copyright file="INotification.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using MyNet.Utilities;

namespace MyNet.UI.Notifications;

public interface INotification : INotifyPropertyChanged, IIdentifiable<Guid>, ISimilar
{
    NotificationSeverity Severity { get; }
}
