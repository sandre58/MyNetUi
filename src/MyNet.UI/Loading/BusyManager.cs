// -----------------------------------------------------------------------
// <copyright file="BusyManager.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using MyNet.UI.Loading.Models;

namespace MyNet.UI.Loading;

public static class BusyManager
{
    private static IBusyServiceFactory? _busyServiceFactory;
    private static IBusyService? _default;

    public static IBusyService Default
    {
        get
        {
            _default ??= Create();

            return _default;
        }
    }

    public static void Initialize(IBusyServiceFactory busyServiceFactory) => _busyServiceFactory = busyServiceFactory;

    public static IBusyService Create() => _busyServiceFactory!.Create();

    public static async Task WaitIndeterminateAsync(Action<IndeterminateBusy> action)
        => await Default.WaitAsync(action).ConfigureAwait(false);

    public static async Task WaitIndeterminateAsync(Action action)
        => await Default.WaitAsync<IndeterminateBusy>(_ => action()).ConfigureAwait(false);

    public static async Task WaitDeterminateAsync(Action<DeterminateBusy> action)
        => await Default.WaitAsync(action).ConfigureAwait(false);
}
