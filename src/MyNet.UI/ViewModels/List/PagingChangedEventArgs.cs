// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.UI.ViewModels.List
{
    public class PagingChangedEventArgs(int page, int pageSize) : EventArgs
    {
        public int PageSize { get; } = pageSize;

        public int Page { get; } = page;
    }
}
