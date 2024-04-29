// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.UI.ViewModels
{
    public interface IItemEditionViewModel<T> : IItemViewModel<T>
    {
        void SetOriginalItem(T? item);
    }
}
