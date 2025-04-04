// -----------------------------------------------------------------------
// <copyright file="ScheduleObservableCollection.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using DynamicData;
using DynamicData.Binding;
using MyNet.Utilities.Collections;

namespace MyNet.UI.Collections;

public class ScheduleObservableCollection<T> : ThreadSafeObservableCollection<T>, IObservableCollection<T>, IExtendedList<T>
{
    public ScheduleObservableCollection(Func<IScheduler> scheduler)
        : base(x => scheduler().Schedule(x)) { }

    public ScheduleObservableCollection(IList<T> list, Func<IScheduler> scheduler)
        : base(list, x => scheduler().Schedule(x)) { }

    public ScheduleObservableCollection(IEnumerable<T> collection, Func<IScheduler> scheduler)
        : base(collection, x => scheduler().Schedule(x)) { }
}
