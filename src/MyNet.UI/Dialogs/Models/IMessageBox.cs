// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.UI.Dialogs.Settings;

namespace MyNet.UI.Dialogs.Models
{
    public interface IMessageBox
    {
        string? Title { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="MessageBoxResultOption"/> value that specifies which button or
        /// buttons to display. Default value is <see cref="MessageBoxResultOption.Ok"/>.
        /// </summary>
        MessageBoxResultOption Buttons { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="MessageBoxResult"/> value that specifies the default result
        /// of the message box. Default value is <see cref="MessageBoxResult.OK"/>.
        /// </summary>
        MessageBoxResult DefaultResult { get; set; }
    }
}
