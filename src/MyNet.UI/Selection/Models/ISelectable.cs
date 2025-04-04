// -----------------------------------------------------------------------
// <copyright file="ISelectable.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.UI.Selection.Models;

public interface ISelectable
{
    bool IsSelectable { get; set; }

    bool IsSelected { get; set; }

    event EventHandler SelectedChanged;
}
