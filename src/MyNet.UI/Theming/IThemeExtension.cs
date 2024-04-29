// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace MyNet.UI.Theming
{
    public interface IThemeExtension
    {
        IDictionary<string, object?> GetResources(Theme theme);
    }
}
