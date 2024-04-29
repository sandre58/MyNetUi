// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.UI.Layout
{
    public interface IColumnLayout
    {
        bool CanBeHidden { get; set; }

        bool IsVisible { get; set; }

        string Width { get; set; }

        int Index { get; set; }

        string Identifier { get; }

        void Reset();
    }
}
