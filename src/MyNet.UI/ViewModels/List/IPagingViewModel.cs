// -----------------------------------------------------------------------
// <copyright file="IPagingViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Windows.Input;

namespace MyNet.UI.ViewModels.List;

public interface IPagingViewModel : INotifyPropertyChanged
{
    event EventHandler<PagingChangedEventArgs>? PagingChanged;

    int PageSize { get; set; }

    int CurrentPage { get; }

    int TotalPages { get; }

    int TotalItems { get; }

    ICommand MoveToFirstPageCommand { get; }

    ICommand MoveToLastPageCommand { get; }

    ICommand MoveToPreviousPageCommand { get; }

    ICommand MoveToNextPageCommand { get; }

    ICommand MoveToPageCommand { get; }

    ICommand SetPageSizeCommand { get; }

    void MoveToPage(int value);

    void MoveToNextPage();

    void MoveToPreviousPage();

    void MoveToFirstPage();

    void MoveToLastPage();

    void Update(PagingResponse response);
}
