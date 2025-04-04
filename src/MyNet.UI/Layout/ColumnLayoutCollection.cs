// -----------------------------------------------------------------------
// <copyright file="ColumnLayoutCollection.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using MyNet.UI.Collections;

namespace MyNet.UI.Layout;

public class ColumnLayoutCollection(string[]? defaultColumns = null) : UiObservableCollection<IColumnLayout>, IColumnLayoutCollection
{
    public event EventHandler? RefreshRequested;

    public void Reset()
    {
        this.ToList().ForEach(x => x.Reset());

        if (defaultColumns is not null)
            SetDisplayedColumns(defaultColumns);

        Refresh();
    }

    public void Refresh() => RefreshRequested?.Invoke(this, EventArgs.Empty);

    public void SetDisplayedColumns(IEnumerable<string> columns) => this.ToList().ForEach(x => x.IsVisible = columns.Contains(x.Identifier) || !x.CanBeHidden);
}
