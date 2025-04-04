// -----------------------------------------------------------------------
// <copyright file="RelayCommand.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Windows.Input;

namespace MyNet.UI.Commands;

/// <summary>
/// Creates a new command.
/// </summary>
/// <param name="execute">The execution logic.</param>
/// <param name="canExecute">The execution status logic.</param>
public class RelayCommand(Action execute, Func<bool>? canExecute = null) : ICommand
{
    private readonly Action _execute = execute ?? throw new ArgumentNullException(nameof(execute));

    /// <summary>
    /// Raised when OnCanExecuteChanged is called.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2290:Field-like events should not be virtual", Justification = "Must be override in WpfCommand")]
    public virtual event EventHandler? CanExecuteChanged;

    /// <summary>
    /// Determines whether this WpfCommand can execute in its current state.
    /// </summary>
    /// <param name="parameter">
    /// Data used by the command. If the command does not require data to be passed,
    /// this object can be set to null.
    /// </param>
    /// <returns>true if this command can be executed; otherwise, false.</returns>
    public virtual bool CanExecute(object? parameter) => canExecute is null || canExecute();

    /// <summary>
    /// Executes the WpfCommand on the current command target.
    /// </summary>
    /// <param name="parameter">
    /// Data used by the command. If the command does not require data to be passed,
    /// this object can be set to null.
    /// </param>
    public virtual void Execute(object? parameter) => _execute();

    /// <summary>
    /// Method used to raise the CanExecuteChanged event
    /// to indicate that the return value of the CanExecute
    /// method has changed.
    /// </summary>
    public virtual void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}

/// <summary>
/// Creates a new command.
/// </summary>
/// <param name="execute">The execution logic.</param>
/// <param name="canExecute">The execution status logic.</param>
public class RelayCommand<T>(Action<T?> execute, Func<T?, bool>? canExecute = null) : ICommand
{
    private readonly Action<T?> _execute = execute ?? throw new ArgumentNullException(nameof(execute));

    /// <summary>
    /// Raised when OnCanExecuteChanged is called.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2290:Field-like events should not be virtual", Justification = "Must be override in WpfCommand")]
    public virtual event EventHandler? CanExecuteChanged;

    /// <summary>
    /// Determines whether this WpfCommand can execute in its current state.
    /// </summary>
    /// <param name="parameter">
    /// Data used by the command. If the command does not require data to be passed,
    /// this object can be set to null.
    /// </param>
    /// <returns>true if this command can be executed; otherwise, false.</returns>
    public virtual bool CanExecute(object? parameter) => canExecute is null || canExecute((T?)parameter);

    /// <summary>
    /// Executes the WpfCommand on the current command target.
    /// </summary>
    /// <param name="parameter">
    /// Data used by the command. If the command does not require data to be passed,
    /// this object can be set to null.
    /// </param>
    public virtual void Execute(object? parameter) => _execute((T?)parameter);

    /// <summary>
    /// Method used to raise the CanExecuteChanged event
    /// to indicate that the return value of the CanExecute
    /// method has changed.
    /// </summary>
    public virtual void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
