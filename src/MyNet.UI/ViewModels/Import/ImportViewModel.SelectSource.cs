// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNet.Observable;
using MyNet.UI.Commands;
using MyNet.Utilities;

namespace MyNet.UI.ViewModels.Import
{
    public class ImportSourceChangedEventArgs : EventArgs
    {
        public ImportSourceChangedEventArgs(bool hasErrors) => HasErrors = hasErrors;

        public bool HasErrors { get; }
    }

    public class ImportSelectSourceViewModel<T> : ObservableObject, IItemsProvider<T> where T : ImportableViewModel
    {
        public IDictionary<ImportSource, ImportSourceViewModel> SourceViewModels { get; }

        public ImportSource? CurrentSource { get; private set; }

        public bool HasErrors { get; private set; }

        public bool IgnoreErrors { get; set; }

        public ICommand SelectSourceCommand { get; set; }

        public ICommand ReloadSourceCommand { get; set; }

        public ICommand IgnoreErrorsCommand { get; set; }

        public ICommand ReloadCurrentSourceCommand { get; set; }

        public event EventHandler? CurrentSourceChanged;

        public ImportSelectSourceViewModel(IDictionary<ImportSource, ImportSourceViewModel> sources)
        {
            SourceViewModels = sources;
            SelectSourceCommand = CommandsManager.CreateNotNull<ImportSource>(async x => await SelectSourceAsync(x).ConfigureAwait(false), x => SourceViewModels.ContainsKey(x) && SourceViewModels[x].IsEnabled());
            ReloadCurrentSourceCommand = CommandsManager.Create(() => LoadSource(CurrentSource!.Value, false), () => CurrentSource.HasValue);
            ReloadSourceCommand = CommandsManager.CreateNotNull<ImportSource>(x => LoadSource(x, false));
            IgnoreErrorsCommand = CommandsManager.CreateNotNull<ImportSource>(x => LoadSource(x, true));
        }

        public async Task RefreshAsync()
            => await Task.WhenAll(SourceViewModels.Values.Select(x => x.RefreshAsync()).ToArray()).ConfigureAwait(false);

        public void Reset()
        {
            CurrentSource = null;
            CurrentSourceChanged?.Invoke(this, EventArgs.Empty);
        }

        private async Task SelectSourceAsync(ImportSource source)
        {
            if (SourceViewModels.TryGetValue(source, out var value))
            {
                var result = await value.InitializeAsync().ConfigureAwait(false);

                if (result)
                    LoadSource(source, false);
            }
        }

        private void LoadSource(ImportSource source, bool ignoreErrors)
        {
            if (SourceViewModels.TryGetValue(source, out var value))
            {
                IgnoreErrors = ignoreErrors;
                CurrentSource = source;
                CurrentSourceChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public IEnumerable<T> ProvideItems()
        {
            if (!CurrentSource.HasValue) return [];

            var (items, hasErrors) = SourceViewModels[CurrentSource.Value].LoadItems<T>();
            HasErrors = hasErrors;
            return items;
        }
    }
}
