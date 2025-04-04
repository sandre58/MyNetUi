// -----------------------------------------------------------------------
// <copyright file="ListViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using DynamicData;
using MyNet.Observable.Attributes;
using MyNet.Observable.Collections;
using MyNet.Observable.Collections.Providers;
using MyNet.UI.Threading;
using MyNet.Utilities.Providers;

namespace MyNet.UI.ViewModels.List;

[CanBeValidatedForDeclaredClassOnly(false)]
[CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
public class ListViewModel<T> : ListViewModelBase<T, ExtendedCollection<T>>
    where T : notnull
{
    public ListViewModel(ICollection<T> source, IListParametersProvider? parametersProvider = null)
        : base(new ExtendedCollection<T>(source, Scheduler.UiOrCurrent), parametersProvider) { }

    public ListViewModel(IItemsProvider<T> source, bool loadItems = true, IListParametersProvider? parametersProvider = null)
        : base(new ExtendedCollection<T>(source, loadItems, Scheduler.UiOrCurrent), parametersProvider) { }

    public ListViewModel(ISourceProvider<T> source, IListParametersProvider? parametersProvider = null)
        : base(new ExtendedCollection<T>(source, Scheduler.UiOrCurrent), parametersProvider) { }

    public ListViewModel(IObservable<IChangeSet<T>> source, IListParametersProvider? parametersProvider = null)
        : base(new ExtendedCollection<T>(source, Scheduler.UiOrCurrent), parametersProvider) { }

    public ListViewModel(IListParametersProvider? parametersProvider = null)
        : base(new ExtendedCollection<T>(Scheduler.UiOrCurrent), parametersProvider) { }
}
