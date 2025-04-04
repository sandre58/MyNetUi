// -----------------------------------------------------------------------
// <copyright file="IClosable.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.Threading.Tasks;

namespace MyNet.UI;

public interface IClosable
{
    /// <summary>
    /// Gets the close event.
    /// </summary>
    event CancelEventHandler? CloseRequest;

    Task<bool> CanCloseAsync();

    void Close();
}
