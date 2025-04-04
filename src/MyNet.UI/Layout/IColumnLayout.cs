// -----------------------------------------------------------------------
// <copyright file="IColumnLayout.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.UI.Layout;

public interface IColumnLayout
{
    bool CanBeHidden { get; set; }

    bool IsVisible { get; set; }

    string Width { get; set; }

    int Index { get; set; }

    string Identifier { get; }

    void Reset();
}
