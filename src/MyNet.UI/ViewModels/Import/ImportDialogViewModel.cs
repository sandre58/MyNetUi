// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using DynamicData;
using MyNet.Observable.Attributes;
using MyNet.Observable.Collections.Providers;
using MyNet.UI.Selection;
using MyNet.UI.ViewModels.List;
using MyNet.Utilities.Providers;

namespace MyNet.UI.ViewModels.Import
{
    [CanBeValidatedForDeclaredClassOnly(false)]
    [CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
    public class ImportDialogViewModel<T> : ImportDialogViewModelBase<T, ImportablesListViewModel<T>> where T : notnull, ImportableViewModel
    {
        public ImportDialogViewModel(ICollection<T> itemsProvider,
                                     IListParametersProvider? parametersProvider = null,
                                     SelectionMode selectionMode = SelectionMode.Multiple,
                                     string? title = null)
            : this(new ImportablesListViewModel<T>(itemsProvider, parametersProvider, selectionMode), title) { }

        public ImportDialogViewModel(IItemsProvider<T> itemsProvider,
                                     IListParametersProvider? parametersProvider = null,
                                     SelectionMode selectionMode = SelectionMode.Multiple,
                                     string? title = null)
            : this(new ImportablesListViewModel<T>(itemsProvider, parametersProvider, selectionMode), title) { }

        public ImportDialogViewModel(ISourceProvider<T> itemsProvider,
                                     IListParametersProvider? parametersProvider = null,
                                     SelectionMode selectionMode = SelectionMode.Multiple,
                                     string? title = null)
            : this(new ImportablesListViewModel<T>(itemsProvider, parametersProvider, selectionMode), title) { }

        public ImportDialogViewModel(IObservable<IChangeSet<T>> itemsProvider,
                                     IListParametersProvider? parametersProvider = null,
                                     SelectionMode selectionMode = SelectionMode.Multiple,
                                     string? title = null)
            : this(new ImportablesListViewModel<T>(itemsProvider, parametersProvider, selectionMode), title) { }

        protected ImportDialogViewModel(ImportablesListViewModel<T> list, string? title = null)
            : base(list, title) { }

    }
}
