// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using MyNet.UI.Navigation.Models;
using MyNet.Utilities.Suspending;

namespace MyNet.UI.Navigation
{
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
}
