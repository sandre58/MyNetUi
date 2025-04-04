// -----------------------------------------------------------------------
// <copyright file="WorkspaceDialogViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows.Input;
using MyNet.Observable.Attributes;
using MyNet.UI.Commands;
using MyNet.UI.Dialogs.Models;

namespace MyNet.UI.ViewModels.Workspace;

[CanBeValidatedForDeclaredClassOnly(false)]
[CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
public abstract class WorkspaceDialogViewModel : WorkspaceViewModel, IDialogViewModel
{
    #region Members

    /// <inheritdoc />
    /// <summary>
    /// Gets or sets the result.
    /// </summary>
    public bool? DialogResult { get; set; }

    public virtual bool LoadWhenDialogOpening => true;

    /// <summary>
    /// Gets close command.
    /// </summary>
    public ICommand CloseCommand { get; private set; }

    #endregion Members

    #region Constructors

    protected WorkspaceDialogViewModel() => CloseCommand = CommandsManager.Create<bool?>(Close, CanClose);

    #endregion

    #region Methods

    /// <summary>
    /// Can Close ?.
    /// </summary>
    protected virtual bool CanClose(bool? dialogResult) => true;

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    public virtual void Close(bool? dialogResult)
    {
        if (dialogResult is not null)
            DialogResult = dialogResult.Value;

        var e = new CancelEventArgs();
        OnCloseRequest(e);

        CloseRequest?.Invoke(this, e);
    }

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    public virtual void Close() => Close(null);

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "Used by children classes")]
    protected virtual void OnCloseRequest(CancelEventArgs e)
    {
        // Method intentionally left empty.
    }

    public virtual Task<bool> CanCloseAsync() => Task.FromResult(true);

    public virtual void Load()
    {
        DialogResult = null;

        Refresh();
    }

    #endregion Methods

    #region Events

    /// <inheritdoc />
    /// <summary>
    /// Closes the dialog.
    /// </summary>
    public event CancelEventHandler? CloseRequest;

    #endregion Events
}
