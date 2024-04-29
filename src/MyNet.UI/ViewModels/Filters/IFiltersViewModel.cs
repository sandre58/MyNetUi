// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.UI.ViewModels.Filters
{
    public interface IFiltersViewModel
    {
        void Reset();

        void Clear();

        void Refresh();

        event EventHandler<FiltersChangedEventArgs>? FiltersChanged;
    }
}
