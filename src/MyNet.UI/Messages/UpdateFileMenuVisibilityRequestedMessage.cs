// -----------------------------------------------------------------------
// <copyright file="UpdateFileMenuVisibilityRequestedMessage.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.UI.Messages;

public class UpdateFileMenuVisibilityRequestedMessage(VisibilityAction visibilityAction)
{
    public VisibilityAction VisibilityAction { get; } = visibilityAction;
}
