// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Binding;
using MyNet.Observable.Collections.Providers;

namespace MyNet.UI.ViewModels.Import
{
    public class ItemsBySourceProvider<T> : ISourceProvider<T> where T : notnull, ImportableViewModel
    {
        private IImportSourceViewModel<T>? _lastSourceLoaded;
        private readonly ICollection<IImportSourceViewModel<T>> _sources;
        private readonly ObservableCollectionExtended<T> _items = [];
        private readonly IObservable<IChangeSet<T>> _observable;

        public ItemsBySourceProvider(ICollection<IImportSourceViewModel<T>> sources)
        {
            _sources = sources;
            Source = new(_items);
            _observable = Source.ToObservableChangeSet();
        }

        public ICollection<IImportSourceViewModel<T>> Sources => _sources;

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
}
