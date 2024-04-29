// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.UI.Messages
{
    public class FileExportedMessage
    {
        public string FilePath { get; }

        public Action<string> OpenAction { get; }

        public FileExportedMessage(string filePath, Action<string> openAction) => (FilePath, OpenAction) = (filePath, openAction);
    }
}
