// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using My.Utilities.Observable.Models;

namespace MyNet.UI.Presentation.ViewModels.Import
{
    public abstract class ImportSourceViewModel : ObservableObject
    {
        public abstract (IEnumerable<T>, bool) LoadItems<T>() where T : ImportableViewModel;

        public abstract Task<bool> InitializeAsync();

        public virtual Task RefreshAsync() => Task.CompletedTask;

        public virtual bool IsEnabled() => true;
    }
}
