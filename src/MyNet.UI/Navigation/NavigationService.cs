// -----------------------------------------------------------------------
// <copyright file="NavigationService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using MyNet.UI.Navigation.Models;
using MyNet.Utilities;
using MyNet.Utilities.Suspending;

namespace MyNet.UI.Navigation;

public class NavigationService : INavigationService
{
    private readonly Stack<NavigationContext> _backStack = new();
    private readonly Stack<NavigationContext> _forwardStack = new();

    public event EventHandler<NavigatingEventArgs>? Navigating;

    public event EventHandler<NavigationEventArgs>? Navigated;

    public event EventHandler? HistoryCleared;

    public event EventHandler? Cleared;

    public NavigationContext? CurrentContext { get; private set; }

    public Suspender JournalSuspender { get; } = new();

    private void RaiseNavigating(NavigatingContext navigatingContext)
    {
        var args = new NavigatingEventArgs(navigatingContext.OldPage, navigatingContext.Page, navigatingContext.Mode, navigatingContext.Parameters);
        Navigating?.Invoke(this, args);
        navigatingContext.Cancel = args.Cancel;
    }

    private void RaiseNavigated(NavigationContext navigationContext)
        => Navigated?.Invoke(this, new NavigationEventArgs(navigationContext.OldPage, navigationContext.Page, navigationContext.Mode, navigationContext.Parameters));

    private void RaiseHistoryCleared() => HistoryCleared?.Invoke(this, EventArgs.Empty);

    private void RaiseCleared() => Cleared?.Invoke(this, EventArgs.Empty);

    public virtual IEnumerable<NavigationContext> GetBackJournal() => [.. _backStack];

    public virtual IEnumerable<NavigationContext> GetForwardJournal() => [.. _forwardStack];

    public virtual bool GoBack()
    {
        if (!CanGoBack()) return false;

        var previousContext = _backStack.Peek();
        return Navigate(CurrentContext?.Page, previousContext.Page, NavigationMode.Back, previousContext.Parameters);
    }

    public virtual bool CanGoBack() => GetBackJournal().Any();

    public virtual bool GoForward()
    {
        if (!CanGoForward()) return false;

        var nextContext = _forwardStack.Peek();
        return Navigate(CurrentContext?.Page, nextContext.Page, NavigationMode.Forward, nextContext.Parameters);
    }

    public virtual bool CanGoForward() => GetForwardJournal().Any();

    protected virtual void AddBackEntry(NavigationContext navigatingContext) => _backStack.Push(navigatingContext);

    protected virtual void AddForwardEntry(NavigationContext navigatingContext) => _forwardStack.Push(navigatingContext);

    protected virtual void RemoveBackEntry(NavigationContext context) => _backStack.Remove(context);

    protected virtual void RemoveForwardEntry(NavigationContext context) => _forwardStack.Remove(context);

    protected virtual void UpdateJournal(NavigationMode mode, NavigationContext navigatingContext)
    {
        switch (mode)
        {
            case NavigationMode.Normal:
                if (!JournalSuspender.IsSuspended)
                    AddBackEntry(navigatingContext);
                _forwardStack.Clear();
                break;
            case NavigationMode.Back:
                _ = _backStack.Pop();
                if (!JournalSuspender.IsSuspended)
                    AddForwardEntry(navigatingContext);
                break;
            case NavigationMode.Forward:
                _ = _forwardStack.Pop();
                if (!JournalSuspender.IsSuspended)
                    AddBackEntry(navigatingContext);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }

    protected virtual void UpdateCurrentContext(NavigationContext navigationContext) => CurrentContext = navigationContext;

    public virtual void ClearJournal()
    {
        _backStack.Clear();
        _forwardStack.Clear();

        RaiseHistoryCleared();
    }

    public virtual void Clear()
    {
        CurrentContext = null;

        RaiseCleared();
    }

    public bool NavigateTo(INavigationPage page, NavigationParameters? navigationParameters = null) => Navigate(CurrentContext?.Page, page, NavigationMode.Normal, navigationParameters);

    protected virtual bool Navigate(INavigationPage? oldPage, INavigationPage newPage, NavigationMode mode, NavigationParameters? navigationParameters)
    {
        var navigatingContext = new NavigatingContext(oldPage, newPage, mode, navigationParameters);

        OnNavigatingFrom(navigatingContext);

        if (navigatingContext.Cancel) return false;

        OnNavigatingTo(navigatingContext);

        if (navigatingContext.Cancel) return false;

        RaiseNavigating(navigatingContext);

        if (navigatingContext.Cancel) return false;

        if (CurrentContext is not null)
            UpdateJournal(mode, CurrentContext);

        UpdateCurrentContext(new NavigationContext(oldPage, newPage, mode, navigationParameters));

        OnNavigated(CurrentContext!);

        RaiseNavigated(CurrentContext!);

        return true;
    }

    protected virtual void OnNavigatingFrom(NavigatingContext navigatingContext) => navigatingContext.OldPage?.OnNavigatingFrom(navigatingContext);

    protected virtual void OnNavigatingTo(NavigatingContext navigatingContext) => navigatingContext.Page.OnNavigatingTo(navigatingContext);

    protected virtual void OnNavigated(NavigationContext navigatingContext) => navigatingContext.Page.OnNavigated(navigatingContext);
}
