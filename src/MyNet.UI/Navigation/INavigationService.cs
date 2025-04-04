// -----------------------------------------------------------------------
// <copyright file="INavigationService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MyNet.UI.Navigation.Models;
using MyNet.Utilities.Suspending;

namespace MyNet.UI.Navigation;

public interface INavigationService
{
    event EventHandler<NavigatingEventArgs>? Navigating;

    event EventHandler<NavigationEventArgs>? Navigated;

    event EventHandler? HistoryCleared;

    event EventHandler? Cleared;

    NavigationContext? CurrentContext { get; }

    Suspender JournalSuspender { get; }

    IEnumerable<NavigationContext> GetBackJournal();

    IEnumerable<NavigationContext> GetForwardJournal();

    bool GoBack();

    bool CanGoBack();

    bool GoForward();

    bool CanGoForward();

    void ClearJournal();

    void Clear();

    bool NavigateTo(INavigationPage page, NavigationParameters? navigationParameters = null);
}
