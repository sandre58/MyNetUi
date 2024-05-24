// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Linq;
using MyNet.Observable.Attributes;
using MyNet.UI.Resources;
using MyNet.UI.Toasting;
using MyNet.UI.Toasting.Settings;
using MyNet.UI.ViewModels.Dialogs;

namespace MyNet.UI.ViewModels.Import
{
    [CanBeValidatedForDeclaredClassOnly(false)]
    [CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
    public abstract class ImportDialogViewModelBase<T, TListViewModel> : ListDialogViewModelBase<T, TListViewModel>
        where T : notnull, ImportableViewModel
        where TListViewModel : ImportablesListViewModel<T>
    {
        protected ImportDialogViewModelBase(TListViewModel list, string? title = null)
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
