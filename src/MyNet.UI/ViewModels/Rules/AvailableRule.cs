// -----------------------------------------------------------------------
// <copyright file="AvailableRule.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Observable.Translatables;

namespace MyNet.UI.ViewModels.Rules;

public class AvailableRule<T>(string resourceKey, Func<T> create, Func<bool>? canAdd = null) : StringTranslatable(resourceKey), IAvailableRule<T>
    where T : IEditableRule
{
    public bool IsEnabled => canAdd?.Invoke() ?? true;

    public virtual T Create() => create.Invoke();
}
