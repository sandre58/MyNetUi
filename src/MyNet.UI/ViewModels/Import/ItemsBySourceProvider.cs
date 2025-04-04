// -----------------------------------------------------------------------
// <copyright file="ItemsBySourceProvider.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Binding;
using MyNet.Observable.Collections.Providers;

namespace MyNet.UI.ViewModels.Import;

public class ItemsBySourceProvider<T> : ISourceProvider<T>
    where T : ImportableViewModel
{
    private readonly ObservableCollectionExtended<T> _items = [];
    private readonly IObservable<IChangeSet<T>> _observable;
    private IImportSourceViewModel<T>? _lastSourceLoaded;

    public ItemsBySourceProvider(ICollection<IImportSourceViewModel<T>> sources)
    {
        Sources = sources;
        Source = new(_items);
        _observable = Source.ToObservableChangeSet();
    }

    public ICollection<IImportSourceViewModel<T>> Sources { get; }

    public ReadOnlyObservableCollection<T> Source { get; }

    public IObservable<IChangeSet<T>> Connect() => _observable;

    public void LoadSource(IImportSourceViewModel<T> source)
    {
        _lastSourceLoaded = source;
        var items = source.ProvideItems();
        SetSource(items);
    }

    public void Reload() => _lastSourceLoaded?.Reload();

    public void Clear() => _items.Clear();

    private void SetSource(IEnumerable<T> items) => _items.Load(items);
}
