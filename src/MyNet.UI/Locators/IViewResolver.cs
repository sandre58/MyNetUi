// -----------------------------------------------------------------------
// <copyright file="IViewResolver.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.UI.Locators;

/// <summary>
/// Resolver that will resolve view types based on the view model type. For example, if a view model with the type
/// name <c>MyAssembly.ViewModels.PersonViewModel</c> is inserted, this could result in the view type
/// <c>MyAssembly.Views.PersonView</c>.
/// </summary>
public interface IViewResolver : IResolver
{
    /// <summary>
    /// Registers the specified view in the local cache. This cache will also be used by the
    /// <see cref="Resolve(Type)"/> method.
    /// </summary>
    /// <param name="viewModelType">Type of the view model.</param>
    /// <param name="viewType">Type of the view.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="viewModelType"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException">The <paramref name="viewType"/> is <c>null</c>.</exception>
    void Register(Type viewModelType, Type viewType);

    /// <summary>
    /// Determines whether the specified view type is compatible with the view model. A view is compatible
    /// if it's either resolved via naming conventions or registered manually.
    /// </summary>
    /// <param name="viewModelType">Type of the view model.</param>
    /// <param name="viewType">Type of the view.</param>
    /// <returns>
    ///   <c>true</c> if the view is compatible with the view model; otherwise, <c>false</c>.
    /// </returns>
    bool IsCompatible(Type viewModelType, Type viewType);

    /// <summary>
    /// Resolves a view type by the view model and the registered <see cref="IResolver.NamingConventions"/>.
    /// </summary>
    /// <param name="viewModelType">Type of the view model to resolve the view for.</param>
    /// <returns>The resolved view or <c>null</c> if the view could not be resolved.</returns>
    /// <remarks>
    /// Keep in mind that all results are cached. The cache itself is not automatically cleared when the
    /// <see cref="IResolver.NamingConventions"/> are changed. If the <see cref="IResolver.NamingConventions"/> are changed,
    /// the cache must be cleared manually.
    /// </remarks>
    /// <exception cref="ArgumentNullException">The <paramref name="viewModelType"/> is <c>null</c>.</exception>
    Type? Resolve(Type viewModelType);
}
