// -----------------------------------------------------------------------
// <copyright file="IPersistentPreferencesService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.UI.Services;

public interface IPersistentPreferencesService
{
    void Save();

    void Reset();

    void Reload();
}
