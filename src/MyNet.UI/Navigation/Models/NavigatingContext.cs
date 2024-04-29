// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.UI.Navigation.Models
{
    public class NavigatingContext(INavigationPage? oldPage, INavigationPage newPage, NavigationMode mode, NavigationParameters? navigationParameters = null) : NavigationContext(oldPage, newPage, mode, navigationParameters)
    {
        public bool Cancel { get; set; }
    }
}
