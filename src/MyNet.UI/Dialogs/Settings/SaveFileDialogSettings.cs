// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.UI.Dialogs.Settings
{
    /// <summary>
    /// Settings for SaveFileDialog.
    /// </summary>
    public class SaveFileDialogSettings : FileDialogSettings
    {
        public static SaveFileDialogSettings Default => new();

        public SaveFileDialogSettings() => CheckFileExists = false;

        /// <summary>
        /// Gets or sets a value indicating whether the dialog box prompts the user for permission
        /// to create a file if the user specifies a file that does not exist.
        /// </summary>
        public bool CreatePrompt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <b>Save As</b> dialog box displays a
        /// warning if the user specifies a file name that already exists.
        /// </summary>
        public bool OverwritePrompt { get; set; }
    }
}
