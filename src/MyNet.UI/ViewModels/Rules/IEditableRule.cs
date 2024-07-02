// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

namespace MyNet.UI.ViewModels.Rules
{
    public interface IEditableRule : INotifyPropertyChanged
    {
        public bool CanRemove { get; }

        public bool CanMove { get; }
    }
}
