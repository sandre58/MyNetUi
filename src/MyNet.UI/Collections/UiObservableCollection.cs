// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace MyNet.UI.Collections
{
    public class UiObservableCollection<T> : ScheduleObservableCollection<T>
    {
        public UiObservableCollection() : base(Threading.Scheduler.GetUIOrCurrent) { }

        public UiObservableCollection(List<T> list) : base(list, Threading.Scheduler.GetUIOrCurrent) { }

        public UiObservableCollection(IEnumerable<T> collection) : base(collection, Threading.Scheduler.GetUIOrCurrent) { }
    }
}
