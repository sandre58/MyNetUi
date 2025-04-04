// -----------------------------------------------------------------------
// <copyright file="PagingChangedEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.UI.ViewModels.List;

public class PagingChangedEventArgs(int page, int pageSize) : EventArgs
{
    public int PageSize { get; } = pageSize;

    public int Page { get; } = page;
}
