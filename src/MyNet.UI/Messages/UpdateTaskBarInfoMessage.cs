// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.UI.Messages
{
    public class UpdateTaskBarInfoMessage(TaskbarProgressState progressState, double? progressValue = null)
    {
        public TaskbarProgressState ProgressState { get; } = progressState;

        public double? ProgressValue { get; } = progressValue;
    }
}
