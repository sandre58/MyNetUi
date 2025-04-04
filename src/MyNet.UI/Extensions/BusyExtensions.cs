// -----------------------------------------------------------------------
// <copyright file="BusyExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using MyNet.UI.Loading;
using MyNet.UI.Loading.Models;

namespace MyNet.UI.Extensions;

public static class BusyExtensions
{
    /// <summary>
    /// Show busy control during action.
    /// </summary>
    public static async Task WaitIndeterminateAsync(this IBusyService busyService, Action<IndeterminateBusy> action) => await busyService.WaitAsync(action).ConfigureAwait(false);

    /// <summary>
    /// Show busy control during action.
    /// </summary>
    public static async Task WaitIndeterminateAsync(this IBusyService busyService, Action action) => await busyService.WaitAsync<IndeterminateBusy>(_ => action()).ConfigureAwait(false);

    /// <summary>
    /// Show busy control during action.
    /// </summary>
    public static async Task WaitDeterminateAsync(this IBusyService busyService, Action<DeterminateBusy> action) => await busyService.WaitAsync(action).ConfigureAwait(false);
}
