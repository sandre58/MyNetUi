// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.UI.Presentation.Enums;

namespace MyNet.UI.Presentation.Messages
{
    public class UpdateTaskBarInfoMessage(TaskbarProgressState progressState, double? progressValue = null)
    {
        public TaskbarProgressState ProgressState { get; } = progressState;

        public double? ProgressValue { get; } = progressValue;
    }
}
