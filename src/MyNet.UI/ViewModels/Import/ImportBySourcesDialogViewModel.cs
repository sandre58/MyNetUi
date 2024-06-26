﻿// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using MyNet.Observable.Attributes;
using MyNet.UI.Extensions;
using MyNet.UI.Selection;
using MyNet.UI.ViewModels.List;
using MyNet.Utilities;
using MyNet.Utilities.Exceptions;
using MyNet.Utilities.Logging;

namespace MyNet.UI.ViewModels.Import
{
    [CanBeValidatedForDeclaredClassOnly(false)]
    [CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
    public class ImportBySourcesDialogViewModel<T> : ImportBySourcesDialogViewModel<T, ImportablesListViewModel<T>>
        where T : notnull, ImportableViewModel
    {
        public ImportBySourcesDialogViewModel(ICollection<IImportSourceViewModel<T>> sources,
                                              IListParametersProvider? listParametersProvider = null,
                                              SelectionMode selectionMode = SelectionMode.Multiple,
                                              string? title = null)
            : this(new ItemsBySourceProvider<T>(sources), listParametersProvider, selectionMode, title) { }

        private ImportBySourcesDialogViewModel(ItemsBySourceProvider<T> itemsProvider,
                                               IListParametersProvider? listParametersProvider = null,
                                               SelectionMode selectionMode = SelectionMode.Multiple,
                                               string? title = null)
            : base(itemsProvider, new ImportablesListViewModel<T>(itemsProvider, listParametersProvider, selectionMode), title) { }
    }

    [CanBeValidatedForDeclaredClassOnly(false)]
    [CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
    public abstract class ImportBySourcesDialogViewModel<T, TListViewModel> : ImportDialogViewModel<T, TListViewModel>
        where T : notnull, ImportableViewModel
        where TListViewModel : ImportablesListViewModel<T>
    {
        private readonly ItemsBySourceProvider<T> _itemsProvider;

        protected ImportBySourcesDialogViewModel(ItemsBySourceProvider<T> itemsProvider,
                                                 TListViewModel list,
                                                 string? title = null)
            : base(list, title)
        {
            _itemsProvider = itemsProvider;

            Sources.ForEach(x => x.ItemsLoadingRequested += OnLoadingRequestedAsync);
        }

        public ICollection<IImportSourceViewModel<T>> Sources => _itemsProvider.Sources;

        public bool ShowList { get; private set; }

        private async void OnLoadingRequestedAsync(object? sender, EventArgs e)
            => await ExecuteAsync(() =>
            {
                try
                {
                    if (sender is not IImportSourceViewModel<T> source) return;

                    _itemsProvider.LoadSource(source);
                    ShowList = true;
                }
                catch (TranslatableException e)
                {
                    e.ShowInToaster(true, false);
                    ShowList = false;
                }
                catch (InvalidOperationException e)
                {
                    LogManager.Warning(e.Message);
                    ShowList = false;
                }
                catch (Exception e)
                {
                    e.ShowInToaster(true, false);
                    LogManager.Error(e);
                    ShowList = false;
                }
            }).ConfigureAwait(false);

        public override async void Load()
        {
            DialogResult = null;
            await Task.WhenAll(Sources.Select(x => x.InitializeAsync()).ToArray()).ConfigureAwait(false);
        }

        protected override bool CanRefresh() => ShowList;

        protected override void RefreshCore() => _itemsProvider.Reload();

        protected override bool CanReset() => ShowList;

        protected override void ResetCore()
        {
            _itemsProvider.Clear();
            ShowList = false;
        }

        protected override bool CanValidate() => ShowList && base.CanValidate();
    }
}
