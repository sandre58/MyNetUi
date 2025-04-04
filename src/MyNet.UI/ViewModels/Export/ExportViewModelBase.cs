// -----------------------------------------------------------------------
// <copyright file="ExportViewModelBase.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MyNet.Humanizer;
using MyNet.UI.Commands;
using MyNet.UI.Resources;
using MyNet.UI.Toasting;
using MyNet.UI.ViewModels.Workspace;
using MyNet.Utilities.Exceptions;

namespace MyNet.UI.ViewModels.Export;

public abstract class ExportViewModelBase<T> : WorkspaceDialogViewModel
{
    private IEnumerable<T>? _items;

    public bool SaveConfigurationOnValidate { get; set; } = true;

    public ICommand ExportAndCloseCommand { get; private set; }

    public ICommand CancelCommand { get; private set; }

    protected ExportViewModelBase()
    {
        ExportAndCloseCommand = CommandsManager.Create(async () => await ExportAndCloseAsync().ConfigureAwait(false));
        CancelCommand = CommandsManager.Create(() => Close(false));
    }

    public virtual void Load(IEnumerable<T> items)
    {
        _items = items;
        Title = nameof(UiResources.ExportXItems).TranslateAndFormatWithCount(_items.Count());
    }

    protected virtual async Task ExportAndCloseAsync()
    {
        if (_items is null || !_items.Any())
            throw new TranslatableException(UiResources.ExportNoItemsError);

        if (ValidateProperties())
        {
            if (SaveConfigurationOnValidate)
                SaveConfiguration();

            if (await ExportItemsAsync(_items).ConfigureAwait(false))
                Close(true);
        }
        else
        {
            var errors = GetValidationErrors().ToList();
            errors.ToList().ForEach(x => ToasterManager.ShowError(x));
        }
    }

    protected virtual void SaveConfiguration() { }

    protected virtual void LoadConfiguration() { }

    protected abstract Task<bool> ExportItemsAsync(IEnumerable<T> items);
}
