// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Reactive.Subjects;

namespace MyNet.UI.ViewModels
{
    public interface IItemViewModel<T>
    {
        T? Item { get; }

        Subject<T?> ItemChanged { get; }
    }
}
