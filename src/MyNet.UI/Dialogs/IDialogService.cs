// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MyNet.UI.Dialogs.Models;
using MyNet.UI.Dialogs.Settings;

namespace MyNet.UI.Dialogs
{
    /// <summary>
    /// Interface abstracting the interaction between view models and views when it comes to
    /// opening dialogs using the MVVM pattern in WPF.
    /// </summary>
    public interface IDialogService
    {
        ObservableCollection<IDialogViewModel> OpenedDialogs { get; }

        /// <summary>
        /// Displays a non-modal custom dialog of specified type.
        /// </summary>
        /// <param name="viewModel">The view model of the new custom dialog.</param>
        /// <param name="view">The type of the custom dialog to show.</param>
        Task ShowAsync(object view, IDialogViewModel viewModel);

        /// <summary>
        /// Displays a modal custom dialog of specified type.
        /// </summary>
        /// <param name="viewModel">The view model of the new custom dialog.</param>
        /// <param name="view">The type of the custom dialog to show.</param>
        Task<bool?> ShowDialogAsync(object view, IDialogViewModel viewModel);

        /// <summary>
        /// Displays a message box that has a message, title bar caption, button, and icon; and
        /// that accepts a default message box result and returns a result.
        /// </summary>
        /// <returns>
        /// A <see cref="MessageBoxResult"/> value that specifies which message box button is
        /// clicked by the user.
        /// </returns>
        Task<MessageBoxResult?> ShowMessageBoxAsync(IMessageBox viewModel);

        /// <summary>
        /// Displays the <see cref="OpenFileDialog"/>.
        /// </summary>
        /// <param name="settings">The settings for the open file dialog.</param>
        /// <returns>
        /// If the user clicks the OK button of the dialog that is displayed, true is returned;
        /// otherwise false.
        /// </returns>
        Task<bool?> ShowOpenFileDialogAsync(OpenFileDialogSettings settings);

        /// <summary>
        /// Displays the <see cref="SaveFileDialog"/>.
        /// </summary>
        /// <param name="settings">The settings for the save file dialog.</param>
        /// <returns>
        /// If the user clicks the OK button of the dialog that is displayed, true is returned;
        /// otherwise false.
        /// </returns>
        Task<bool?> ShowSaveFileDialogAsync(SaveFileDialogSettings settings);

        /// <summary>
        /// Displays the <see cref="FolderBrowserDialog"/>.
        /// </summary>
        /// <param name="settings">The settings for the folder browser dialog.</param>
        /// <returns>
        /// If the user clicks the OK button of the dialog that is displayed, true is returned;
        /// otherwise false.
        /// </returns>
        Task<bool?> ShowFolderDialogAsync(OpenFolderDialogSettings settings);
    }
}
