// -----------------------------------------------------------------------
// <copyright file="MessageNotification.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Observable;

namespace MyNet.UI.Notifications;

public class MessageNotification(string message, string title = "", NotificationSeverity severity = NotificationSeverity.Information) : ObservableObject, INotification
{
    public Guid Id { get; } = Guid.NewGuid();

    public string Title { get; } = title;

    public string Message { get; } = message;

    public NotificationSeverity Severity { get; } = severity;

    #region Methods

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj) => ReferenceEquals(this, obj);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode() => Message.GetHashCode(StringComparison.OrdinalIgnoreCase);

    public override string ToString() => Message;

    public bool IsSimilar(object? obj) => obj is MessageNotification other && Equals(Message, other.Message) && Equals(Title, other.Title);

    #endregion Methods
}
