// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace MyNet.UI.Dialogs.Settings
{
    /// <summary>
    /// Settings for FileDialog.
    /// </summary>
    public abstract class FileDialogSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether the dialog box displays a warning if the user
        /// specifies a file name that does not exist.
        /// </summary>
        public bool CheckFileExists { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the dialog box displays a warning if the user
        /// specifies a path that does not exist.
        /// </summary>
        public bool CheckPathExists { get; set; } = true;

        /// <summary>
        /// Gets or sets the default file name extension.
        /// </summary>
        public string DefaultExtension { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a string containing the file name selected in the file dialog box.
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Gets the file names of all selected files in the dialog box.
        /// </summary>
        public IEnumerable<string> FileNames { get; set; } = [];

        /// <summary>
        /// Gets or sets the current file name filter string, which determines the choices that
        /// appear in the "Save as file type" or "Files of type" box in the dialog box.
        /// </summary>
        public IDictionary<string, string>? Filters { get; set; }

        /// <summary>
        /// Gets or sets the initial directory displayed by the file dialog box.
        /// </summary>
        public string InitialDirectory { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the file dialog box title.
        /// </summary>
        public string Title { get; set; } = string.Empty;
    }
}
