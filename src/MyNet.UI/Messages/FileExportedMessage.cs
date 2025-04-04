// -----------------------------------------------------------------------
// <copyright file="FileExportedMessage.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.UI.Messages;

public class FileExportedMessage
{
    public string FilePath { get; }

    public Action<string> OpenAction { get; }

    public FileExportedMessage(string filePath, Action<string> openAction) => (FilePath, OpenAction) = (filePath, openAction);
}
