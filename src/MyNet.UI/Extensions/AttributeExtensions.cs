// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reflection;
using MyNet.UI.Attributes;

namespace MyNet.UI.Extensions
{
    public static class AttributeExtensions
    {
        public static bool IsRegisteredAsTransient(this Type type) => type.GetCustomAttributes<IsTransientAttribute>().Any();
    }
}
