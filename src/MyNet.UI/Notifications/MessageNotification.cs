// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using MyNet.Observable;
using MyNet.Utilities;

namespace MyNet.UI.Notifications
{
    public class MessageNotification : ObservableObject, INotification
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Title { get; }

        public string Message { get; }

        public NotificationSeverity Severity { get; }

        public string Category { get; set; }

        public MessageNotification(string message, string title = "", NotificationSeverity severity = NotificationSeverity.Information, string? category = null)
        {
            Message = message;
            Title = title;
            Severity = severity;
            Category = category.OrEmpty();
        }

        #region Methods

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is MessageNotification other && Equals(Message, other.Message) && Equals(Title, other.Title) && Equals(Category, other.Category);

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => Message.GetHashCode();

        public override string ToString() => Message;

        #endregion Methods
    }
}
