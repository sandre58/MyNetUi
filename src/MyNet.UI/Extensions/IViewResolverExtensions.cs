// -----------------------------------------------------------------------
// <copyright file="IViewResolverExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.UI.Locators;

namespace MyNet.UI.Extensions;

/// <summary>
/// Extension methods for the <see cref="IViewResolver"/>.
/// </summary>
public static class IViewResolverExtensions
{
    public static void Register<TViewModel, TView>(this IViewResolver viewResolver) => viewResolver.Register(typeof(TViewModel), typeof(TView));

    /// <summary>
    /// Resolves a view by the view model and the registered <see cref="IResolver.NamingConventions" />.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <param name="viewResolver">The view locator.</param>
    /// <returns>The resolved view or <c>null</c> if the view could not be resolved.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="viewResolver" /> is <c>null</c>.</exception>
    /// <remarks>Keep in mind that all results are cached. The cache itself is not automatically cleared when the
    /// <see cref="IResolver.NamingConventions" /> are changed. If the <see cref="IResolver.NamingConventions" /> are changed,
    /// the cache must be cleared manually.</remarks>
    public static Type? Resolve<TViewModel>(this IViewResolver viewResolver) => viewResolver.Resolve(typeof(TViewModel));
}
