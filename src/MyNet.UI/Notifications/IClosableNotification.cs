// -----------------------------------------------------------------------
// <copyright file="IClosableNotification.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.UI.Notifications;

public interface IClosableNotification : INotification, IClosable
{
    bool IsClosable { get; }
}
