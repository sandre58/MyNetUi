// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.UI.Notifications
{
    public interface IClosableNotification : INotification, IClosable
    {
        bool IsClosable { get; }
    }
}
