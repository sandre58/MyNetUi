// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.UI.Presentation.Interfaces
{
    public interface IPersistentSettingsService
    {
        void Save();

        void Reset();

        void Reload();
    }
}
