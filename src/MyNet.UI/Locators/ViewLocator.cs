// -----------------------------------------------------------------------
// <copyright file="ViewLocator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MyNet.UI.Extensions;

namespace MyNet.UI.Locators;

public class ViewLocator : IViewLocator
{
    private readonly Dictionary<Type, Lazy<object>> _instances = [];

    public void Register(Type type, Func<object> createInstance)
    {
        if (!type.IsRegisteredAsTransient() && !_instances.ContainsKey(type))
        {
            _instances.Add(type, new Lazy<object>(createInstance));
        }
    }

    public object Get(Type viewType)
    {
        Register(viewType, () => Activator.CreateInstance(viewType)!);

        return _instances.TryGetValue(viewType, out var instance) ? instance.Value : Activator.CreateInstance(viewType)!;
    }
}
