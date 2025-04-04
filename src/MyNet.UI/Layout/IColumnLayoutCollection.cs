// -----------------------------------------------------------------------
// <copyright file="IColumnLayoutCollection.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace MyNet.UI.Layout;

public interface IColumnLayoutCollection : ICollection<IColumnLayout>
{
    event EventHandler? RefreshRequested;

    void Reset();

    void Refresh();

    void SetDisplayedColumns(IEnumerable<string> columns);
}
