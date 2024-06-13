// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Reactive.Concurrency;
using DynamicData.Binding;
using DynamicData;
using MyNet.Utilities.Collections;

namespace MyNet.UI.Collections
{
    public class ScheduleObservableCollection<T> : ThreadSafeObservableCollection<T>, IObservableCollection<T>, IExtendedList<T>
    {
        public ScheduleObservableCollection(IScheduler scheduler) : base(x => scheduler.Schedule(x)) { }

        public ScheduleObservableCollection(List<T> list, IScheduler scheduler) : base(list, x => scheduler.Schedule(x)) { }

        public ScheduleObservableCollection(IEnumerable<T> collection, IScheduler scheduler) : base(collection, x => scheduler.Schedule(x)) { }
    }
}
