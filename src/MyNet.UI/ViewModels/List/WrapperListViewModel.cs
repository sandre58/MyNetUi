// -----------------------------------------------------------------------
// <copyright file="WrapperListViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using DynamicData;
using MyNet.Observable.Attributes;
using MyNet.Observable.Collections;
using MyNet.Observable.Collections.Providers;
using MyNet.UI.Collections;
using MyNet.UI.Threading;
using MyNet.Utilities;
using MyNet.Utilities.Providers;

namespace MyNet.UI.ViewModels.List;

[CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
[CanBeValidatedForDeclaredClassOnly(false)]
public abstract class WrapperListViewModel<T, TWrapper, TCollection> : ListViewModelBase<T, TCollection>, IWrapperListViewModel<T, TWrapper>
    where TCollection : ExtendedWrapperCollection<T, TWrapper>
    where TWrapper : IWrapper<T>
    where T : notnull
{
    private readonly UiObservableCollection<TWrapper> _pagedWrappers = [];

    protected WrapperListViewModel(TCollection collection,
        IListParametersProvider? parametersProvider = null)
        : base(collection, parametersProvider) => PagedWrappers = new(_pagedWrappers);

    public ReadOnlyObservableCollection<TWrapper> Wrappers => Collection.Wrappers;

    public ReadOnlyObservableCollection<TWrapper> WrappersSource => Collection.WrappersSource;

    public ReadOnlyObservableCollection<TWrapper> PagedWrappers { get; }

    protected override IDisposable SubscribePager(IObservable<PageRequest> pager)
    {
        _pagedWrappers.Clear();
        return new CompositeDisposable(
            base.SubscribePager(pager),
            Collection.ConnectWrappers().Page(pager).Bind(_pagedWrappers).Subscribe());
    }
}

[CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
[CanBeValidatedForDeclaredClassOnly(false)]
public class WrapperListViewModel<T, TWrapper> : WrapperListViewModel<T, TWrapper, ExtendedWrapperCollection<T, TWrapper>>
    where TWrapper : IWrapper<T>
    where T : notnull
{
    public WrapperListViewModel(ICollection<T> source, Func<T, TWrapper> createWrapper, IListParametersProvider? parametersProvider = null)
        : base(new ExtendedWrapperCollection<T, TWrapper>(source, Scheduler.UiOrCurrent, createWrapper), parametersProvider) { }

    public WrapperListViewModel(IItemsProvider<T> source, Func<T, TWrapper> createWrapper, bool loadItems = true, IListParametersProvider? parametersProvider = null)
        : base(new ExtendedWrapperCollection<T, TWrapper>(source, loadItems, Scheduler.UiOrCurrent, createWrapper), parametersProvider) { }

    public WrapperListViewModel(ISourceProvider<T> source, Func<T, TWrapper> createWrapper, IListParametersProvider? parametersProvider = null)
        : base(new ExtendedWrapperCollection<T, TWrapper>(source, Scheduler.UiOrCurrent, createWrapper), parametersProvider) { }

    public WrapperListViewModel(IObservable<IChangeSet<T>> source, Func<T, TWrapper> createWrapper, IListParametersProvider? parametersProvider = null)
        : base(new ExtendedWrapperCollection<T, TWrapper>(source, Scheduler.UiOrCurrent, createWrapper), parametersProvider) { }

    public WrapperListViewModel(Func<T, TWrapper> createWrapper, IListParametersProvider? parametersProvider = null)
        : base(new ExtendedWrapperCollection<T, TWrapper>(Scheduler.UiOrCurrent, createWrapper), parametersProvider) { }
}
