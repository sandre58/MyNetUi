// -----------------------------------------------------------------------
// <copyright file="PagingResponse.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.UI.ViewModels.List;

public readonly struct PagingResponse(int currentPage, int totalPages, int totalItems) : IEquatable<PagingResponse>
{
    public int CurrentPage { get; } = currentPage;

    public int TotalPages { get; } = totalPages;

    public int TotalItems { get; } = totalItems;

    public override bool Equals(object? obj) => obj is PagingResponse pagingResponse && Equals(pagingResponse);

    bool IEquatable<PagingResponse>.Equals(PagingResponse other) => Equals(other);

    public override int GetHashCode() => HashCode.Combine(CurrentPage, TotalPages, TotalItems);

    public static bool operator ==(PagingResponse left, PagingResponse right) => left.Equals(right);

    public static bool operator !=(PagingResponse left, PagingResponse right) => !(left == right);
}
