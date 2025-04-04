// -----------------------------------------------------------------------
// <copyright file="GroupingViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using DynamicData.Binding;
using DynamicData.Kernel;
using MyNet.DynamicData.Extensions;
using MyNet.Observable;
using MyNet.Observable.Attributes;
using MyNet.UI.Commands;
using MyNet.Utilities;
using MyNet.Utilities.Deferring;
using PropertyChanged;

namespace MyNet.UI.ViewModels.List.Grouping;

[CanBeValidated(false)]
[CanSetIsModified(false)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "It's a viewModel")]
public class GroupingViewModel : EditableObject, IGroupingViewModel, ICollection<IGroupingPropertyViewModel>, INotifyCollectionChanged
{
    private readonly IReadOnlyCollection<string> _defaultGroupingProperties;
    private readonly Deferrer _groupingChangedDeferrer;

    protected GroupingPropertiesCollection GroupingProperties { get; } = [];

    public IGroupingPropertyViewModel? ActiveGroupingProperty => GroupingProperties.OrderBy(x => x.Order).FirstOrDefault(x => x.IsEnabled);

    public int ActiveCount => GroupingProperties.Count(x => x.IsEnabled);

    public ICommand AddCommand { get; }

    public ICommand ApplyCommand { get; }

    public ICommand RemoveCommand { get; }

    public ICommand ResetCommand { get; }

    public ICommand ClearCommand { get; }

    public event EventHandler<GroupingChangedEventArgs>? GroupingChanged;

    public GroupingViewModel()
        : this([]) { }

    public GroupingViewModel(IEnumerable<string> defaultProperties)
    {
        _groupingChangedDeferrer = new Deferrer(OnSortChanged);
        _defaultGroupingProperties = defaultProperties.AsList().AsReadOnly();

        ClearCommand = CommandsManager.Create(Clear);
        AddCommand = CommandsManager.CreateNotNull<string>(x => Add(x));
        ApplyCommand = CommandsManager.CreateNotNull<string>(Set);
        RemoveCommand = CommandsManager.CreateNotNull<string>(Remove);
        ResetCommand = CommandsManager.Create(Reset);

        Reset();

        Disposables.Add(GroupingProperties.ToObservableChangeSet(x => x.PropertyName).SubscribeAll(() => _groupingChangedDeferrer.DeferOrExecute()));

        GroupingProperties.CollectionChanged += HandleCollectionChanged;
    }

    protected IDisposable DeferChanged() => _groupingChangedDeferrer.Defer();

    protected virtual IGroupingPropertyViewModel CreateGroupingProperty(string propertyName, string? sortingPropertyName = null, int? order = null)
        => new GroupingPropertyViewModel(propertyName, propertyName, sortingPropertyName, order ?? ActiveCount + 1) { IsEnabled = true };

    public virtual void Add(string propertyName, string? sortingPropertyName = null, int? order = null) => GroupingProperties.TryAdd(CreateGroupingProperty(propertyName, sortingPropertyName, order));

    public virtual void Remove(string propertyName) => GroupingProperties.Remove(propertyName);

    public void Set(string propertyName)
    {
        using (_groupingChangedDeferrer.Defer())
        {
            Clear();
            Add(propertyName);
        }
    }

    public virtual void Set(IEnumerable<IGroupingPropertyViewModel> properties)
    {
        using (_groupingChangedDeferrer.Defer())
            GroupingProperties.Set(properties);
    }

    public virtual void Clear() => GroupingProperties.Clear();

    public void Reset() => Set(_defaultGroupingProperties.Select((x, index) => CreateGroupingProperty(x, order: index + 1)));

    [SuppressPropertyChangedWarnings]
    protected virtual void OnSortChanged()
    {
        OnPropertyChanged(nameof(Count));
        OnPropertyChanged(nameof(ActiveCount));
        OnPropertyChanged(nameof(ActiveGroupingProperty));
        GroupingChanged?.Invoke(this, new GroupingChangedEventArgs(GroupingProperties));
    }

    #region ICollection

    public int Count => GroupingProperties.Count;

    public virtual bool IsReadOnly => false;

    public virtual void Add(IGroupingPropertyViewModel item) => IsReadOnly.IfFalse(() => GroupingProperties.Add(item));

    public virtual bool Remove(IGroupingPropertyViewModel item) => !IsReadOnly && GroupingProperties.Remove(item);

    public bool Contains(IGroupingPropertyViewModel item) => GroupingProperties.Contains(item);

    public void CopyTo(IGroupingPropertyViewModel[] array, int arrayIndex) => GroupingProperties.CopyTo(array, arrayIndex);

    public IEnumerator<IGroupingPropertyViewModel> GetEnumerator() => GroupingProperties.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GroupingProperties.GetEnumerator();

    #endregion

    #region INotifyCollectionChanged

    event NotifyCollectionChangedEventHandler? INotifyCollectionChanged.CollectionChanged
    {
        add => CollectionChanged += value;
        remove => CollectionChanged -= value;
    }

    protected event NotifyCollectionChangedEventHandler? CollectionChanged;

    [SuppressPropertyChangedWarnings]
    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args) => CollectionChanged?.Invoke(this, args);

    private void HandleCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => OnCollectionChanged(e);

    #endregion INotifyCollectionChanged

    protected override void Cleanup()
    {
        GroupingProperties.CollectionChanged -= HandleCollectionChanged;
        base.Cleanup();
    }
}
