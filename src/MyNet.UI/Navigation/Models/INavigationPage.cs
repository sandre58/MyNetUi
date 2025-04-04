// -----------------------------------------------------------------------
// <copyright file="INavigationPage.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

namespace MyNet.UI.Navigation.Models;

public interface INavigationPage
{
    Type? GetParentPageType();

    void OnNavigated(NavigationContext navigationContext);

    [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "Used by children classes")]
    void OnNavigatingFrom(NavigatingContext navigatingContext);

    void OnNavigatingTo(NavigatingContext navigatingContext);
}
