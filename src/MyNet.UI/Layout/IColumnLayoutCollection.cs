// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace MyNet.UI.Layout
{
    public interface IColumnLayoutCollection : ICollection<IColumnLayout>
    {
        event EventHandler? RefreshRequested;

        void Reset();

        void Refresh();

        void SetDisplayedColumns(IEnumerable<string> columns);
    }
}
