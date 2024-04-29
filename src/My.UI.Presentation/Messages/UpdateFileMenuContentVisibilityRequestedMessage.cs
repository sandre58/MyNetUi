// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using MyNet.UI.Presentation.Enums;

namespace MyNet.UI.Presentation.Messages
{
    public class UpdateFileMenuContentVisibilityRequestedMessage
    {
        public Type ContentType { get; }

        public VisibilityAction VisibilityAction { get; }

        public UpdateFileMenuContentVisibilityRequestedMessage(Type contentType, VisibilityAction visibilityAction) => (ContentType, VisibilityAction) = (contentType, visibilityAction);
    }
}
