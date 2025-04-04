// -----------------------------------------------------------------------
// <copyright file="RecentFilesManager.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.UI.Messages;
using MyNet.Utilities.IO.FileHistory;
using MyNet.Utilities.Logging;
using MyNet.Utilities.Messaging;

namespace MyNet.UI.Services;

public sealed class RecentFilesManager(RecentFilesService recentFilesService)
{
    public void Add(string name, string path)
    {
        using (LogManager.MeasureTime($"Add recent File : {name} | {path}", TraceLevel.Debug))
        {
            _ = recentFilesService.Add(name, path);
            Messenger.Default.Send(new RecentFilesChangedMessage());
        }
    }

    public void Remove(string file)
    {
        recentFilesService.Remove(file);

        Messenger.Default.Send(new RecentFilesChangedMessage());

        LogManager.Debug($"Recent file removed : {file}");
    }

    public void Update(string file, bool isPinned) => recentFilesService.Update(file, isPinned);
}
