// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using MyNet.Observable;
using MyNet.UI.Commands;

namespace MyNet.UI.Busy.Models
{
    public class Busy : ObservableObject, IBusy
    {
        public Action? CancelAction { get; set; }

        public ICommand CancelCommand { get; }

        public bool IsCancellable => CancelAction is not null;

        public bool IsCancelling { get; private set; }

        public bool CanCancel { get; set; } = true;

        public Busy() => CancelCommand = CommandsManager.Create(Cancel, () => IsCancellable && CanCancel && !IsCancelling);

        public void Cancel()
        {
            IsCancelling = true;
            CancelAction?.Invoke();
        }
    }
}
