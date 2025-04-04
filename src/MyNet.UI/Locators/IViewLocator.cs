// -----------------------------------------------------------------------
// <copyright file="IViewLocator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.UI.Locators;

public interface IViewLocator
{
    void Register(Type type, Func<object> createInstance);

    object Get(Type viewType);
}
