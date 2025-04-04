// -----------------------------------------------------------------------
// <copyright file="IItemEditionViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.UI.ViewModels;

public interface IItemEditionViewModel<T> : IItemViewModel<T>
{
    void SetOriginalItem(T? item);
}
