// -----------------------------------------------------------------------
// <copyright file="IItemViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Reactive.Subjects;

namespace MyNet.UI.ViewModels;

public interface IItemViewModel<T>
{
    T? Item { get; }

    Subject<T?> ItemChanged { get; }
}
