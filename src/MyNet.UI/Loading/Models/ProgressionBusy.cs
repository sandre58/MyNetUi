// -----------------------------------------------------------------------
// <copyright file="ProgressionBusy.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace MyNet.UI.Loading.Models;

public class ProgressionBusy : Busy
{
    public string Title { get; set; } = string.Empty;

    public IEnumerable<string>? Messages { get; set; }

    public double Value { get; set; }
}
