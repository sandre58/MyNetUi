// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using MyNet.Observable.Attributes;
using MyNet.UI.Commands;
using MyNet.UI.Resources;
using MyNet.UI.Selection.Models;
using MyNet.UI.Toasting;
using MyNet.UI.Toasting.Settings;
using MyNet.UI.ViewModels.List;
using MyNet.UI.ViewModels.Selection;
using MyNet.Utilities;

namespace MyNet.UI.ViewModels.Import
{
    [CanBeValidatedForDeclaredClassOnly(false)]
    [CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
    public abstract class ImportViewModel<T> : ItemsSelectionViewModel<T>
        where T : notnull, ImportableViewModel
    {
        public ImportSelectSourceViewModel<T> SelectSourceViewModel { get; }

        public bool SourceHasBeenImported { get; set; }

        public int CountItemsToImport { get; private set; }

        public IEnumerable? SelectedRows { get; set; }

        public ICommand ImportCommand { get; }

        public ICommand DoNotImportCommand { get; }

        public override bool RefreshItemsOnLoading => false;

        protected ImportViewModel(ImportSelectSourceViewModel<T> selectSourceViewModel,
                                  IListParametersProvider? parametersProvider = null,
                                  string? title = null)
            : base(selectSourceViewModel, parametersProvider: parametersProvider, title: title ?? UiResources.Import)
        {
            SelectSourceViewModel = selectSourceViewModel;

            ImportCommand = CommandsManager.Create(() => SetValueInSelectedRows(y => y.Import = true));
            DoNotImportCommand = CommandsManager.Create(() => SetValueInSelectedRows(y => y.Import = false));

            SelectSourceViewModel.CurrentSourceChanged += OnCurrentSourceChangedAsync;

            Disposables.Add(Collection.ConnectSource().WhenPropertyChanged(x => x.Import).Subscribe(_ => CountItemsToImport = Source.Count(x => x.Import)));
        }

        private async void OnCurrentSourceChangedAsync(object? sender, EventArgs e)
        {
            await RefreshAsync().ConfigureAwait(false);
            SourceHasBeenImported = SelectSourceViewModel.CurrentSource is not null && (!SelectSourceViewModel.HasErrors || SelectSourceViewModel.IgnoreErrors);
        }

        protected override void ResetCore()
        {
            SelectSourceViewModel.Reset();
            SourceHasBeenImported = false;
        }

        protected override bool CanRefresh() => SourceHasBeenImported;

        protected override bool CanReset() => SourceHasBeenImported;

        protected override bool CanValidate() => SourceHasBeenImported && Items.Any(x => x.Import);

        protected override void Validate()
        {
            if (Items.Where(x => x.Import).Any(x => !x.ValidateProperties()))
            {
                GetErrors().ToList().ForEach(x => ToasterManager.ShowError(x, ToastClosingStrategy.AutoClose));
                return;
            }

            base.Validate();
            Reset();
        }

        public override void Load()
        {
            base.Load();

            Task.Run(SelectSourceViewModel.RefreshAsync);
        }

        protected virtual void SetValueInSelectedRows(Action<T> action) => SelectedRows?.Cast<SelectedWrapper<T>>().Select(x => x.Item).ForEach(action);

        protected override void Cleanup()
        {
            base.Cleanup();
            SelectSourceViewModel.CurrentSourceChanged -= OnCurrentSourceChangedAsync;
        }
    }
}
