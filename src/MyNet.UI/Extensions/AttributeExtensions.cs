// -----------------------------------------------------------------------
// <copyright file="AttributeExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;
using MyNet.UI.Attributes;

namespace MyNet.UI.Extensions;

public static class AttributeExtensions
{
    public static bool IsRegisteredAsTransient(this Type type) => type.GetCustomAttributes<IsTransientAttribute>().Any();
}
