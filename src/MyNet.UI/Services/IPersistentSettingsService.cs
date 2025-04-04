// -----------------------------------------------------------------------
// <copyright file="IPersistentSettingsService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.UI.Services;

public interface IPersistentSettingsService
{
    void Save();

    void Reset();

    void Reload();
}
