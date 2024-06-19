// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using MyNet.UI.Collections;

namespace MyNet.UI.Layout
{
    public class ColumnLayoutCollection(string[]? defaultColumns = null) : UiObservableCollection<IColumnLayout>, IColumnLayoutCollection
    {
        private readonly string[]? _defaultColumns = defaultColumns;

        public event EventHandler? RefreshRequested;

        public void Reset()
        {
            this.ToList().ForEach(x => x.Reset());

            if (_defaultColumns is not null)
                SetDisplayedColumns(_defaultColumns);

            Refresh();
        }

        public void Refresh() => RefreshRequested?.Invoke(this, EventArgs.Empty);

        public void SetDisplayedColumns(IEnumerable<string> columns) => this.ToList().ForEach(x => x.IsVisible = columns.Contains(x.Identifier) || !x.CanBeHidden);
    }
}
