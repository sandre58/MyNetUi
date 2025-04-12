// -----------------------------------------------------------------------
// <copyright file="PagingViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MyNet.Observable;
using MyNet.UI.Collections;
using MyNet.UI.Commands;
using MyNet.Utilities;

namespace MyNet.UI.ViewModels.List.Paging;

public class PagingViewModel : ObservableObject, IPagingViewModel
{
    private readonly UiObservableCollection<int> _pages = [];

    public int TotalItems { get; private set; }

    public int CurrentPage { get; private set; } = 1;

    public int TotalPages { get; private set; }

    public int PageSize { get; set; }

    public ReadOnlyObservableCollection<int> Pages { get; }

    public ICommand MoveToFirstPageCommand { get; }

    public ICommand MoveToLastPageCommand { get; }

    public ICommand MoveToPreviousPageCommand { get; }

    public ICommand MoveToNextPageCommand { get; }

    public ICommand MoveToPageCommand { get; }

    public ICommand SetPageSizeCommand { get; }

    public event EventHandler<PagingChangedEventArgs>? PagingChanged;

    public PagingViewModel(int pageSize = 25)
    {
        PageSize = pageSize;
        Pages = new(_pages);
        MoveToFirstPageCommand = CommandsManager.Create(MoveToFirstPage, () => CurrentPage > 1);
        MoveToLastPageCommand = CommandsManager.Create(MoveToLastPage, () => CurrentPage < TotalPages);
        MoveToPreviousPageCommand = CommandsManager.Create(MoveToPreviousPage, () => CurrentPage > 1);
        MoveToNextPageCommand = CommandsManager.Create(MoveToNextPage, () => CurrentPage < TotalPages);
        MoveToPageCommand = CommandsManager.Create<int>(MoveToPage, x => x >= 1 && x <= TotalPages && x != CurrentPage);
        SetPageSizeCommand = CommandsManager.Create<int>(x => PageSize = x, x => x > 0);
    }

    public void MoveToPage(int value)
    {
        if (value < 1 || value > TotalPages || value == CurrentPage) return;

        PagingChanged?.Invoke(this, new PagingChangedEventArgs(value, PageSize));
    }

    public void MoveToNextPage() => MoveToPage(CurrentPage + 1);

    public void MoveToPreviousPage() => MoveToPage(CurrentPage - 1);

    public void MoveToFirstPage() => MoveToPage(1);

    public void MoveToLastPage() => MoveToPage(TotalPages);

    public void Update(PagingResponse response)
    {
        TotalItems = response.TotalItems;
        CurrentPage = response.CurrentPage;
        TotalPages = response.TotalPages;

        if (_pages.Count != TotalPages)
            _pages.Set(Enumerable.Range(1, TotalPages));
    }

    protected void OnPageSizeChanged() => PagingChanged?.Invoke(this, new PagingChangedEventArgs(CurrentPage, PageSize));
}
