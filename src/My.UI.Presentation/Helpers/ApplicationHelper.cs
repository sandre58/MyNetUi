// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using My.Utilities;
using My.Utilities.Resources;

namespace MyNet.UI.Presentation.Helpers
{
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
            return MyResources.VersionAbbrX.FormatWith(assembly?.GetName().Version?.ToString() ?? string.Empty);
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
}
