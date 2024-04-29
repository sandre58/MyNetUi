// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.UI.Commands
{
    public interface ICommandFactory
    {
        ICommand Create(Action execute);

        ICommand Create(Action execute, Func<bool> canExectute);

        ICommand Create<T>(Action<T?> execute);

        ICommand Create<T>(Action<T?> execute, Func<T?, bool> canExectute);
    }
}
