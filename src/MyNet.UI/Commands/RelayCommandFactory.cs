// -----------------------------------------------------------------------
// <copyright file="RelayCommandFactory.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Windows.Input;

namespace MyNet.UI.Commands;

public class RelayCommandFactory : ICommandFactory
{
    public static readonly RelayCommandFactory Default = new();

    public ICommand Create(Action execute) => new RelayCommand(execute);

    public ICommand Create(Action execute, Func<bool> canExectute) => new RelayCommand(execute, canExectute);

    public ICommand Create<T>(Action<T?> execute) => new RelayCommand<T>(execute);

    public ICommand Create<T>(Action<T?> execute, Func<T?, bool> canExectute) => new RelayCommand<T>(execute, canExectute);
}
