// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.UI.Dialogs.Models;
using MyNet.UI.Dialogs.Settings;

namespace MyNet.UI.Dialogs
{
    public interface IMessageBoxFactory
    {
        IMessageBox Create(string message, string? title, MessageSeverity severity, MessageBoxResultOption buttons, MessageBoxResult defaultResut);
    }
}
