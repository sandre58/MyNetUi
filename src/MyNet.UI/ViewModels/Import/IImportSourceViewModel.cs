// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyNet.UI.ViewModels.Import
{
    public interface IImportSourceViewModel<out T> where T : ImportableViewModel
    {
        object View { get; }

        event EventHandler? ItemsLoadingRequested;

        IEnumerable<T> ProvideItems();

        Task InitializeAsync();

        void Reload();
    }
}
