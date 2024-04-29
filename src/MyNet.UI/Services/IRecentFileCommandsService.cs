// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

namespace MyNet.UI.Services
{
    public interface IRecentFileCommandsService
    {
        Task<byte[]?> GetImageAsync(string file);

        Task OpenAsync(string file);

        Task OpenCopyAsync(string file);
    }
}
