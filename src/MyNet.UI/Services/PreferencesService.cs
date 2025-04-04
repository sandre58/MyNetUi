// -----------------------------------------------------------------------
// <copyright file="PreferencesService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using MyNet.Utilities;

namespace MyNet.UI.Services;

public class PreferencesService(IEnumerable<IPersistentSettingsService> groups) : IPersistentPreferencesService, IDisposable
{
    private readonly IList<IPersistentSettingsService> _groups = [.. groups];
    private bool _disposedValue;

    public void Reload() => _groups.ForEach(x => x.Reload());

    public void Reset() => _groups.ForEach(x => x.Reset());

    public void Save() => _groups.ForEach(x => x.Save());

    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue)
            return;
        if (disposing)
        {
            _groups.OfType<IDisposable>().ForEach(x => x.Dispose());
        }

        _disposedValue = true;
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
