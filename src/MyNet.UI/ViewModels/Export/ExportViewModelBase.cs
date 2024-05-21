// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNet.Humanizer;
using MyNet.UI.Commands;
using MyNet.UI.Resources;
using MyNet.UI.Toasting;
using MyNet.UI.Toasting.Settings;
using MyNet.UI.ViewModels.Workspace;
using MyNet.Utilities.Exceptions;

namespace MyNet.UI.ViewModels.Export
{
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
            Title = UiResources.ExportXItems.TranslateWithCountAndOptionalFormat(_items.Count());
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
                var errors = GetErrors().ToList();
                errors.ToList().ForEach(x => ToasterManager.ShowError(x, ToastClosingStrategy.AutoClose));
            }
        }

        protected abstract void SaveConfiguration();

        protected abstract Task<bool> ExportItemsAsync(IEnumerable<T> items);
    }
}
