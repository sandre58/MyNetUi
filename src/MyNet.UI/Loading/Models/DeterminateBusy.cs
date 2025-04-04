// -----------------------------------------------------------------------
// <copyright file="DeterminateBusy.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.UI.Loading.Models;

public class DeterminateBusy : Busy
{
    public string? Message { get; set; }

    public double Value { get; set; }

    public double Maximum { get; set; } = 1;

    public double Minimum { get; set; }
}
