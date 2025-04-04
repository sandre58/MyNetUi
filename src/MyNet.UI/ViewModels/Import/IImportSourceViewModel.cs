// -----------------------------------------------------------------------
// <copyright file="IImportSourceViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyNet.UI.ViewModels.Import;

public interface IImportSourceViewModel<out T>
    where T : ImportableViewModel
{
    object View { get; }

    event EventHandler? ItemsLoadingRequested;

    IEnumerable<T> ProvideItems();

    Task InitializeAsync();

    void Reload();
}
