﻿// -----------------------------------------------------------------------
// <copyright file="NamingConvention.cs" company="Stéphane ANDRE">
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
/// Class that is able to resolve naming conventions.
/// </summary>
public static class NamingConvention
{
    /// <summary>
    /// The view model name constant that will be replaced by the naming convention. This constant will be
    /// replaced by the view model name without the "ViewModel" prefix.
    /// <para />
    /// For example, the following naming convention:
    ///   <c>[AS].UI.Views.[VM]View</c>.
    /// <para />
    /// will result in the following view for <c>ExampleViewModel</c>:
    ///   <c>MyAssembly.UI.Views.ExampleView.xaml</c>.
    /// </summary>
    public const string ViewModelName = "[VM]";

    /// <summary>
    /// The view name constant that will be replaced by the naming convention. This constant will be
    /// replaced by the view name without the "View", "Window" and "Control" prefix.
    /// <para />
    /// For example, the following naming convention:
    ///   <c>[AS].UI.Views.[VW]ViewModel</c>.
    /// <para />
    /// will result in the following view model for <c>ExampleViewModel</c>:
    ///   <c>MyAssembly.UI.Views.ExampleView.xaml</c>.
    /// </summary>
    public const string ViewName = "[VW]";

    /// <summary>
    /// The assembly constant that will be replaced by the naming convention. This constant will be
    /// replaced by the assembly name.
    /// <para />
    /// For example, the following naming convention:
    ///   <c>[AS].UI.Views.[VM]View</c>.
    /// <para />
    /// will result in the following view for <c>ExampleViewModel</c>:
    ///   <c>MyAssembly.UI.Views.ExampleView.xaml</c>.
    /// </summary>
    public const string Assembly = "[AS]";

    /// <summary>
    /// The up constant that will be replaced by the naming convention. This constant will be
    /// move up one step in the type namespace tree.
    /// <para />
    /// For example, the following naming convention:
    ///   <c>[UP].Views.[VM]View</c>.
    /// <para />
    /// will result in the following view for <c>Catel.ViewModels.ExampleViewModel</c>:
    ///   <c>Catel.Views.ExampleView.xaml</c>.
    /// </summary>
    public const string Up = "[UP]";

    /// <summary>
    /// The current constant that will keep the convention in the current namespace.
    /// <para />
    /// For example, the following naming convention:
    ///   <c>[CURRENT].[VM]View</c>.
    /// <para />
    /// will result in the following view for <c>Catel.ExampleViewModel</c>:
    ///   <c>Catel.ExampleView.xaml</c>.
    /// </summary>
    public const string Current = "[CURRENT]";
    private static readonly string[] SuffixesArray = ["View", "Control", "UserControl", "Page", "Activity"];
    private static readonly string[] PostfixesToRemove = ["ViewModel"];

    #region Methods

    /// <summary>
    /// Resolves the convention by using the view model name.
    /// <para/>
    /// This method does not add assemblies or resolve types, but will only return a string containing the resolved
    /// value based on the convention.
    /// <para/>
    /// The following constants can be used in this method:
    /// * <see cref="ViewModelName"/>.
    /// </summary>
    /// <param name="assembly">The assembly which is the base namespace.</param>
    /// <param name="fullViewModelName">Class name of the view model. This parameter must only contain the type name, not the full
    /// type. For example <c>ExampleViewModel</c>.</param>
    /// <param name="conventionToUse">The convention to use.</param>
    /// <returns>The resolved convention.</returns>
    /// <exception cref="ArgumentException">If <paramref name="assembly"/> is <c>null</c> or whitespace.</exception>
    /// <exception cref="ArgumentException">If <paramref name="fullViewModelName"/> is <c>null</c> or whitespace.</exception>
    /// <exception cref="ArgumentException">If <paramref name="conventionToUse"/> is <c>null</c> or whitespace.</exception>
    public static string ResolveViewByViewModelName(string assembly, string fullViewModelName, string conventionToUse)
    {
        var viewModelWithoutViewModel = TypeHelper.GetTypeNameWithoutNamespace(fullViewModelName);
        viewModelWithoutViewModel = RemoveAllPostfixes(viewModelWithoutViewModel, PostfixesToRemove);

        var constantsWithValues = new Dictionary<string, string>
        {
            { Assembly, assembly },
            { Current, TypeHelper.GetTypeNamespace(fullViewModelName) },
            { ViewModelName, viewModelWithoutViewModel },
            { ".ViewModels.", ".Views." }
        };

        return ResolveNamingConvention(constantsWithValues, conventionToUse, fullViewModelName);
    }

