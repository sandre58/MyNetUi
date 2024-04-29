// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.UI.Busy;
using MyNet.UI.Busy.Models;
using System;
using System.Threading.Tasks;

namespace MyNet.UI.Extensions
{
    public static class BusyExtensions
    {
        /// <summary>
        /// Show busy control during action.
        /// </summary>
        public static async Task WaitIndeterminateAsync(this IBusyService busyService, Action<IndeterminateBusy> action) => await busyService.WaitAsync(action);

        /// <summary>
        /// Show busy control during action.
        /// </summary>
        public static async Task WaitIndeterminateAsync(this IBusyService busyService, Action action) => await busyService.WaitAsync<IndeterminateBusy>(_ => action());

        /// <summary>
        /// Show busy control during action.
        /// </summary>
        public static async Task WaitDeterminateAsync(this IBusyService busyService, Action<DeterminateBusy> action) => await busyService.WaitAsync(action);
    }
}
