// -----------------------------------------------------------------------
// <copyright file="UpdateFileMenuContentVisibilityRequestedMessage.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.UI.Messages;

public class UpdateFileMenuContentVisibilityRequestedMessage
{
    public Type ContentType { get; }

    public VisibilityAction VisibilityAction { get; }

    public UpdateFileMenuContentVisibilityRequestedMessage(Type contentType, VisibilityAction visibilityAction) => (ContentType, VisibilityAction) = (contentType, visibilityAction);
}
