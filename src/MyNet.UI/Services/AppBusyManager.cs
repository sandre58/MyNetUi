// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using MyNet.UI.Busy;
using MyNet.UI.Busy.Models;

namespace MyNet.UI.Services
{
    public static class AppBusyManager
    {
        private static IBusyServiceFactory? _busyServiceFactory;
        private static IBusyService? _mainBusyService;
        private static IBusyService? _backgroundBusyService;

        public static void Initialize(IBusyServiceFactory busyServiceFactory) => _busyServiceFactory = busyServiceFactory;

        public static IBusyService MainBusyService => GetOrCreate(ref _mainBusyService);

        public static IBusyService BackgroundBusyService => GetOrCreate(ref _backgroundBusyService);

        private static IBusyService GetOrCreate(ref IBusyService? busyService)
        {
            if (busyService is null)
            {
                if (_busyServiceFactory is null)
                    throw new InvalidOperationException("Busy Service has not been Initialized.");

                busyService = _busyServiceFactory.Create();
            }

            return busyService;
        }

        public static async Task BackgroundAsync(Action<IndeterminateBusy> action) => await BackgroundBusyService.WaitAsync(action).ConfigureAwait(false);

        public static async Task BackgroundAsync(Action action) => await BackgroundBusyService.WaitAsync<IndeterminateBusy>(_ => action()).ConfigureAwait(false);

        public static async Task WaitAsync(Action<IndeterminateBusy> action) => await MainBusyService.WaitAsync(action).ConfigureAwait(false);

        public static async Task WaitAsync(Action action) => await MainBusyService.WaitAsync<IndeterminateBusy>(_ => action()).ConfigureAwait(false);

        public static async Task ProgressAsync(Action action) => await MainBusyService.WaitAsync<ProgressionBusy>(_ => action()).ConfigureAwait(false);

        public static async Task ProgressAsync(Task task) => await MainBusyService.WaitAsync(new Func<ProgressionBusy, Task>(_ => task)).ConfigureAwait(false);

        public static void Resume() => MainBusyService.Resume();

        public static void Progress() => _ = MainBusyService.Wait<ProgressionBusy>();
    }
}
