// -----------------------------------------------------------------------
// <copyright file="Scheduler.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reactive.Concurrency;

namespace MyNet.UI.Threading;

public static class Scheduler
{
    private static IScheduler? _ui;

    public static IScheduler Background => DefaultScheduler.Instance;

    public static IScheduler Ui => _ui ?? throw new InvalidOperationException("UI thread has not been initialized");

    public static IScheduler UiOrCurrent => _ui ?? Background;

    public static void Initialize(IScheduler? uiScheduler) => _ui = uiScheduler;
}
