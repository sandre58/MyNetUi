// -----------------------------------------------------------------------
// <copyright file="IBusy.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.UI.Loading.Models;

public interface IBusy
{
    bool IsCancellable { get; }

    bool IsCancelling { get; }

    bool CanCancel { get; set; }

    void Cancel();
}
