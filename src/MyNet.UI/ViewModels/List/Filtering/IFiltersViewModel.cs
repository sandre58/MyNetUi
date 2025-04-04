// -----------------------------------------------------------------------
// <copyright file="IFiltersViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.UI.ViewModels.List.Filtering;

public interface IFiltersViewModel
{
    void Reset();

    void Clear();

    void Refresh();

    event EventHandler<FiltersChangedEventArgs>? FiltersChanged;
}
