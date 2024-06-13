// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Concurrency;

namespace MyNet.UI.Threading
{
    public static class Scheduler
    {
        private static IScheduler? _ui;

        public static IScheduler Background => DefaultScheduler.Instance;

        public static IScheduler UI => _ui ?? throw new InvalidOperationException("UI thread has not been initialized");

        public static IScheduler GetUIOrCurrent() => _ui ?? Background;

        public static void Initialize(IScheduler? uiScheduler) => _ui = uiScheduler;
    }
}
