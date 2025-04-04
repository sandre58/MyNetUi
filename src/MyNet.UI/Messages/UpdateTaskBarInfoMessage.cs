// -----------------------------------------------------------------------
// <copyright file="UpdateTaskBarInfoMessage.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.UI.Messages;

public class UpdateTaskBarInfoMessage(TaskbarProgressState progressState, double? progressValue = null)
{
    public TaskbarProgressState ProgressState { get; } = progressState;

    public double? ProgressValue { get; } = progressValue;
}
