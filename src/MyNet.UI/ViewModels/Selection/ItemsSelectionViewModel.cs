// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MyNet.Observable.Attributes;
using MyNet.UI.Commands;
using MyNet.UI.Dialogs.Models;
using MyNet.UI.Resources;
using MyNet.UI.Selection;
using MyNet.UI.ViewModels.List;
using MyNet.Utilities;
using MyNet.Utilities.Providers;

namespace MyNet.UI.ViewModels.Selection
{
    [CanBeValidatedForDeclaredClassOnly(false)]
    [CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
    public abstract class ItemsSelectionViewModel<T> : SelectionListViewModel<T>, IDialogViewModel
        where T : notnull
    {
        private readonly IItemsProvider<T> _itemsProvider;

        protected ItemsSelectionViewModel(
            IEnumerable<T> items,
            IListParametersProvider? parametersProvider = null,
            SelectionMode? selectionMode = null,
            string? title = null)
            : this(new ItemsProvider<T>(items), parametersProvider, selectionMode, title) { }

        protected ItemsSelectionViewModel(
            IItemsProvider<T> itemsProvider,
            IListParametersProvider? parametersProvider = null,
            SelectionMode? selectionMode = null,
            string? title = null)
            : base(parametersProvider, selectionMode)
        {
            _itemsProvider = itemsProvider;
            CancelCommand = CommandsManager.Create(Cancel, CanCancel);
            ValidateCommand = CommandsManager.Create(Validate, CanValidate);
            DoubleClickCommand = CommandsManager.Create(Validate, () => Collection.SelectionMode == SelectionMode.Single && CanValidate());

            Title = title ?? (selectionMode == SelectionMode.Multiple ? UiResources.SelectItems : UiResources.SelectItem);
        }

        public event CancelEventHandler? CloseRequest;

        public bool? DialogResult { get; set; }

        public virtual bool LoadWhenDialogOpening => true;

        public virtual bool RefreshItemsOnLoading => true;

        public ICommand DoubleClickCommand { get; private set; }

        public ICommand ValidateCommand { get; private set; }

        public ICommand CancelCommand { get; private set; }

        protected virtual void Cancel() => Close(false);

        protected virtual void Validate() => Close(true);

        protected virtual bool CanValidate() => SelectedItems.Any();

        protected virtual bool CanCancel() => true;

        public virtual void Close(bool? dialogResult)
        {
            if (dialogResult != null)
                DialogResult = dialogResult.Value;

            var e = new CancelEventArgs();
            OnCloseRequest(e);

            CloseRequest?.Invoke(this, e);
        }

        public virtual void Close() => Close(null);

        protected virtual void OnCloseRequest(CancelEventArgs e)
        {
            // Method intentionally left empty.
        }

        public virtual Task<bool> CanCloseAsync() => Task.FromResult(true);

        public virtual void Load()
        {
            DialogResult = null;

            if (RefreshItemsOnLoading)
                Task.Run(RefreshAsync);
        }

        protected override void RefreshCore() => Collection.Set(_itemsProvider.ProvideItems());
    }
}
