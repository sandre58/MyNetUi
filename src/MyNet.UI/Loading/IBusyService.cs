// -----------------------------------------------------------------------
// <copyright file="IBusyService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using MyNet.UI.Loading.Models;

namespace MyNet.UI.Loading;

public interface IBusyService : INotifyPropertyChanged
{
    Task WaitAsync<TBusy>(Action<TBusy> action)
        where TBusy : class, IBusy, new();

    Task WaitAsync<TBusy>(Func<TBusy, Task> action)
        where TBusy : class, IBusy, new();

    TBusy Wait<TBusy>()
        where TBusy : class, IBusy, new();

    void Resume();

    TBusy? GetCurrent<TBusy>()
        where TBusy : class, IBusy;

    bool IsBusy { get; }
}
