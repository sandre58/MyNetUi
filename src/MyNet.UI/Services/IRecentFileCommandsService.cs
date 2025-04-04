// -----------------------------------------------------------------------
// <copyright file="IRecentFileCommandsService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;

namespace MyNet.UI.Services;

public interface IRecentFileCommandsService
{
    Task<byte[]?> GetImageAsync(string file);

    Task OpenAsync(string file);

    Task OpenCopyAsync(string file);
}
