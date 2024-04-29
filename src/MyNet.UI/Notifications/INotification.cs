// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using MyNet.Utilities;

namespace MyNet.UI.Notifications
{
    public interface INotification : INotifyPropertyChanged, IIdentifiable<Guid>
    {
        string Category { get; }
    }
}
