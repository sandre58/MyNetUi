// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.UI.Dialogs.Models
{
    public interface IDialogViewModel : IClosable
    {

        string? Title { get; set; }

        bool? DialogResult { get; }

        bool LoadWhenDialogOpening { get; }

        void Load();
    }
}
