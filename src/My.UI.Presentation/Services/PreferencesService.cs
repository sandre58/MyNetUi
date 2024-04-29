// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using My.Utilities;
using MyNet.UI.Presentation.Interfaces;

namespace MyNet.UI.Presentation.Services
{
    public class PreferencesService(IEnumerable<IPersistentSettingsService> groups) : IPersistentPreferencesService, IDisposable
    {
        private bool _disposedValue;
        private readonly IList<IPersistentSettingsService> _groups = groups.ToList();

        public void Reload() => _groups.ForEach(x => x.Reload());

        public void Reset() => _groups.ForEach(x => x.Reset());

        public void Save() => _groups.ForEach(x => x.Save());

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _groups.OfType<IDisposable>().ForEach(x => x.Dispose());
                }

                _disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
