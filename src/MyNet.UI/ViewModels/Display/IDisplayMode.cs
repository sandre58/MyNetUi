﻿// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.Observable;

namespace MyNet.UI.ViewModels.Display
{
    public interface IDisplayMode : IProvideValue<string>
    {
        string Key { get; }

        bool OverrideEmptySourceTemplate { get; }

        bool OverrideEmptyItemsTemplate { get; }

        void Reset();
    }
}
