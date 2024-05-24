// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyNet.UI.ViewModels.Import
{
    public interface IImportSourceViewModel<out T> where T : ImportableViewModel
    {
        event EventHandler? ItemsLoadingRequested;

        Task InitializeAsync();

        IEnumerable<T> ProvideItems();

        void Reload();
    }
}
