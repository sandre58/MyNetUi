// -----------------------------------------------------------------------
// <copyright file="IThemeExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace MyNet.UI.Theming;

public interface IThemeExtension
{
    IDictionary<string, object?> GetResources(Theme theme);
}
