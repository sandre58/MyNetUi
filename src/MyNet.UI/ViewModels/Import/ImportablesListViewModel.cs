// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using MyNet.Observable.Attributes;
using MyNet.Observable.Collections.Providers;
using MyNet.Observable.Threading;
using MyNet.UI.Commands;
using MyNet.UI.Selection;
using MyNet.UI.ViewModels.List;
using MyNet.Utilities.Providers;

namespace MyNet.UI.ViewModels.Import
{
    [CanBeValidatedForDeclaredClassOnly(false)]
    [CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
    public class ImportablesListViewModel<T> : SelectionListViewModel<T> where T : notnull, ImportableViewModel
    {
        private readonly ReadOnlyObservableCollection<T> _importItems;

        public ImportablesListViewModel(ICollection<T> source, IListParametersProvider? parametersProvider = null, SelectionMode selectionMode = SelectionMode.Multiple)
            : this(new SelectableCollection<T>(source, selectionMode: selectionMode, scheduler: Scheduler.GetUIOrCurrent()), parametersProvider) { }

        public ImportablesListViewModel(IItemsProvider<T> source, bool loadItems = true, IListParametersProvider? parametersProvider = null, SelectionMode selectionMode = SelectionMode.Multiple)
            : this(new SelectableCollection<T>(source, loadItems, selectionMode: selectionMode, scheduler: Scheduler.GetUIOrCurrent()), parametersProvider) { }

        public ImportablesListViewModel(ISourceProvider<T> source, IListParametersProvider? parametersProvider = null, SelectionMode selectionMode = SelectionMode.Multiple)
            : this(new SelectableCollection<T>(source, selectionMode: selectionMode, scheduler: Scheduler.GetUIOrCurrent()), parametersProvider) { }

        public ImportablesListViewModel(IObservable<IChangeSet<T>> source, IListParametersProvider? parametersProvider = null, SelectionMode selectionMode = SelectionMode.Multiple)
            : this(new SelectableCollection<T>(source, selectionMode: selectionMode, scheduler: Scheduler.GetUIOrCurrent()), parametersProvider) { }

        public ImportablesListViewModel(IListParametersProvider? parametersProvider = null, SelectionMode selectionMode = SelectionMode.Multiple)
            : this(new SelectableCollection<T>(selectionMode: selectionMode, scheduler: Scheduler.GetUIOrCurrent()), parametersProvider) { }

        protected ImportablesListViewModel(
            SelectableCollection<T> collection,
            IListParametersProvider? parametersProvider = null) : base(collection, parametersProvider)
        {
            ImportSelectionCommand = CommandsManager.Create(() => ApplyOnSelection(y => y.Import = true));
            DoNotImportSelectionCommand = CommandsManager.Create(() => ApplyOnSelection(y => y.Import = false));

            Disposables.Add(Collection.ConnectSource().AutoRefresh(x => x.Import).Filter(x => x.Import).Bind(out _importItems).Subscribe());
        }

        public ReadOnlyObservableCollection<T> ImportItems => _importItems;

        public ICommand ImportSelectionCommand { get; }

        public ICommand DoNotImportSelectionCommand { get; }
    }
}
