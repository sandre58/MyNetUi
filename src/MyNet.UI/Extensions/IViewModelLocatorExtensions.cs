// -----------------------------------------------------------------------
// <copyright file="IViewModelLocatorExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.UI.Locators;

namespace MyNet.UI.Extensions;

public static class IViewModelLocatorExtensions
{
    public static TViewModel Get<TViewModel>(this IViewModelLocator viewModelLocator) => (TViewModel)viewModelLocator.Get(typeof(TViewModel));
}
