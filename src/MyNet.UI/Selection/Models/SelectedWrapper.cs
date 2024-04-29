// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using MyNet.Observable;
using MyNet.Observable.Attributes;

namespace MyNet.UI.Selection.Models
{
    public class SelectedWrapper<T> : EditableWrapper<T>, ISelectable
    {
        [CanBeValidated(false)]
        public virtual bool IsSelectable { get; set; } = true;

        [CanBeValidated(false)]
        public virtual bool IsSelected { get; set; }

        public event EventHandler? SelectedChanged;

        public SelectedWrapper(T item) : base(item) { }

        public SelectedWrapper(T item, bool isSelected) : this(item) => IsSelected = isSelected;

        protected void RaiseIsSelectedChanged() => SelectedChanged?.Invoke(this, new EventArgs());

        protected virtual void OnIsSelectedChanged() => RaiseIsSelectedChanged();

        protected virtual void OnIsSelectableChanged()
        {
            if (!IsSelectable) IsSelected = false;
        }

        protected override EditableWrapper<T> CreateCloneInstance(T item) => new SelectedWrapper<T>(item, IsSelected) { IsSelectable = IsSelectable };

    }
}
