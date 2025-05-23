﻿// -----------------------------------------------------------------------
// <copyright file="SelectionDialogViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DynamicData;
using MyNet.Observable.Attributes;
using MyNet.Observable.Collections.Providers;
using MyNet.UI.Commands;
using MyNet.UI.Resources;
using MyNet.UI.Selection;
using MyNet.Utilities.Providers;

namespace MyNet.UI.ViewModels.List;

[CanBeValidatedForDeclaredClassOnly(false)]
[CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
public class SelectionDialogViewModel<T> : SelectionDialogViewModel<T, SelectionListViewModel<T>>
    where T : notnull
{
    public SelectionDialogViewModel(ICollection<T> itemsProvider,
        IListParametersProvider? parametersProvider = null,
        SelectionMode selectionMode = SelectionMode.Multiple,
        string? title = null)
        : this(new SelectionListViewModel<T>(itemsProvider, parametersProvider, selectionMode), title) { }

    public SelectionDialogViewModel(IItemsProvider<T> itemsProvider,
        bool loadItems = true,
        IListParametersProvider? parametersProvider = null,
        SelectionMode selectionMode = SelectionMode.Multiple,
        string? title = null)
        : this(new SelectionListViewModel<T>(itemsProvider, loadItems, parametersProvider, selectionMode), title) { }

    public SelectionDialogViewModel(ISourceProvider<T> itemsProvider,
        IListParametersProvider? parametersProvider = null,
        SelectionMode selectionMode = SelectionMode.Multiple,
        string? title = null)
        : this(new SelectionListViewModel<T>(itemsProvider, parametersProvider, selectionMode), title) { }

    public SelectionDialogViewModel(IObservable<IChangeSet<T>> itemsProvider,
        IListParametersProvider? parametersProvider = null,
        SelectionMode selectionMode = SelectionMode.Multiple,
        string? title = null)
        : this(new SelectionListViewModel<T>(itemsProvider, parametersProvider, selectionMode), title) { }

    protected SelectionDialogViewModel(SelectionListViewModel<T> selectionViewModel, string? title = null)
        : base(selectionViewModel, title) { }
}

[CanBeValidatedForDeclaredClassOnly(false)]
[CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
public abstract class SelectionDialogViewModel<T, TListViewModel> : ListDialogViewModelBase<T, TListViewModel>
    where T : notnull
    where TListViewModel : SelectionListViewModel<T>
{
    protected SelectionDialogViewModel(TListViewModel list, string? title = null)
        : base(list)
    {
        DoubleClickCommand = CommandsManager.Create(Validate, () => List.SelectionMode == SelectionMode.Single && CanValidate());

        Title = title ?? (List.SelectionMode == SelectionMode.Multiple ? UiResources.SelectItems : UiResources.SelectItem);
    }

    public ICommand DoubleClickCommand { get; private set; }

    protected override bool CanValidate() => List.SelectedItems.Any();
}
