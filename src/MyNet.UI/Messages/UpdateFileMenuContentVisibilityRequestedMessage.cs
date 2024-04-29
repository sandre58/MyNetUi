// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.UI.Messages
{
    public class UpdateFileMenuContentVisibilityRequestedMessage
    {
        public Type ContentType { get; }

        public VisibilityAction VisibilityAction { get; }

        public UpdateFileMenuContentVisibilityRequestedMessage(Type contentType, VisibilityAction visibilityAction) => (ContentType, VisibilityAction) = (contentType, visibilityAction);
    }
}
