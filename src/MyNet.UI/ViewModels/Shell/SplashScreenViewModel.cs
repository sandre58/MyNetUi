// -----------------------------------------------------------------------
// <copyright file="SplashScreenViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyNet.UI.Helpers;
using MyNet.UI.Resources;
using MyNet.UI.ViewModels.Dialogs;
using MyNet.Utilities;
using MyNet.Utilities.Logging;

namespace MyNet.UI.ViewModels.Shell;

public class SplashScreenViewModel : DialogViewModel
{
    private readonly Dictionary<Func<string>, (Func<Task> Action, Func<bool> CanExecute)> _tasks = [];

    public string? Version { get; private set; } = ApplicationHelper.GetVersion();

    public string? Message { get; private set; }

    public string? Copyright { get; private set; } = ApplicationHelper.GetCopyright();

    public string? ProductName { get; private set; } = ApplicationHelper.GetProductName();

    public string? Company { get; private set; } = ApplicationHelper.GetCompany();

    public bool IsBusy { get; private set; }

    public SplashScreenViewModel() => Message = UiResources.Ready;

    public void AddTask(Func<string> message, Func<Task> task, Func<bool>? canExecute = null) => _tasks.Add(message, (task, canExecute ?? (() => true)));

    public void AddTask(Func<string> message, Action action, Func<bool>? canExecute = null) => AddTask(message, () => Task.Run(action), canExecute);

    public void AddTask(string message, Action action, Func<bool>? canExecute = null) => AddTask(() => message, action, canExecute);

    public void AddTask(string message, Func<Task> task, Func<bool>? canExecute = null) => AddTask(() => message, task, canExecute);

    public void AddTasks(IEnumerable<(string Message, Func<Task> Task)> tasks) => tasks.ForEach(x => AddTask(x.Message, x.Task));

    public void AddTasks(IEnumerable<(string Message, Action Action)> tasks) => tasks.ForEach(x => AddTask(x.Message, x.Action));

    public void AddTasks(IEnumerable<(Func<string> Message, Func<Task> Task)> tasks) => tasks.ForEach(x => AddTask(x.Message, x.Task));

    public void AddTasks(IEnumerable<(Func<string> Message, Action Action)> tasks) => tasks.ForEach(x => AddTask(x.Message, x.Action));

    public void AddTasks(IEnumerable<(Func<string> Message, Func<Task> Task, Func<bool> CanExecute)> tasks) => tasks.ForEach(x => AddTask(x.Message, x.Task, x.CanExecute));

    public void AddTasks(IEnumerable<(Func<string> Message, Action Action, Func<bool> CanExecute)> tasks) => tasks.ForEach(x => AddTask(x.Message, x.Action, x.CanExecute));

    public async Task ExecuteAsync(Action? completedCallBack = null, Action<Exception>? failedCallback = null)
    {
        try
        {
            IsBusy = true;
            foreach (var task in _tasks)
            {
                if (!task.Value.CanExecute.Invoke()) continue;

                var message = task.Key.Invoke();
                UpdateMessage(message);

                await Task.Delay(200).ConfigureAwait(false);
                using (LogManager.MeasureTime(message, TraceLevel.Debug))
                    await task.Value.Action.Invoke().ConfigureAwait(false);
            }

            completedCallBack?.Invoke();
        }
        catch (Exception e)
        {
            failedCallback?.Invoke(e);
        }
        finally { IsBusy = false; }
    }

    /// <summary>
    /// Updates message.
    /// </summary>
    private void UpdateMessage(string message) => Message = message + "...";
}
