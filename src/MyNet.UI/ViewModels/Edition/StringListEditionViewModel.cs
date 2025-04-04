// -----------------------------------------------------------------------
// <copyright file="StringListEditionViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DynamicData.Binding;
using MyNet.DynamicData.Extensions;
using MyNet.Observable;
using MyNet.Observable.Attributes;
using MyNet.UI.Commands;
using MyNet.UI.ViewModels.Workspace;
using MyNet.Utilities;

namespace MyNet.UI.ViewModels.Edition;

public class StringListEditionViewModel : WorkspaceViewModel
{
    public ICommand AddCommand { get; set; }

    public ICommand RemoveCommand { get; set; }

    public int Count => Items.Count(x => !string.IsNullOrEmpty(x.Value));

    [CanBeValidated]
    [CanSetIsModified]
    public ObservableCollection<StringWrapper> Items { get; } = [];

    public StringListEditionViewModel(string? title = null)
    {
        AddCommand = CommandsManager.CreateNotNull<StringWrapper>(_ => Add(), CanAdd);
        RemoveCommand = CommandsManager.CreateNotNull<StringWrapper>(Remove, CanRemove);

        Title = title;

        Disposables.Add(Items.ToObservableChangeSet().SubscribeAll(() => OnPropertyChanged(nameof(Count))));
    }

    public void SetSource(ObservableCollection<string> source)
    {
        Items.Set(source.Select(x => new StringWrapper { Value = x }));
        if (!Items.Any()) Add();
    }

    private void Add() => Items.Add(new());

    private bool CanAdd(StringWrapper item) => Items.IndexOf(item) == Items.Count - 1 && !string.IsNullOrEmpty(item.Value);

    private void Remove(StringWrapper item)
    {
        _ = Items.Remove(item);
        if (!Items.Any()) Add();
    }

    private bool CanRemove(StringWrapper item) => Items.Count > 1 || !string.IsNullOrEmpty(item.Value);
}

public class StringWrapper : ObservableObject
{
    public string? Value { get; set; }
}
