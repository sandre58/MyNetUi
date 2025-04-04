// -----------------------------------------------------------------------
// <copyright file="ImportBySourcesDialogViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNet.Observable.Attributes;
using MyNet.UI.Extensions;
using MyNet.UI.Selection;
using MyNet.UI.ViewModels.List;
using MyNet.Utilities;
using MyNet.Utilities.Exceptions;
using MyNet.Utilities.Logging;

namespace MyNet.UI.ViewModels.Import;

[CanBeValidatedForDeclaredClassOnly(false)]
[CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
public class ImportBySourcesDialogViewModel<T> : ImportBySourcesDialogViewModel<T, ImportablesListViewModel<T>>
    where T : ImportableViewModel
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
    where T : ImportableViewModel
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
            catch (TranslatableException ex)
            {
                ex.ShowInToaster(true, false);
                ShowList = false;
            }
            catch (InvalidOperationException ex)
            {
                LogManager.Warning(ex.Message);
                ShowList = false;
            }
            catch (Exception ex)
            {
                ex.ShowInToaster(true, false);
                LogManager.Error(ex);
                ShowList = false;
            }
        }).ConfigureAwait(false);

    public override async void Load()
    {
        DialogResult = null;
        await Task.WhenAll([.. Sources.Select(x => x.InitializeAsync())]).ConfigureAwait(false);
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
