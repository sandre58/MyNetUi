// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.UI.Navigation.Models
{
    /// <summary>
    /// Encapsulates information about a navigation request.
    /// </summary>
    public class NavigationContext(INavigationPage? oldPage, INavigationPage newPage, NavigationMode mode, NavigationParameters? navigationParameters = null)
    {
        public INavigationPage? OldPage { get; } = oldPage;

        public INavigationPage Page { get; } = newPage;

        public NavigationParameters? Parameters { get; } = navigationParameters;

        public NavigationMode Mode { get; } = mode;
    }
}
