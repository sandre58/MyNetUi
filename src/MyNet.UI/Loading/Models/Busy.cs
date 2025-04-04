// -----------------------------------------------------------------------
// <copyright file="Busy.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Windows.Input;
using MyNet.Observable;
using MyNet.UI.Commands;

namespace MyNet.UI.Loading.Models;

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
