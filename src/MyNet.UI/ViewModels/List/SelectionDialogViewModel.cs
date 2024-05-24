// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using MyNet.Observable.Attributes;
using MyNet.Observable.Collections.Providers;
using MyNet.UI.Commands;
using MyNet.UI.Resources;
using MyNet.UI.Selection;
using MyNet.UI.ViewModels.Dialogs;
using MyNet.Utilities.Providers;

namespace MyNet.UI.ViewModels.List
{
    [CanBeValidatedForDeclaredClassOnly(false)]
    [CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
    public class SelectionDialogViewModel<T> : ListDialogViewModelBase<T, SelectionListViewModel<T>>
        where T : notnull
    {
        public SelectionDialogViewModel(ICollection<T> itemsProvider,
                                        IListParametersProvider? parametersProvider = null,
                                        SelectionMode selectionMode = SelectionMode.Multiple,
                                        string? title = null)
            : this(new SelectionListViewModel<T>(parametersProvider, selectionMode), title) { }

        public SelectionDialogViewModel(IItemsProvider<T> itemsProvider,
                                        IListParametersProvider? parametersProvider = null,
                                        SelectionMode selectionMode = SelectionMode.Multiple,
                                        string? title = null)
            : this(new SelectionListViewModel<T>(parametersProvider, selectionMode), title) { }

        public SelectionDialogViewModel(ISourceProvider<T> itemsProvider,
                                        IListParametersProvider? parametersProvider = null,
                                        SelectionMode selectionMode = SelectionMode.Multiple,
                                        string? title = null)
            : this(new SelectionListViewModel<T>(parametersProvider, selectionMode), title) { }

        public SelectionDialogViewModel(IObservable<T> itemsProvider,
                                        IListParametersProvider? parametersProvider = null,
                                        SelectionMode selectionMode = SelectionMode.Multiple,
                                        string? title = null)
            : this(new SelectionListViewModel<T>(parametersProvider, selectionMode), title) { }

        protected SelectionDialogViewModel(SelectionListViewModel<T> selectionViewModel, string? title = null)
            : base(selectionViewModel)
        {
            DoubleClickCommand = CommandsManager.Create(Validate, () => List.SelectionMode == SelectionMode.Single && CanValidate());

            Title = title ?? (List.SelectionMode == SelectionMode.Multiple ? UiResources.SelectItems : UiResources.SelectItem);
        }

        public ICommand DoubleClickCommand { get; private set; }

        protected override bool CanValidate() => List.SelectedItems.Any();
    }
}
