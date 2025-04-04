// -----------------------------------------------------------------------
// <copyright file="IViewModelLocator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.UI.Locators;

public interface IViewModelLocator
{
    object Get(Type viewModelType);
}
