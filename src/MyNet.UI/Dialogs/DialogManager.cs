﻿// -----------------------------------------------------------------------
// <copyright file="DialogManager.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNet.UI.Dialogs.Messages;
using MyNet.UI.Dialogs.Models;
using MyNet.UI.Dialogs.Settings;
using MyNet.UI.Locators;
using MyNet.UI.Resources;
using MyNet.Utilities.Messaging;

namespace MyNet.UI.Dialogs;

/// <summary>
/// Provides methods and properties to display a window dialog.
/// </summary>
public static class DialogManager
{
    #region Fields

    private static IDialogService? _dialogService;
    private static IMessageBoxFactory? _messageBoxFactory;
    private static IViewResolver? _viewResolver;
    private static IViewLocator? _viewLocator;
    private static IViewModelLocator? _viewModelLocator;

    #endregion Fields

    #region Members

    public static IList<IDialogViewModel>? OpenedDialogs => _dialogService?.OpenedDialogs;

    public static bool HasOpenedDialogs => OpenedDialogs is not null && OpenedDialogs.Any();

    public static IDialogService DialogService => _dialogService ?? throw new InvalidOperationException("DialogService has not been initialized.");

    #endregion Members

    public static void Initialize(
        IDialogService dialogService,
        IMessageBoxFactory messageBoxFactory,
        IViewResolver viewResolver,
        IViewLocator viewLocator,
        IViewModelLocator viewModelLocator) => (_dialogService, _messageBoxFactory, _viewResolver, _viewLocator, _viewModelLocator) = (dialogService, messageBoxFactory, viewResolver, viewLocator, viewModelLocator);

    #region Show

