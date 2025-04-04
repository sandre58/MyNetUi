﻿// -----------------------------------------------------------------------
// <copyright file="MessageBoxResult.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.UI.Dialogs;

// Summary:
//     Specifies which message box button that a user clicks. System.Windows.MessageBoxResult
//     is returned by the Overload:System.Windows.MessageBox.Show method.
public enum MessageBoxResult
{
    // Summary:
    //     The message box returns no result.
    None = 0,

    // Summary:
    //     The result value of the message box is OK.
    Ok = 1,

    // Summary:
    //     The result value of the message box is Cancel.
    Cancel = 2,

    // Summary:
    //     The result value of the message box is Yes.
    Yes = 6,

    // Summary:
    //     The result value of the message box is No.
    No = 7
}
