// -----------------------------------------------------------------------
// <copyright file="ScreenMode.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.UI.ViewModels;

/// <summary>
/// Enum des états possible d'un viewModel.
/// </summary>
public enum ScreenMode
{
    /// <summary>
    /// Default.
    /// </summary>
    Unknown,

    /// <summary>
    /// Mode Creation.
    /// </summary>
    Creation,

    /// <summary>
    /// Mode Edition.
    /// </summary>
    Edition,

    /// <summary>
    /// Mode Read Only.
    /// </summary>
    Read
}
