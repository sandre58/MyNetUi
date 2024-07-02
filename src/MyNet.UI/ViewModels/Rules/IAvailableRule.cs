// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.Observable;

namespace MyNet.UI.ViewModels.Rules
{
    public interface IAvailableRule<out T> : IProvideValue<string> where T : IEditableRule
    {
        bool IsEnabled { get; }

        T Create();
    }
}
