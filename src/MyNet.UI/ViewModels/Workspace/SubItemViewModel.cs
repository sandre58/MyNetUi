// -----------------------------------------------------------------------
// <copyright file="SubItemViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using MyNet.Observable.Attributes;
using PropertyChanged;

namespace MyNet.UI.ViewModels.Workspace;

[CanBeValidatedForDeclaredClassOnly(false)]
[CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
public class SubItemViewModel<T> : NavigableWorkspaceViewModel, IItemViewModel<T>
{
    [SuppressMessage("ReSharper", "CollectionNeverUpdated.Global", Justification = "Use by children classes")]
    protected CompositeDisposable? ItemSubscriptions { get; private set; }

    [DoNotCheckEquality]
    public T? Item { get; private set; }

    public Subject<T?> ItemChanged { get; } = new();

    protected virtual void OnItemChanged()
    {
        ItemSubscriptions?.Dispose();
        ItemSubscriptions = [];

        ItemChanged.OnNext(Item);
    }

    public override void SetParentPage(IWorkspaceViewModel parentPage)
    {
        base.SetParentPage(parentPage);

        if (parentPage is IItemViewModel<T> itemViewModelParent)
            Disposables.Add(itemViewModelParent.ItemChanged.Subscribe(x => Item = x));
    }

    protected override void Cleanup()
    {
        base.Cleanup();

        ItemSubscriptions?.Dispose();
    }
}
