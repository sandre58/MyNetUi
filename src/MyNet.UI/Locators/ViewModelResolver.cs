// -----------------------------------------------------------------------
// <copyright file="ViewModelResolver.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MyNet.Utilities.Helpers;

namespace MyNet.UI.Locators;

/// <summary>
/// Resolver that will resolve view model types based on the view type. For example, if a view with the type
/// name <c>MyAssembly.Views.PersonView</c> is inserted, this could result in the view model type
/// <c>MyAssembly.ViewModels.PersonViewModel</c>.
/// </summary>
public class ViewModelResolver : ResolverBase, IViewModelResolver
{
    /// <summary>
    /// Registers the specified view model in the local cache. This cache will also be used by the
    /// <see cref="Resolve(Type)"/> method.
    /// </summary>
    /// <param name="viewType">Type of the view.</param>
    /// <param name="viewModelType">Type of the view model.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="viewType"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentNullException">The <paramref name="viewModelType"/> is <c>null</c>.</exception>
    public void Register(Type viewType, Type viewModelType)
    {
        var viewTypeName = TypeHelper.GetTypeNameWithAssembly(viewType.AssemblyQualifiedName ?? string.Empty);
        var viewModelTypeName = TypeHelper.GetTypeNameWithAssembly(viewModelType.AssemblyQualifiedName ?? string.Empty);

        Register(viewTypeName, viewModelTypeName);
    }

    /// <summary>
    /// Determines whether the specified view type is compatible with the view model. A view is compatible
    /// if it's either resolved via naming conventions or registered manually.
    /// </summary>
    /// <param name="viewType">Type of the view.</param>
    /// <param name="viewModelType">Type of the view model.</param>
    /// <returns>
    ///   <c>true</c> if the view is compatible with the view model; otherwise, <c>false</c>.
    /// </returns>
    public virtual bool IsCompatible(Type viewType, Type viewModelType)
    {
        var viewTypeName = TypeHelper.GetTypeNameWithAssembly(viewType.AssemblyQualifiedName ?? string.Empty);
        var viewModelTypeName = TypeHelper.GetTypeNameWithAssembly(viewModelType.AssemblyQualifiedName ?? string.Empty);

        var values = ResolveValues(viewTypeName);
        return values.Contains(viewModelTypeName);
    }

    /// <summary>ResolveNamingConventionResolveNamingConvention
    /// Resolves a view model type by the view and the registered <see cref="IResolver.NamingConventions"/>.
    /// </summary>
    /// <param name="viewType">Type of the view to resolve the view model for.</param>
    /// <returns>The resolved view model or <c>null</c> if the view model could not be resolved.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="viewType"/> is <c>null</c>.</exception>
    public virtual Type? Resolve(Type viewType)
    {
        var viewTypeName = TypeHelper.GetTypeNameWithAssembly(viewType.AssemblyQualifiedName ?? string.Empty);

        var resolvedType = Resolve(viewTypeName);
        return !string.IsNullOrWhiteSpace(resolvedType) ? TypeHelper.GetTypeFrom(resolvedType) : null;
    }

    /// <summary>
    /// Resolves a single naming convention.
    /// <para/>
    /// This method is abstract because each locator should or could use its own naming convention to resolve
    /// the type. The <see cref="ResolverBase.Resolve"/> method has prepared all the values such as the assembly name and the
    /// only thing this method has to do is to actually resolve a string value based on the specified naming convention.
    /// </summary>
    /// <param name="assembly">The assembly name.</param>
    /// <param name="typeToResolveName">The full type name of the type to resolve.</param>
    /// <param name="namingConvention">The naming convention to use for resolving.</param>
    /// <returns>The resolved naming convention.</returns>
    protected override string ResolveNamingConvention(string assembly, string typeToResolveName, string namingConvention)
        => NamingConvention.ResolveViewModelByViewName(assembly, typeToResolveName, namingConvention);

    /// <summary>
    /// Gets the default naming conventions.
    /// </summary>
    /// <returns>An enumerable of default naming conventions.</returns>
    protected override IEnumerable<string> GetDefaultNamingConventions() => AllNamingConventions;

    public static IEnumerable<string> AllNamingConventions { get; } =
    [
        string.Format(CultureInfo.InvariantCulture, "{0}.ViewModels.{1}ViewModel", NamingConvention.Up, NamingConvention.ViewName),
        string.Format(CultureInfo.InvariantCulture, "{0}.ViewModels.{1}ControlViewModel", NamingConvention.Up, NamingConvention.ViewName),
        string.Format(CultureInfo.InvariantCulture, "{0}.ViewModels.{1}WindowViewModel", NamingConvention.Up, NamingConvention.ViewName),
        string.Format(CultureInfo.InvariantCulture, "{0}.ViewModels.{1}PageViewModel", NamingConvention.Up, NamingConvention.ViewName),
        string.Format(CultureInfo.InvariantCulture, "{0}.ViewModels.{1}ActivityViewModel", NamingConvention.Up, NamingConvention.ViewName),
        string.Format(CultureInfo.InvariantCulture, "{0}.ViewModels.{1}FragmentViewModel", NamingConvention.Up, NamingConvention.ViewName),

        string.Format(CultureInfo.InvariantCulture, "{0}.ViewModels.{1}ViewModel", NamingConvention.Assembly, NamingConvention.ViewName),
        string.Format(CultureInfo.InvariantCulture, "{0}.ViewModels.{1}ControlViewModel", NamingConvention.Assembly, NamingConvention.ViewName),
        string.Format(CultureInfo.InvariantCulture, "{0}.ViewModels.{1}WindowViewModel", NamingConvention.Assembly, NamingConvention.ViewName),
        string.Format(CultureInfo.InvariantCulture, "{0}.ViewModels.{1}PageViewModel", NamingConvention.Assembly, NamingConvention.ViewName),
        string.Format(CultureInfo.InvariantCulture, "{0}.ViewModels.{1}ActivityViewModel", NamingConvention.Assembly, NamingConvention.ViewName),
        string.Format(CultureInfo.InvariantCulture, "{0}.ViewModels.{1}FragmentViewModel", NamingConvention.Assembly, NamingConvention.ViewName),

        string.Format(CultureInfo.InvariantCulture, "{0}.{1}ViewModel", NamingConvention.Current, NamingConvention.ViewName),
        string.Format(CultureInfo.InvariantCulture, "{0}.{1}ControlViewModel", NamingConvention.Current, NamingConvention.ViewName),
        string.Format(CultureInfo.InvariantCulture, "{0}.{1}WindowViewModel", NamingConvention.Current, NamingConvention.ViewName),
        string.Format(CultureInfo.InvariantCulture, "{0}.{1}PageViewModel", NamingConvention.Current, NamingConvention.ViewName),
        string.Format(CultureInfo.InvariantCulture, "{0}.{1}ActivityViewModel", NamingConvention.Current, NamingConvention.ViewName),
        string.Format(CultureInfo.InvariantCulture, "{0}.{1}FragmentViewModel", NamingConvention.Current, NamingConvention.ViewName)
    ];
}
