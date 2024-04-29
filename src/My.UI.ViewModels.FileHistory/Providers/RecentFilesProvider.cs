// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using DynamicData;
using DynamicData.Binding;
using MyNet.UI.ViewModels.FileHistory.Interfaces;
using MyNet.UI.ViewModels.FileHistory.Messages;
using MyNet.UI.ViewModels.FileHistory.Services;
using MyNet.UI.ViewModels.FileHistory.ViewModels;
using My.Utilities;
using My.Utilities.FileHistory;
using My.Utilities.Messaging;

namespace MyNet.UI.ViewModels.FileHistory.Providers
{
    public sealed class RecentFilesProvider : IDisposable
    {
        private readonly RecentFilesService _recentFilesService;
        private readonly RecentFilesManager _recentFilesManager;
        private readonly IRecentFileCommandsService _recentFileCommandsService;

        private readonly ObservableCollection<RecentFileViewModel> _source = [];
        private readonly CompositeDisposable _cleanup = [];
        private readonly IObservable<IChangeSet<RecentFileViewModel>> _observableItems;

        public RecentFilesProvider(RecentFilesService recentFilesService, RecentFilesManager recentFilesManager, IRecentFileCommandsService recentFileCommandsService)
        {
            _recentFileCommandsService = recentFileCommandsService;
            _recentFilesService = recentFilesService;
            _recentFilesManager = recentFilesManager;
            Items = new(_source);
            _observableItems = Items.ToObservableChangeSet();

            _cleanup.Add(Items.ToObservableChangeSet().DisposeMany().OnItemAdded(async x => await x.LoadImageAsync().ConfigureAwait(false)).Subscribe());

            Messenger.Default.Register<RecentFilesChangedMessage>(this, _ => Reload());
        }

        public ReadOnlyObservableCollection<RecentFileViewModel> Items { get; }

        public void Reload()
        {
            var recentFiles = _recentFilesService.GetAll();

            _source.UpdateFrom(recentFiles,
                x => _source.Add(new RecentFileViewModel(x, _recentFilesManager, _recentFileCommandsService)),
                x => _source.Remove(x),
                (destination, source) => destination.Update(source),
                (x, y) => x.Path == y.Path);
        }

        public IObservable<IChangeSet<RecentFileViewModel>> Connect() => _observableItems;

        public void Dispose()
        {
            Messenger.Default.Unregister(this);
            _cleanup.Dispose();
        }
    }
}
