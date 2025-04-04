// -----------------------------------------------------------------------
// <copyright file="IAvailableRule.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Observable;

namespace MyNet.UI.ViewModels.Rules;

public interface IAvailableRule<out T> : IProvideValue<string>
    where T : IEditableRule
{
    bool IsEnabled { get; }

    T Create();
}
