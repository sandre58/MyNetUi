// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.IO;
using MyNet.UI.Resources;
using MyNet.UI.Toasting;
using MyNet.Utilities;
using MyNet.Utilities.Helpers;

namespace MyNet.UI.Helpers
{
    public static class IOHelper
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
}
