﻿// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.UI.Messages
{
    public class UpdateFileMenuVisibilityRequestedMessage(VisibilityAction visibilityAction)
    {
        public VisibilityAction VisibilityAction { get; } = visibilityAction;
    }
}
