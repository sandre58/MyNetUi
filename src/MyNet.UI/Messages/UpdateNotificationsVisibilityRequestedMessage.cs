// -----------------------------------------------------------------------
// <copyright file="UpdateNotificationsVisibilityRequestedMessage.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.UI.Messages;

public class UpdateNotificationsVisibilityRequestedMessage(VisibilityAction visibilityAction)
{
    public VisibilityAction VisibilityAction { get; } = visibilityAction;
}
