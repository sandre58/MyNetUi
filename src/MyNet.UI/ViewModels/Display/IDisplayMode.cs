// -----------------------------------------------------------------------
// <copyright file="IDisplayMode.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Observable;

namespace MyNet.UI.ViewModels.Display;

public interface IDisplayMode : IProvideValue<string>
{
    string Key { get; }

    bool OverrideEmptySourceTemplate { get; }

    bool OverrideEmptyItemsTemplate { get; }

    void Reset();
}
