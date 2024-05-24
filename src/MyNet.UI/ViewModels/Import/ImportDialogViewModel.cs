// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using MyNet.Observable.Attributes;
using MyNet.Observable.Collections.Providers;
using MyNet.UI.Resources;
using MyNet.UI.Selection;
using MyNet.UI.Toasting;
using MyNet.UI.Toasting.Settings;
using MyNet.UI.ViewModels.Dialogs;
using MyNet.UI.ViewModels.List;
using MyNet.Utilities.Providers;

namespace MyNet.UI.ViewModels.Import
{
    [CanBeValidatedForDeclaredClassOnly(false)]
    [CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
    public class ImportDialogViewModel<T> : ListDialogViewModelBase<T, ImportablesListViewModel<T>> where T : notnull, ImportableViewModel
    {
        public ImportDialogViewModel(ICollection<T> itemsProvider,
                                     IListParametersProvider? parametersProvider = null,
                                     SelectionMode selectionMode = SelectionMode.Multiple,
                                     string? title = null)
            : this(new ImportablesListViewModel<T>(parametersProvider, selectionMode), title) { }

        public ImportDialogViewModel(IItemsProvider<T> itemsProvider,
                                     IListParametersProvider? parametersProvider = null,
                                     SelectionMode selectionMode = SelectionMode.Multiple,
                                     string? title = null)
            : this(new ImportablesListViewModel<T>(parametersProvider, selectionMode), title) { }

        public ImportDialogViewModel(ISourceProvider<T> itemsProvider,
                                     IListParametersProvider? parametersProvider = null,
                                     SelectionMode selectionMode = SelectionMode.Multiple,
                                     string? title = null)
            : this(new ImportablesListViewModel<T>(parametersProvider, selectionMode), title) { }

        public ImportDialogViewModel(IObservable<T> itemsProvider,
                                     IListParametersProvider? parametersProvider = null,
                                     SelectionMode selectionMode = SelectionMode.Multiple,
                                     string? title = null)
            : this(new ImportablesListViewModel<T>(parametersProvider, selectionMode), title) { }

        protected ImportDialogViewModel(ImportablesListViewModel<T> list, string? title = null)
            : base(list) => Title = title ?? UiResources.Import;

        protected override bool CanValidate() => List.ImportItems.Any();

        protected override void Validate()
        {
            if (List.ImportItems.Any(x => !x.ValidateProperties()))
            {
                GetErrors().ToList().ForEach(x => ToasterManager.ShowError(x, ToastClosingStrategy.AutoClose));
                return;
            }

            base.Validate();
        }
    }
}
