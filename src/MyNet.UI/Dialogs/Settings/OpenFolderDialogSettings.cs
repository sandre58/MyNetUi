// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.UI.Dialogs.Settings
{
    public class OpenFolderDialogSettings
    {
        public static OpenFolderDialogSettings Default => new();

        public bool CheckPathExists { get; set; } = true;

        public string Folder { get; set; } = string.Empty;

        public string InitialDirectory { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
    }
}
