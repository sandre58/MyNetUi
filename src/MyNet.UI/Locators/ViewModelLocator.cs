// -----------------------------------------------------------------------
// <copyright file="ViewModelLocator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.UI.Locators;

public class ViewModelLocator(IServiceProvider serviceProvider) : IViewModelLocator
{
    public object Get(Type viewModelType) => serviceProvider.GetService(viewModelType) ?? Activator.CreateInstance(viewModelType)!;
}