    /// <summary>
    /// Displays a modal dialog.
    /// </summary>
    public static async Task ShowAsync<T>(Action<T>? closeAction = null)
        where T : class, IDialogViewModel
    {
        var vm = GetViewModel<T>();

        if (vm is not null)
        {
            await ShowAsync(vm, closeAction).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Displays a modal dialog.
    /// </summary>
    public static async Task ShowAsync(Type typeViewModel, Action<IDialogViewModel>? closeAction = null)
    {
        var vm = GetViewModel<IDialogViewModel>(typeViewModel);

        if (vm is not null)
        {
            await ShowAsync(vm, closeAction).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Displays a message dialog.
    /// </summary>
    public static async Task ShowAsync<T>(T viewModel, Action<T>? closeAction = null)
        where T : IDialogViewModel
    {
        if (_dialogService is null) return;

        var view = GetViewFromViewModel(viewModel.GetType());

        if (view is not null)
        {
            if (closeAction is not null)
                viewModel.CloseRequest += (_, _) => closeAction(viewModel);

            Messenger.Default.Send(new OpenDialogMessage(DialogType.Dialog, viewModel));

            await _dialogService.ShowAsync(view, viewModel).ConfigureAwait(false);
        }
    }

    #endregion Show

    #region ShowDialog

    /// <summary>
    /// Displays a modal dialog.
    /// </summary>
    public static async Task<bool?> ShowDialogAsync<TViewModel>()
        where TViewModel : class, IDialogViewModel
        => await ShowDialogAsync(typeof(TViewModel)).ConfigureAwait(false);

    /// <summary>
    /// Displays a modal dialog.
    /// </summary>
    /// <param name="typeViewModel">The view to include in workspace dialog.</param>
    public static async Task<bool?> ShowDialogAsync(Type typeViewModel) => GetViewModel<IDialogViewModel>(typeViewModel) is not { } vm ? false : await ShowDialogAsync(vm).ConfigureAwait(false);

    /// <summary>
    /// Displays a message dialog.
    /// </summary>
    /// <param name="viewModel">The view to include in workspace dialog.</param>
    public static async Task<bool?> ShowDialogAsync<T>(T viewModel)
        where T : IDialogViewModel
    {
        if (_dialogService is null) return null;

        Messenger.Default.Send(new OpenDialogMessage(DialogType.ModalDialog, viewModel));

        var view = GetViewFromViewModel(viewModel.GetType());

        return view is null ? false : await _dialogService.ShowDialogAsync(view, viewModel).ConfigureAwait(false);
    }

    #endregion ShowDialog

    #region MessageBox

    public static async Task<MessageBoxResult> ShowSuccessAsync(string message, string? title = null, MessageBoxResultOption buttons = MessageBoxResultOption.Ok)
        => await ShowMessageAsync(message, title ?? UiResources.Success, buttons, MessageSeverity.Success, MessageBoxResult.Ok).ConfigureAwait(false);

    /// <summary>
    /// Displays a message dialog.
    /// </summary>
    public static async Task<MessageBoxResult> ShowInformationAsync(string message, string? title = null, MessageBoxResultOption buttons = MessageBoxResultOption.Ok)
        => await ShowMessageAsync(message, title ?? UiResources.Information, buttons, MessageSeverity.Information, MessageBoxResult.Ok).ConfigureAwait(false);

    /// <summary>
    /// Displays a message dialog.
    /// </summary>
    public static async Task<MessageBoxResult> ShowErrorAsync(string message, string? title = null, MessageBoxResultOption buttons = MessageBoxResultOption.Ok)
        => await ShowMessageAsync(message, title ?? UiResources.Error, buttons, MessageSeverity.Error, MessageBoxResult.Ok).ConfigureAwait(false);

    /// <summary>
    /// Displays a message dialog.
    /// </summary>
    public static async Task<MessageBoxResult> ShowWarningAsync(string message, string? title = null, MessageBoxResultOption buttons = MessageBoxResultOption.Ok)
        => await ShowMessageAsync(message, title ?? UiResources.Warning, buttons, MessageSeverity.Warning, MessageBoxResult.Ok).ConfigureAwait(false);

    /// <summary>
    /// Displays a message dialog.
    /// </summary>
    public static async Task<MessageBoxResult> ShowQuestionAsync(string message, string? title = null) => await ShowMessageAsync(message, title ?? UiResources.Question, MessageBoxResultOption.YesNo, MessageSeverity.Question, MessageBoxResult.Yes).ConfigureAwait(false);

    /// <summary>
    /// Displays a message dialog.
    /// </summary>
    public static async Task<MessageBoxResult> ShowQuestionWithCancelAsync(string message, string? title = null) => await ShowMessageAsync(message, title ?? UiResources.Question, MessageBoxResultOption.YesNoCancel, MessageSeverity.Question, MessageBoxResult.Yes).ConfigureAwait(false);

    /// <summary>
    /// Displays a message box that has a message, title bar caption, button, and icon; and
    /// that accepts a default message box result and returns a result.
    /// </summary>
    /// <param name="message">
    /// A <see cref="string"/> that specifies the text to display.
    /// </param>
    /// <param name="caption">
    /// A <see cref="string"/> that specifies the title bar caption to display. Default value
    /// is an empty string.
    /// </param>
    /// <param name="button">
    /// A MessageBoxButton value that specifies which button or buttons to
    /// display. Default value is MessageBoxButton.OK.
    /// </param>
    /// <param name="severity">
    /// A MessageBoxImage value that specifies the icon to display. Default value
    /// is MessageBoxImage.None.
    /// </param>
    /// <param name="defaultResult">
    /// A <see cref="MessageBoxResult"/> value that specifies the default result of the
    /// message box. Default value is <see cref="MessageBoxResult.None"/>.
    /// </param>
    /// <returns>
    /// A <see cref="MessageBoxResult"/> value that specifies which message box button is
    /// clicked by the user.
    /// </returns>
    public static async Task<MessageBoxResult> ShowMessageAsync(
        string message,
        string? caption = "",
        MessageBoxResultOption button = MessageBoxResultOption.Ok,
        MessageSeverity severity = MessageSeverity.Information,
        MessageBoxResult defaultResult = MessageBoxResult.None)
    {
        var vm = _messageBoxFactory?.Create(message, caption, severity, button, defaultResult);
        return vm is null ? MessageBoxResult.None : await ShowMessageBoxAsync(vm).ConfigureAwait(false);
    }

    /// <summary>
    /// Displays a message box that has a message, title bar caption, button, and icon; and
    /// that accepts a default message box result and returns a result.
    /// </summary>
    /// <returns>
    /// A <see cref="MessageBoxResult"/> value that specifies which message box button is
    /// clicked by the user.
    /// </returns>
    public static async Task<MessageBoxResult> ShowMessageBoxAsync(IMessageBox viewModel)
    {
        if (_dialogService is null) return MessageBoxResult.None;

        Messenger.Default.Send(new OpenDialogMessage(DialogType.MessageBox, viewModel));

        return await _dialogService.ShowMessageBoxAsync(viewModel).ConfigureAwait(false) ?? MessageBoxResult.None;
    }

    #endregion MessageBox

    #region Files

    /// <summary>
    /// Displays the OpenFileDialog.
    /// </summary>
    /// <returns>
    /// If the user clicks the OK button of the dialog that is displayed, true is returned;
    /// otherwise false.
    /// </returns>
    public static async Task<(bool? Result, string Filename)> ShowOpenFileDialogAsync()
    {
        if (_dialogService is null) return (false, string.Empty);

        Messenger.Default.Send(new OpenDialogMessage(DialogType.FileDialog, null));

        var settings = new OpenFileDialogSettings();
        var result = await _dialogService.ShowOpenFileDialogAsync(settings).ConfigureAwait(false);
        return (result, settings.FileName);
    }

    /// <summary>
    /// Displays the OpenFileDialog.
    /// </summary>
    /// <param name="settings">The settings for the open file dialog.</param>
    /// <returns>
    /// If the user clicks the OK button of the dialog that is displayed, true is returned;
    /// otherwise false.
    /// </returns>
    public static async Task<(bool? Result, string Filename)> ShowOpenFileDialogAsync(OpenFileDialogSettings settings)
    {
        if (_dialogService is null) return (false, string.Empty);

        Messenger.Default.Send(new OpenDialogMessage(DialogType.FileDialog, null));

        var result = await _dialogService.ShowOpenFileDialogAsync(settings).ConfigureAwait(false);
        return (result, settings.FileName);
    }

    /// <summary>
    /// Displays the SaveFileDialog.
    /// </summary>
    /// <param name="settings">The settings for the save file dialog.</param>
    /// <returns>
    /// If the user clicks the OK button of the dialog that is displayed, true is returned;
    /// otherwise false.
    /// </returns>
    public static async Task<(bool? Result, string Filename)> ShowSaveFileDialogAsync(SaveFileDialogSettings settings)
    {
        if (_dialogService is null) return (false, string.Empty);

        Messenger.Default.Send(new OpenDialogMessage(DialogType.FileDialog, null));

        var result = await _dialogService.ShowSaveFileDialogAsync(settings).ConfigureAwait(false);
        return (result, settings.FileName);
    }

    /// <summary>
    /// Displays the FolderBrowserDialog.
    /// </summary>
    /// <param name="settings">The settings for the folder browser dialog.</param>
    /// <returns>
    /// If the user clicks the OK button of the dialog that is displayed, true is returned;
    /// otherwise false.
    /// </returns>
    public static async Task<(bool? Result, string? SelectedPath)> ShowFolderBrowserDialogAsync(OpenFolderDialogSettings settings)
    {
        if (_dialogService is null) return (false, string.Empty);

        Messenger.Default.Send(new OpenDialogMessage(DialogType.FileDialog, null));

        var result = await _dialogService.ShowFolderDialogAsync(settings).ConfigureAwait(false);
        return (result, settings.Folder);
    }

    #endregion Files

    private static T? GetViewModel<T>()
        where T : class
        => GetViewModel<T>(typeof(T));

    private static T? GetViewModel<T>(Type typeViewModel)
        where T : class
        => (T?)_viewModelLocator?.Get(typeViewModel);

    private static object? GetViewFromViewModel(Type viewModelType)
    {
        var viewType = _viewResolver?.Resolve(viewModelType);

        if (viewType is null) throw new InvalidOperationException($"{viewType} has not been resolved.");

        var view = _viewLocator?.Get(viewType);

        return view;
    }
}