    /// <summary>
    /// Resolves the convention by using the view name.
    /// <para/>
    /// This method does not add assemblies or resolve types, but will only return a string containing the resolved
    /// value based on the convention.
    /// <para/>
    /// The following constants can be used in this method:
    /// * <see cref="ViewName"/>.
    /// </summary>
    /// <param name="assembly">The assembly which is the base namespace.</param>
    /// <param name="fullViewName">Class name of the view. This parameter must only contain the type name, not the full
    /// type. For example <c>ExampleView</c>.</param>
    /// <param name="conventionToUse">The convention to use.</param>
    /// <returns>The resolved convention.</returns>
    /// <exception cref="ArgumentException">If <paramref name="assembly"/> is <c>null</c> or whitespace.</exception>
    /// <exception cref="ArgumentException">If <paramref name="fullViewName"/> is <c>null</c> or whitespace.</exception>
    /// <exception cref="ArgumentException">If <paramref name="conventionToUse"/> is <c>null</c> or whitespace.</exception>
    public static string ResolveViewModelByViewName(string assembly, string fullViewName, string conventionToUse)
    {
        var viewWithoutView = TypeHelper.GetTypeNameWithoutNamespace(fullViewName);
        viewWithoutView = RemoveAllPostfixes(viewWithoutView, SuffixesArray);

        var constantsWithValues = new Dictionary<string, string>
        {
            { Assembly, assembly },
            { Current, TypeHelper.GetTypeNamespace(fullViewName) },
            { ViewName, viewWithoutView },
            { ".Views.", ".ViewModels." }
        };

        return ResolveNamingConvention(constantsWithValues, conventionToUse, fullViewName);
    }

