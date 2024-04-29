// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.UI.Navigation.Models;

namespace MyNet.UI.Navigation
{
    public class NavigatingEventArgs(INavigationPage? oldPage, INavigationPage newPage, NavigationMode mode, NavigationParameters? navigationParameters = null) : NavigationEventArgs(oldPage, newPage, mode, navigationParameters)
    {
        public bool Cancel { get; set; }
    }
}
