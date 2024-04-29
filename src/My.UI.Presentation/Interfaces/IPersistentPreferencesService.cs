// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.UI.Presentation.Interfaces
{
    public interface IPersistentPreferencesService
    {
        void Save();

        void Reset();

        void Reload();
    }
}
