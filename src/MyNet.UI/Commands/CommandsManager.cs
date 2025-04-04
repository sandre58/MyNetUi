// -----------------------------------------------------------------------
// <copyright file="CommandsManager.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Windows.Input;

namespace MyNet.UI.Commands;

public static class CommandsManager
{
    private static ICommandFactory? _commandFactory;

    public static void Initialize(ICommandFactory commandFactory) => _commandFactory = commandFactory;

    public static ICommandFactory CommandFactory => _commandFactory ?? RelayCommandFactory.Default;

    public static ICommand Create(Action execute) => CommandFactory.Create(execute);

    public static ICommand Create(Action execute, Func<bool> canExectute) => CommandFactory.Create(execute, canExectute);

    public static ICommand Create<T>(Action<T?> execute) => CommandFactory.Create(execute);

    public static ICommand Create<T>(Action<T?> execute, Func<T?, bool> canExectute) => CommandFactory.Create(execute, canExectute);

    public static ICommand CreateNotNull<T>(Action<T> execute) => CommandFactory.Create<T>(x =>
    {
        if (x is null) return;
        execute.Invoke(x);
    });

    public static ICommand CreateNotNull<T>(Action<T> execute, Func<T, bool> canExectute)
        => CommandFactory.Create<T>(x =>
            {
                if (x is null) return;
                execute.Invoke(x);
            },
            x => x is not null && canExectute.Invoke(x));
}
