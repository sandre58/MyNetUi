// -----------------------------------------------------------------------
// <copyright file="IEditableRule.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace MyNet.UI.ViewModels.Rules;

public interface IEditableRule : INotifyPropertyChanged
{
    public bool CanRemove { get; }

    public bool CanMove { get; }
}
