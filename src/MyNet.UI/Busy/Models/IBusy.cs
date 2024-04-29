// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.UI.Busy.Models
{
    public interface IBusy
    {
        bool IsCancellable { get; }

        bool IsCancelling { get; }

        bool CanCancel { get; set; }

        void Cancel();
    }
}
