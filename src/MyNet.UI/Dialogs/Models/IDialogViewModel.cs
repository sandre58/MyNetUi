// -----------------------------------------------------------------------
// <copyright file="IDialogViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.UI.Dialogs.Models;

public interface IDialogViewModel : IClosable
{
    string? Title { get; set; }

    bool? DialogResult { get; }

    bool LoadWhenDialogOpening { get; }

    void Load();
}
