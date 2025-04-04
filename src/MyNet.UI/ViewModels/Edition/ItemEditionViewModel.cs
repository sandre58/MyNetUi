// -----------------------------------------------------------------------
// <copyright file="ItemEditionViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using MyNet.Observable.Attributes;
using MyNet.Utilities;
using PropertyChanged;

namespace MyNet.UI.ViewModels.Edition;

public abstract class ItemEditionViewModel<T> : EditionViewModel, IItemEditionViewModel<T>
    where T : IModifiable, INotifyPropertyChanged, ICloneable, ISettable, IDisposable
{
    private T? _originalItem;

    [SuppressMessage("ReSharper", "CollectionNeverUpdated.Global", Justification = "Use by children classses")]
    protected CompositeDisposable? ItemSubscriptions { get; private set; }

    public Subject<T?> ItemChanged { get; } = new();

    [DoNotCheckEquality]
    [CanSetIsModified]
    [CanBeValidated]
    public T? Item { get; private set; }

    public void SetOriginalItem(T? item) => _originalItem = item;

    #region Restore

    protected override void RefreshCore()
    {
        using (IsModifiedSuspender.Suspend())
        {
            var item = _originalItem is not null ? LoadItem(_originalItem) : CreateNewItem();
            Mode = _originalItem is null ? ScreenMode.Creation : ScreenMode.Edition;
            Item = item;
        }
    }

    protected abstract T CreateNewItem();

    protected virtual T? LoadItem(T? originalItem) => originalItem is not null ? originalItem.Clone<T>() : default;

    #endregion Restore

    #region Validate

    protected override void SaveCore()
    {
        _originalItem = SaveItem(Item);
        Mode = _originalItem is null ? ScreenMode.Creation : ScreenMode.Edition;

        if (_originalItem is null)
            return;
        Item = _originalItem.Clone<T>();
        UpdateTitle();
        Item?.ResetIsModified();
    }

    protected virtual T? SaveItem(T? item) => item;

    #endregion Validate

    #region PropertyChanged

    protected virtual void OnItemChanged()
    {
        using (IsModifiedSuspender.Suspend())
        {
            ItemSubscriptions?.Dispose();
            ItemSubscriptions = [];
            UpdateTitle();
            Item?.ResetIsModified();

            ItemChanged.OnNext(Item);
        }
    }

    #endregion PropertyChanged

    protected override void Cleanup()
    {
        base.Cleanup();

        ItemSubscriptions?.Dispose();
    }
}
