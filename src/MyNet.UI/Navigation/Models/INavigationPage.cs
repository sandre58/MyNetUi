// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.UI.Navigation.Models
{
    public interface INavigationPage
    {
        Type? GetParentPageType();

        void OnNavigated(NavigationContext navigationContext);

        void OnNavigatingFrom(NavigatingContext navigatingContext);

        void OnNavigatingTo(NavigatingContext navigatingContext);
    }
}
