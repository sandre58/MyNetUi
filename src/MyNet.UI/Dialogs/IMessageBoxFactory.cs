// -----------------------------------------------------------------------
// <copyright file="IMessageBoxFactory.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.UI.Dialogs.Models;
using MyNet.UI.Dialogs.Settings;

namespace MyNet.UI.Dialogs;

public interface IMessageBoxFactory
{
    IMessageBox Create(string message, string? title, MessageSeverity severity, MessageBoxResultOption buttons, MessageBoxResult defaultResut);
}
