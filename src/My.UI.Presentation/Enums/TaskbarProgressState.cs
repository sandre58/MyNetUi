// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.UI.Presentation.Enums
{
    //
    // Summary:
    //     Specifies the state of the progress indicator in the Windows taskbar.
    public enum TaskbarProgressState
    {
        //
        // Summary:
        //     No progress indicator is displayed in the taskbar button.
        None = 0,
        //
        // Summary:
        //     A pulsing green indicator is displayed in the taskbar button.
        Indeterminate = 1,
        //
        // Summary:
        //     A green progress indicator is displayed in the taskbar button.
        Normal = 2,
        //
        // Summary:
        //     A red progress indicator is displayed in the taskbar button.
        Error = 3,
        //
        // Summary:
        //     A yellow progress indicator is displayed in the taskbar button.
        Paused = 4
    }
}
