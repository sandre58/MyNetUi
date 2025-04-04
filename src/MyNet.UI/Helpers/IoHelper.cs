// -----------------------------------------------------------------------
// <copyright file="IoHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.IO;
using MyNet.UI.Resources;
using MyNet.UI.Toasting;
using MyNet.Utilities;
using MyNet.Utilities.Helpers;

namespace MyNet.UI.Helpers;

public static class IoHelper
{
    public static void OpenFolderLocation(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);

        if (Directory.Exists(directory) && !string.IsNullOrEmpty(directory))
            ProcessHelper.OpenFolder(directory);
        else
            ToasterManager.ShowError(MessageResources.FileXNotFoundError.FormatWith(filePath));
    }
}
