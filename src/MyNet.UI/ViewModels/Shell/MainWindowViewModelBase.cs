// -----------------------------------------------------------------------
// <copyright file="MainWindowViewModelBase.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using DynamicData.Binding;
using MyNet.Observable;
using MyNet.UI.Commands;
using MyNet.UI.Dialogs;
using MyNet.UI.Helpers;
using MyNet.UI.Loading;
using MyNet.UI.Loading.Models;
using MyNet.UI.Messages;
using MyNet.UI.Notifications;
using MyNet.UI.Services;
using MyNet.UI.Theming;
using MyNet.Utilities;
using MyNet.Utilities.Localization;
using MyNet.Utilities.Messaging;
using PropertyChanged;

namespace MyNet.UI.ViewModels.Shell;

public class MainWindowViewModelBase : LocalizableObject
{
    public bool IsDebug { get; }

    public bool IsDark { get; set; }

    public CultureInfo? SelectedCulture { get; set; }

    public ObservableCollection<CultureInfo?> Cultures { get; } = [];

    public TaskbarProgressState ProgressState { get; set; } = TaskbarProgressState.None;

    public double ProgressValue { get; set; }

    public ICommand ToggleNotificationsCommand { get; }

    public ICommand ToggleFileMenuCommand { get; }

    public ICommand CloseDrawersCommand { get; }

    public ICommand ExitCommand { get; }

    public ICommand IsDarkCommand { get; }

    public ICommand IsLightCommand { get; }

    public string ProductName { get; } = ApplicationHelper.GetProductName();

    public virtual string Title => IsDebug ? $"{ProductName} [Debug]" : ProductName;

    public NotificationsViewModel NotificationsViewModel { get; }

    public IBusyService BusyService { get; }

    public MainWindowViewModelBase(
        INotificationsManager notificationsManager,
        IAppCommandsService appCommandsService,
        IBusyService mainBusyService)
    {
#if DEBUG
        IsDebug = true;
#endif

        NotificationsViewModel = new(notificationsManager);
        BusyService = mainBusyService;

        ToggleNotificationsCommand = CommandsManager.Create(() => Messenger.Default.Send(new UpdateNotificationsVisibilityRequestedMessage(VisibilityAction.Toggle)), () => !DialogManager.HasOpenedDialogs && NotificationsViewModel.Notifications.Count != 0);
        ToggleFileMenuCommand = CommandsManager.Create(() => Messenger.Default.Send(new UpdateFileMenuVisibilityRequestedMessage(VisibilityAction.Toggle)), () => !DialogManager.HasOpenedDialogs);
        CloseDrawersCommand = CommandsManager.Create(CloseDrawers, () => !DialogManager.HasOpenedDialogs);
        IsDarkCommand = CommandsManager.Create(() => IsDark = true);
        IsLightCommand = CommandsManager.Create(() => IsDark = false);
        ExitCommand = CommandsManager.Create(appCommandsService.Exit, () => !DialogManager.HasOpenedDialogs);

        Disposables.AddRange(
        [
            mainBusyService.WhenPropertyChanged(x => x.IsBusy).Subscribe(_ =>
            {
                var currentBusy = mainBusyService.GetCurrent<ProgressionBusy>();
                var progressState = mainBusyService.IsBusy ? TaskbarProgressState.Indeterminate : ProgressState == TaskbarProgressState.Error ? TaskbarProgressState.Error : TaskbarProgressState.None;
                double? progressValue = null;

                if (currentBusy != null)
                {
                    if (mainBusyService.IsBusy)
                        currentBusy.PropertyChanged += OnProgressBusyPropertyChanged;
                    else
                        currentBusy.PropertyChanged -= OnProgressBusyPropertyChanged;
                }

                RefreshTaskBarState(progressState, progressValue);
            })
        ]);

        Messenger.Default.Register<UpdateTaskBarInfoMessage>(this, UpdateTaskBarInfo);

        using (PropertyChangedSuspender.Suspend())
        {
            Cultures.AddRange(GlobalizationService.Current.SupportedCultures);
            IsDark = ThemeManager.CurrentTheme?.Base == ThemeBase.Dark;
            UpdateSelectedCulture();
        }

        ThemeManager.ThemeChanged += ThemeService_ThemeChanged;
    }

    [SuppressPropertyChangedWarnings]
    private void OnProgressBusyPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var progressionBusy = (ProgressionBusy?)sender;

        if (progressionBusy is null) return;

        switch (e.PropertyName)
        {
            case nameof(ProgressionBusy.Value):
                RefreshTaskBarState(TaskbarProgressState.Normal, progressionBusy.Value);
                break;

            case nameof(ProgressionBusy.IsCancelling):
                RefreshTaskBarState(TaskbarProgressState.Paused, progressionBusy.Value);
                break;
        }
    }

    protected virtual void CloseDrawers()
    {
        Messenger.Default.Send(new UpdateNotificationsVisibilityRequestedMessage(VisibilityAction.Hide));
        Messenger.Default.Send(new UpdateFileMenuVisibilityRequestedMessage(VisibilityAction.Hide));
    }

    #region TaskBar management

    private void UpdateTaskBarInfo(UpdateTaskBarInfoMessage obj) => RefreshTaskBarState(obj.ProgressState, obj.ProgressValue);

    private void RefreshTaskBarState(TaskbarProgressState state, double? progressValue = null)
    {
        ProgressState = state;
        if (progressValue.HasValue) ProgressValue = progressValue.Value;
    }

    #endregion

    #region Culture management

    // ReSharper disable once TailRecursiveCall
    private CultureInfo? GetSelectedCulture(CultureInfo culture) => Cultures.Contains(culture) ? culture : GetSelectedCulture(culture.Parent);

    private void UpdateSelectedCulture() => SelectedCulture = GetSelectedCulture(GlobalizationService.Current.Culture);

    protected virtual void OnSelectedCultureChanged() => GlobalizationService.Current.SetCulture(SelectedCulture?.ToString() ?? CultureInfo.InstalledUICulture.ToString());

    #endregion

    #region Theme management

    private void ThemeService_ThemeChanged(object? sender, ThemeChangedEventArgs e) => IsDark = e.CurrentTheme.Base == ThemeBase.Dark;

    protected void OnIsDarkChanged() => ThemeManager.ApplyBase(IsDark ? ThemeBase.Dark : ThemeBase.Light);

    #endregion

    protected override void Cleanup()
    {
        Messenger.Default.Unregister(this);
        NotificationsViewModel.Dispose();
        ThemeManager.ThemeChanged -= ThemeService_ThemeChanged;
        base.Cleanup();
    }
}
