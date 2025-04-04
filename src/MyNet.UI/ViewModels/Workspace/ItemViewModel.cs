// -----------------------------------------------------------------------
// <copyright file="ItemViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using MyNet.UI.Commands;
using MyNet.UI.Dialogs;
using MyNet.UI.Dialogs.Models;
using MyNet.UI.Extensions;
using MyNet.UI.Locators;
using MyNet.UI.ViewModels.Edition;
using MyNet.Utilities;
using PropertyChanged;

namespace MyNet.UI.ViewModels.Workspace;

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

        OnEditRequested(vm);
        var result = await DialogManager.ShowDialogAsync(vm).ConfigureAwait(false);

        if (result.IsTrue())
            OnEditCompleted(vm);
    }

    protected virtual bool CanEdit() => Item is not null;

    [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "Used by children classes")]
    protected virtual void OnEditRequested(TEditViewModel? editViewModel)
    {
    }

    [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "Used by children classes")]
    protected virtual void OnEditCompleted(TEditViewModel? editViewModel)
    {
    }

    protected virtual TEditViewModel GetEditViewModel()
    {
        var vm = _viewModelLocator.Get<TEditViewModel>();
        vm.SetOriginalItem(Item);

        return vm;
    }

    #endregion Edit
}

public abstract class ItemDialogViewModel<TModel> : ItemViewModel<TModel>, IDialogViewModel
    where TModel : INotifyPropertyChanged
{
    #region Members

    /// <inheritdoc />
    /// <summary>
    /// Gets or sets the result.
    /// </summary>
    public bool? DialogResult { get; set; }

    public virtual bool LoadWhenDialogOpening => true;

    /// <summary>
    /// Gets close command.
    /// </summary>
    public ICommand CloseCommand { get; private set; }

    #endregion Members

    #region Constructors

    protected ItemDialogViewModel() => CloseCommand = CommandsManager.Create<bool?>(Close, CanClose);

    #endregion

    #region Methods

    /// <summary>
    /// Can Close ?.
    /// </summary>
    protected virtual bool CanClose(bool? dialogResult) => true;

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    public virtual void Close(bool? dialogResult)
    {
        if (dialogResult is not null)
            DialogResult = dialogResult.Value;

        var e = new CancelEventArgs();
        OnCloseRequest(e);

        CloseRequest?.Invoke(this, e);
    }

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    public virtual void Close() => Close(null);

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "Used by children classes")]
    protected virtual void OnCloseRequest(CancelEventArgs e)
    {
        // Method intentionally left empty.
    }

    public virtual Task<bool> CanCloseAsync() => Task.FromResult(true);

    public virtual void Load()
    {
        DialogResult = null;

        Refresh();
    }

    #endregion Methods

    #region Events

    /// <inheritdoc />
    /// <summary>
    /// Closes the dialog.
    /// </summary>
    public event CancelEventHandler? CloseRequest;

    #endregion Events
}

public abstract class ItemViewModel<TModel> : NavigableWorkspaceViewModel, IItemViewModel<TModel>
    where TModel : INotifyPropertyChanged
{
    [SuppressMessage("ReSharper", "CollectionNeverUpdated.Global", Justification = "Use by children classes")]
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
