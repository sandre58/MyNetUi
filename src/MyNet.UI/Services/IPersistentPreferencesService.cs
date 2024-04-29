// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.UI.Services
{
    public interface IPersistentPreferencesService
    {
        void Save();

        void Reset();

        void Reload();
    }
}
