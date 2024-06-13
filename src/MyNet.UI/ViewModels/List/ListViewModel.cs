// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using DynamicData;
using MyNet.Observable.Attributes;
using MyNet.Observable.Collections;
using MyNet.Observable.Collections.Providers;
using MyNet.UI.Threading;
using MyNet.Utilities.Providers;

namespace MyNet.UI.ViewModels.List
{
    [CanBeValidatedForDeclaredClassOnly(false)]
    [CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
    public class ListViewModel<T> : ListViewModelBase<T, ExtendedCollection<T>>
        where T : notnull
    {
        public ListViewModel(ICollection<T> source, IListParametersProvider? parametersProvider = null)
            : base(new ExtendedCollection<T>(source, Scheduler.GetUIOrCurrent()), parametersProvider) { }

        public ListViewModel(IItemsProvider<T> source, bool loadItems = true, IListParametersProvider? parametersProvider = null)
            : base(new ExtendedCollection<T>(source, loadItems, Scheduler.GetUIOrCurrent()), parametersProvider) { }

        public ListViewModel(ISourceProvider<T> source, IListParametersProvider? parametersProvider = null)
            : base(new ExtendedCollection<T>(source, Scheduler.GetUIOrCurrent()), parametersProvider) { }

        public ListViewModel(IObservable<IChangeSet<T>> source, IListParametersProvider? parametersProvider = null)
            : base(new ExtendedCollection<T>(source, Scheduler.GetUIOrCurrent()), parametersProvider) { }

        public ListViewModel(IListParametersProvider? parametersProvider = null)
            : base(new ExtendedCollection<T>(Scheduler.GetUIOrCurrent()), parametersProvider) { }
    }
}
