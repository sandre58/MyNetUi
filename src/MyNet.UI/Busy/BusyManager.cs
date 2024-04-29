// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MyNet.UI.Busy.Models;

namespace MyNet.UI.Busy
{
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
            => await Default.WaitAsync(action);

        public static async Task WaitIndeterminateAsync(Action action)
            => await Default.WaitAsync<IndeterminateBusy>(_ => action());

        public static async Task WaitDeterminateAsync(Action<DeterminateBusy> action)
            => await Default.WaitAsync(action);
    }
}
