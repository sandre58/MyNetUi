// -----------------------------------------------------------------------
// <copyright file="OpenDialogMessage.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.UI.Dialogs.Messages;

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
