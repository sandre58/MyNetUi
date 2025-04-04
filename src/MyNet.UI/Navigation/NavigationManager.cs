// -----------------------------------------------------------------------
// <copyright file="NavigationManager.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MyNet.UI.Locators;
using MyNet.UI.Navigation.Models;

namespace MyNet.UI.Navigation;

public static class NavigationManager
{
    public const string ItemParameter = "Item";

    private static INavigationService? _navigationService;
    private static IViewModelLocator? _viewModelLocator;

    public static NavigationContext? CurrentContext => _navigationService?.CurrentContext;

    public static void Initialize(INavigationService navigationService, IViewModelLocator viewModelLocator)
        => (_navigationService, _viewModelLocator) = (navigationService, viewModelLocator);

    public static void NavigateTo<TPage>(object paramValue, string paramKey = ItemParameter)
        where TPage : INavigationPage
        => NavigateTo(typeof(TPage), paramValue, paramKey);

    public static bool NavigateTo<TPage>(IEnumerable<KeyValuePair<string, object?>>? parameters = null)
        where TPage : INavigationPage
        => NavigateTo(typeof(TPage), parameters);

    public static bool NavigateTo(Type typePage, object paramValue, string paramKey = ItemParameter)
        => NavigateTo(typePage, [new(paramKey, paramValue)]);

    public static bool NavigateTo(Type typePage, IEnumerable<KeyValuePair<string, object?>>? parameters = null)
        => _viewModelLocator?.Get(typePage) is INavigationPage page && NavigateTo(page, parameters);

    public static bool NavigateTo(INavigationPage page, object? paramValue = null, string paramKey = ItemParameter)
        => NavigateTo(page, paramValue is null ? null : new List<KeyValuePair<string, object?>>(1) { new(paramKey, paramValue) });

    public static bool NavigateTo(INavigationPage page, IEnumerable<KeyValuePair<string, object?>>? parameters = null) => _navigationService?.NavigateTo(page, ToNavigationParameters(parameters)) ?? false;

    public static void GoBack() => _navigationService?.GoBack();

    public static bool CanGoBack() => _navigationService?.CanGoBack() ?? false;

    public static void GoForward() => _navigationService?.GoForward();

    public static bool CanGoForward() => _navigationService?.CanGoForward() ?? false;

    public static void ClearHistory() => _navigationService?.ClearJournal();

    public static void Clear() => _navigationService?.Clear();

    private static NavigationParameters? ToNavigationParameters(IEnumerable<KeyValuePair<string, object?>>? parameters)
    {
        if (parameters is null) return null;

        var res = new NavigationParameters();

        foreach (var parameter in parameters)
            res.Add(parameter.Key, parameter.Value);

        return res;
    }
}
