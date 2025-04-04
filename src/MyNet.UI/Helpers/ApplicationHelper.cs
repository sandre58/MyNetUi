// -----------------------------------------------------------------------
// <copyright file="ApplicationHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;
using MyNet.UI.Resources;
using MyNet.Utilities;

namespace MyNet.UI.Helpers;

public static class ApplicationHelper
{
    public static string GetProductName()
    {
        var assembly = Assembly.GetEntryAssembly();
        var productAttr = assembly?.GetCustomAttribute<AssemblyProductAttribute>();
        return productAttr?.Product ?? string.Empty;
    }

    public static string GetVersion()
    {
        var assembly = Assembly.GetEntryAssembly();
        return UiResources.VersionAbbrX.FormatWith(assembly?.GetName().Version?.ToString() ?? string.Empty);
    }

    public static string GetCopyright()
    {
        var assembly = Assembly.GetEntryAssembly();
        var attr = assembly?.GetCustomAttribute<AssemblyCopyrightAttribute>();
        return attr?.Copyright ?? string.Empty;
    }

    public static string GetCompany()
    {
        var assembly = Assembly.GetEntryAssembly();
        var attr = assembly?.GetCustomAttribute<AssemblyCompanyAttribute>();
        return attr?.Company ?? string.Empty;
    }

    public static string GetDescription()
    {
        var assembly = Assembly.GetEntryAssembly();
        var attr = assembly?.GetCustomAttribute<AssemblyDescriptionAttribute>();
        return attr?.Description ?? string.Empty;
    }
}
