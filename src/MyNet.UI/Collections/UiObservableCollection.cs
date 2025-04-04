// -----------------------------------------------------------------------
// <copyright file="UiObservableCollection.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace MyNet.UI.Collections;

public class UiObservableCollection<T> : ScheduleObservableCollection<T>
{
    public UiObservableCollection()
        : base(() => Threading.Scheduler.UiOrCurrent) { }

    public UiObservableCollection(IList<T> list)
        : base(list, () => Threading.Scheduler.UiOrCurrent) { }

    public UiObservableCollection(IEnumerable<T> collection)
        : base(collection, () => Threading.Scheduler.UiOrCurrent) { }
}