    /// <summary>
    /// Resolves the naming convention.
    /// </summary>
    /// <param name="constantsWithValues">
    /// The constants with values. The <c>Key</c> must be the constant as it will be used inside the convention. The
    /// <c>Value</c> is the value the constant must be replaced with.
    /// </param>
    /// <param name="conventionToUse">The convention to use.</param>
    /// <returns>The resolved convention.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="constantsWithValues"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="conventionToUse"/> is <c>null</c> or whitespace.</exception>
    public static string ResolveNamingConvention(Dictionary<string, string> constantsWithValues, string conventionToUse)
        => constantsWithValues.Aggregate(conventionToUse, (current, constantWithValue) => current.Replace(constantWithValue.Key, constantWithValue.Value, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Resolves the naming convention.
    /// </summary>
    /// <param name="constantsWithValues">
    /// The constants with values. The <c>Key</c> must be the constant as it will be used inside the convention. The
    /// <c>Value</c> is the value the constant must be replaced with.
    /// </param>
    /// <param name="conventionToUse">The convention to use.</param>
    /// <param name="value">The value you try to resolve.</param>
    /// <returns>The resolved convention.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="constantsWithValues"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="conventionToUse"/> is <c>null</c> or whitespace.</exception>
    public static string ResolveNamingConvention(Dictionary<string, string> constantsWithValues, string conventionToUse, string value)
    {
        var fullnamespace = GetParentPath(value);
        var separator = GetParentSeparator(fullnamespace) ?? string.Empty;
        var namespaces = fullnamespace.Split([separator], StringSplitOptions.RemoveEmptyEntries).ToList();

        var result = conventionToUse;

        var lastIndex = conventionToUse.LastIndexOf(Up, StringComparison.OrdinalIgnoreCase);
        if (lastIndex != -1)
        {
            result = string.Format(CultureInfo.InvariantCulture, "{0}{1}", namespaces, conventionToUse[(lastIndex + Up.Length)..]);
        }

        return ResolveNamingConvention(constantsWithValues, result);
    }

    /// <summary>
    /// Gets the parent path with the separator of choice. For example, the following path:
    /// <para />
    /// <c>Catel.Services</c>.
    /// <para />
    /// Will return the following value when using the dot (.) as separator:
    /// <para />
    /// <c>Catel.MVVM</c>.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>The parent path.</returns>
    /// <remarks>
    /// This method automatically finds the parent separator by calling the <see cref="GetParentSeparator"/> and
    /// then calls <see cref="GetParentPath(string, string)"/>.
    /// </remarks>
    /// <exception cref="ArgumentException">The <paramref name="path"/> is <c>null</c> or whitespace.</exception>
    public static string GetParentPath(string path)
    {
        var separator = GetParentSeparator(path);
        return separator is null ? path : GetParentPath(path, separator);
    }

    /// <summary>
    /// Gets the parent path with the separator of choice. For example, the following path:
    /// <para />
    /// <c>Catel.Services</c>.
    /// <para />
    /// Will return the following value when using the dot (.) as separator:
    /// <para />
    /// <c>Catel.MVVM</c>.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="separator">The separator.</param>
    /// <returns>The parent path.</returns>
    /// <exception cref="ArgumentException">The <paramref name="path"/> is <c>null</c> or whitespace.</exception>
    /// <exception cref="ArgumentException">The <paramref name="separator"/> is <c>null</c> or empty.</exception>
    public static string GetParentPath(string path, string separator)
    {
        if (!path.Contains(separator, StringComparison.OrdinalIgnoreCase))
        {
            return path;
        }

        var lastIndex = path.LastIndexOf(separator, StringComparison.Ordinal);
        return path[..lastIndex];
    }

    /// <summary>
    /// Gets the parent separator.
    /// <para />
    /// This method tries to use the following separators:
    /// <list type="number">
    ///   <item><description>Backslash (\)</description></item>
    ///   <item><description>Slash (/)</description></item>
    ///   <item><description>Dot (.)</description></item>
    ///   <item><description>Pipe (|)</description></item>
    /// </list>
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>The separator or <c>null</c> if no known separator was found.</returns>
    /// <exception cref="ArgumentException">The <paramref name="path"/> is <c>null</c> or whitespace.</exception>
    public static string? GetParentSeparator(string path)
    {
        var knownSeparators = new[] { "\\", "/", "|", "." };

        return Array.Find(knownSeparators, path.Contains);
    }

    /// <summary>
    /// Removes all postfixes from the specified value.
    /// </summary>
    /// <param name="value">The value to remove the postfixes from.</param>
    /// <param name="postfixesToRemove">The postfixes to remove.</param>
    /// <returns>The value without the postfixes.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="value"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">The <paramref name="postfixesToRemove"/> is <c>null</c> or an empty array.</exception>
    private static string RemoveAllPostfixes(string value, string[] postfixesToRemove)
    {
        foreach (var (postfix, lastIndex) in postfixesToRemove.Where(postfix => value.EndsWith(postfix, StringComparison.OrdinalIgnoreCase))
                     .Select(postfix => new
                     {
                         postfix, lastIndex = value.LastIndexOf(postfix, StringComparison.CurrentCultureIgnoreCase)
                     })
                     .Select(t => (t.postfix, t.lastIndex)))
        {
            value = value.Remove(lastIndex, postfix.Length);
            break;
        }

        return value;
    }

    #endregion
}
