// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using System.Threading.Tasks;

namespace MyNet.UI
{
    public interface IClosable
    {
        /// <summary>
        /// Gets the close event.
        /// </summary>
        event CancelEventHandler? CloseRequest;

        Task<bool> CanCloseAsync();

        void Close();
    }
}
