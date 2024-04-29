// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.UI.Dialogs.Messages
{
    public enum DialogType
    {
        WorkspaceDialog,

        Dialog,

        ModalDialog,

        MessageBox,

        FileDialog
    }

    public class OpenDialogMessage(DialogType type, object? dialog)
    {
        public DialogType Type { get; } = type;

        public object? Dialog { get; } = dialog;
    }
}
