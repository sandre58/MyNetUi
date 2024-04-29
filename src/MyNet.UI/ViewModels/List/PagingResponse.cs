// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.UI.ViewModels.List
{
    public readonly struct PagingResponse
    {
        public PagingResponse(int currentPage, int totalPages, int totalItems)
        {
            CurrentPage = currentPage;
            TotalPages = totalPages;
            TotalItems = totalItems;
        }

        public int CurrentPage { get; }

        public int TotalPages { get; }

        public int TotalItems { get; }
    }
}
