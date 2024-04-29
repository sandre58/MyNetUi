// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.UI.Selection.Models
{
    /// <summary>
    /// Services to allow changes to an entity to be selectable.
    /// </summary>
    public interface ISelectable
    {
        /// <summary>
        /// Gets or sets the selectable value.
        /// </summary>
        bool IsSelectable { get; set; }

        /// <summary>
        /// Gets or sets the selected Value.
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// Calls when selection Changed.
        /// </summary>
        event EventHandler SelectedChanged;
    }
}
