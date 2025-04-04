// -----------------------------------------------------------------------
// <copyright file="ICommandFactory.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Windows.Input;

namespace MyNet.UI.Commands;

public interface ICommandFactory
{
    ICommand Create(Action execute);

    ICommand Create(Action execute, Func<bool> canExectute);

    ICommand Create<T>(Action<T?> execute);

    ICommand Create<T>(Action<T?> execute, Func<T?, bool> canExectute);
}
