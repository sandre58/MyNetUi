// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MyNet.UI.Commands;
using MyNet.UI.Dialogs;
using MyNet.UI.Extensions;
using MyNet.UI.Locators;
using MyNet.UI.ViewModels.Edition;
using MyNet.Utilities;
using PropertyChanged;

namespace MyNet.UI.ViewModels.Workspace
{
    public abstract class ItemViewModel<TModel, TEditViewModel> : ItemViewModel<TModel>
    where TModel : IModifiable, INotifyPropertyChanged, ICloneable, ISettable, IDisposable
    where TEditViewModel : ItemEditionViewModel<TModel>
    {
        public ICommand EditCommand { get; private set; }

        private readonly IViewModelLocator _viewModelLocator;

        protected ItemViewModel(IViewModelLocator viewModelLocator)
        {
            _viewModelLocator = viewModelLocator;

            EditCommand = CommandsManager.Create(async () => await EditAsync().ConfigureAwait(false), CanEdit);
        }

        #region Edit

        protected virtual async Task EditAsync()
        {
            var vm = GetEditViewModel();

            if (vm is null) return;

            OnEditRequested(vm);
            var result = await DialogManager.ShowDialogAsync(vm).ConfigureAwait(false);

            if (result.Value)
                OnEditCompleted(vm);
        }

        protected virtual bool CanEdit() => Item is not null;

        protected virtual void OnEditRequested(TEditViewModel? editViewModel)
        {
        }

        protected virtual void OnEditCompleted(TEditViewModel? editViewModel)
        {
        }

        protected virtual TEditViewModel? GetEditViewModel()
        {
            var vm = _viewModelLocator.Get<TEditViewModel>();
            vm?.SetOriginalItem(Item);

            return vm;
        }

        #endregion Edit
    }

    public abstract class ItemViewModel<TModel> : NavigableWorkspaceViewModel, IItemViewModel<TModel>
        where TModel : INotifyPropertyChanged
    {
        protected CompositeDisposable? ItemSubscriptions { get; private set; }

        public Subject<TModel?> ItemChanged { get; } = new();

        [DoNotCheckEquality]
        public TModel? Item { get; private set; }

        protected virtual TModel? LoadItem(TModel? originalItem) => originalItem;

        public virtual void SetItem(TModel? item) => Item = LoadItem(item);

        protected override void RefreshCore() => SetItem(Item);

        protected virtual void OnItemChanged()
        {
            ItemSubscriptions?.Dispose();
            ItemSubscriptions = [];

            ItemChanged.OnNext(Item);
        }

        public override bool Equals(object? obj) => obj is ItemViewModel<TModel> itemViewModel && Equals(Item, itemViewModel.Item);

        public override int GetHashCode() => Item?.GetHashCode() ?? RuntimeHelpers.GetHashCode(this);

        protected override void Cleanup()
        {
            base.Cleanup();

            ItemSubscriptions?.Dispose();
        }

    }
}
