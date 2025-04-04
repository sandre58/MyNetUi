// -----------------------------------------------------------------------
// <copyright file="IViewModelResolverExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.UI.Locators;

namespace MyNet.UI.Extensions;

/// <summary>
/// Extension methods for the <see cref="IViewModelResolver"/>.
/// </summary>
public static class IViewModelResolverExtensions
{
    /// <summary>
    /// Registers the specified view model in the local cache. This cache will also be used by the
    /// <see cref="Resolve{TView}" /> method.
    /// </summary>
    /// <typeparam name="TView">The type of the view.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <param name="viewModelResolver">The view model locator.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="viewModelResolver" /> is <c>null</c>.</exception>
    public static void Register<TView, TViewModel>(this IViewModelResolver viewModelResolver) => viewModelResolver.Register(typeof(TView), typeof(TViewModel));

    /// <summary>
    /// Resolves a view model type by the view and the registered <see cref="IResolver.NamingConventions" />.
    /// </summary>
    /// <typeparam name="TView">The type of the view.</typeparam>
    /// <param name="viewModelResolver">The view model locator.</param>
    /// <returns>The resolved view model or <c>null</c> if the view model could not be resolved.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="viewModelResolver" /> is <c>null</c>.</exception>
    /// <remarks>Keep in mind that all results are cached. The cache itself is not automatically cleared when the
    /// <see cref="IResolver.NamingConventions" /> are changed. If the <see cref="IResolver.NamingConventions" /> are changed,
    /// the cache must be cleared manually.</remarks>
    public static Type? Resolve<TView>(this IViewModelResolver viewModelResolver) => viewModelResolver.Resolve(typeof(TView));
}
