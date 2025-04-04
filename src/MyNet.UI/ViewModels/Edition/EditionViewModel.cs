// -----------------------------------------------------------------------
// <copyright file="EditionViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MyNet.Observable.Attributes;
using MyNet.UI.Commands;
using MyNet.UI.Dialogs;
using MyNet.UI.Resources;
using MyNet.UI.Toasting;
using MyNet.UI.ViewModels.Workspace;

namespace MyNet.UI.ViewModels.Edition;

[CanBeValidatedForDeclaredClassOnly(false)]
[CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
public abstract class EditionViewModel : WorkspaceDialogViewModel
{
    public ICommand SaveCommand { get; protected set; }

    public ICommand SaveAsyncCommand { get; protected set; }

    public ICommand SaveAndCloseCommand { get; protected set; }

    public ICommand SaveAndCloseAsyncCommand { get; protected set; }

    public ICommand CancelCommand { get; protected set; }

    protected EditionViewModel()
    {
        CancelCommand = CommandsManager.Create(async () => await CancelAsync().ConfigureAwait(false));
        SaveCommand = CommandsManager.Create(() => Save(), CanSave);
        SaveAsyncCommand = CommandsManager.Create(async () => await SaveAsync().ConfigureAwait(false), CanSave);
        SaveAndCloseCommand = CommandsManager.Create(SaveAndClose, CanSave);
        SaveAndCloseAsyncCommand = CommandsManager.Create(async () => await SaveAndCloseAsync().ConfigureAwait(false), CanSave);

        Mode = ScreenMode.Creation;
    }

    protected override string CreateTitle() => Mode == ScreenMode.Edition ? UiResources.Edition : UiResources.Creation;

    protected virtual async Task<MessageBoxResult> SavingRequestAsync() => await DialogManager.ShowQuestionWithCancelAsync(MessageResources.ItemSavingQuestion, UiResources.Edition).ConfigureAwait(false);

    public override async Task<bool> CanCloseAsync()
    {
        if (DialogResult.HasValue || !IsModified())
            return true;
        var result = await SavingRequestAsync().ConfigureAwait(false);

        switch (result)
        {
            case MessageBoxResult.Yes:
                DialogResult = true;
                return await CanCloseWithYesResultAsync().ConfigureAwait(false);
            case MessageBoxResult.No:
                DialogResult = false;
                return true;
            case MessageBoxResult.Cancel or MessageBoxResult.None:
                DialogResult = null;
                return false;
            default:
                return true;
        }
    }

    protected virtual async Task<bool> CanCloseWithYesResultAsync() => await SaveAsync().ConfigureAwait(false);

    [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "Used by children classes")]
    protected virtual void OnClosingWithNoResult(CancelEventArgs e) => DialogResult = false;

    protected virtual void OnClosingWithCancelResult(CancelEventArgs e)
    {
        DialogResult = null;
        e.Cancel = true;
    }

    #region Cancel

    protected virtual async Task<bool> CanCancelAsync() => !IsModified() || await DialogManager.ShowQuestionAsync(MessageResources.ItemModificationCancellingQuestion, UiResources.Edition).ConfigureAwait(false) == MessageBoxResult.Yes;

    protected virtual async Task CancelAsync()
    {
        if (await CanCancelAsync().ConfigureAwait(false))
            Close(false);
    }

    #endregion Cancel

    #region Save

    private void SaveInternal()
    {
        SaveCore();
        ResetIsModified();
    }

    protected bool Save()
    {
        var args = new CancelEventArgs();
        OnSaveRequested(args);

        if (args.Cancel) return false;

        if (ValidateProperties())
        {
            SaveInternal();

            OnSaveSucceeded();

            return true;
        }

        var errors = GetValidationErrors().ToList();
        ShowValidationErrors(errors);
        return false;
    }

    protected async Task<bool> SaveAsync()
    {
        var args = new CancelEventArgs();
        OnSaveRequested(args);

        if (args.Cancel) return false;

        if (ValidateProperties())
        {
            await ExecuteAsync(SaveInternal).ConfigureAwait(false);

            OnSaveSucceeded();

            return true;
        }

        var errors = GetValidationErrors().ToList();
        ShowValidationErrors(errors);
        return false;
    }

    protected virtual void ShowValidationErrors(IEnumerable<string> errors) => errors.ToList().ForEach(x => ToasterManager.ShowError(x));

    protected abstract void SaveCore();

    protected virtual void OnSaveSucceeded() { }

    [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "Used by children classes")]
    protected virtual void OnSaveRequested(CancelEventArgs args)
    {
    }

    protected virtual bool CanSave() => true;

    #endregion Save

    #region SaveAndClose

    protected virtual void SaveAndClose()
    {
        if (Save())
            Close(true);
    }

    protected virtual async Task SaveAndCloseAsync()
    {
        if (await SaveAsync().ConfigureAwait(false))
            Close(true);
    }

    #endregion SaveAndClose
}
