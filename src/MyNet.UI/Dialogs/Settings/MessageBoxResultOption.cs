﻿// -----------------------------------------------------------------------
// <copyright file="MessageBoxResultOption.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.UI.Dialogs.Settings;

// Summary:
//     Specifies the buttons that are displayed on a message box. Used as an argument
//     of the Overload:System.Windows.MessageBox.Show method.
public enum MessageBoxResultOption
{
    None,

    // Summary:
    //     The message box displays an OK button.
    Ok,

    // Summary:
    //     The message box displays OK and Cancel buttons.
    OkCancel,

    // Summary:
    //     The message box displays Yes, No, and Cancel buttons.
    YesNoCancel,

    // Summary:
    //     The message box displays Yes and No buttons.
    YesNo
}
