// -----------------------------------------------------------------------
// <copyright file="SelectedWrapper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Observable;
using MyNet.Observable.Attributes;

namespace MyNet.UI.Selection.Models;

public class SelectedWrapper<T>(T item) : EditableWrapper<T>(item), ISelectable
{
    [CanBeValidated(false)]
    public virtual bool IsSelectable { get; set; } = true;

    [CanBeValidated(false)]
    public virtual bool IsSelected { get; set; }

    public event EventHandler? SelectedChanged;

    public SelectedWrapper(T item, bool isSelected)
        : this(item) => IsSelected = isSelected;

    protected void RaiseIsSelectedChanged() => SelectedChanged?.Invoke(this, EventArgs.Empty);

    protected virtual void OnIsSelectedChanged() => RaiseIsSelectedChanged();

    protected virtual void OnIsSelectableChanged()
    {
        if (!IsSelectable) IsSelected = false;
    }

    protected override EditableWrapper<T> CreateCloneInstance(T item) => new SelectedWrapper<T>(item, IsSelected) { IsSelectable = IsSelectable };
}
