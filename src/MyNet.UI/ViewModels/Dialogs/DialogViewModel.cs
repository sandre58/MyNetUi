// -----------------------------------------------------------------------
// <copyright file="DialogViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows.Input;
using MyNet.Observable;
using MyNet.Observable.Attributes;
using MyNet.UI.Commands;
using MyNet.UI.Dialogs.Models;

namespace MyNet.UI.ViewModels.Dialogs;

[CanBeValidatedForDeclaredClassOnly(false)]
[CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
public abstract class DialogViewModel : EditableObject, IDialogViewModel
{
    #region Members

    /// <summary>
    /// Gets or sets title.
    /// </summary>
    [UpdateOnCultureChanged]
    public string? Title { get; set; }

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

    protected DialogViewModel()
    {
        CloseCommand = CommandsManager.Create<bool?>(Close, CanClose);
        UpdateTitle();
    }

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

    public virtual void Load() => DialogResult = null;

    #endregion Methods

    #region Events

    /// <inheritdoc />
    /// <summary>
    /// Closes the dialog.
    /// </summary>
    public event CancelEventHandler? CloseRequest;

    #endregion Events

    #region Culture Management

    protected virtual string CreateTitle() => string.Empty;

    protected void UpdateTitle()
    {
        var newTitle = CreateTitle();

        if (!string.IsNullOrEmpty(newTitle))
            Title = newTitle;
    }

    #endregion
}
