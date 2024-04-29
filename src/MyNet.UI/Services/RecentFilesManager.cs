// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.UI.Messages;
using MyNet.Utilities.IO.FileHistory;
using MyNet.Utilities.Logging;
using MyNet.Utilities.Messaging;

namespace MyNet.UI.Services
{
    public sealed class RecentFilesManager(RecentFilesService recentFilesService)
    {
        private readonly RecentFilesService _recentFilesService = recentFilesService;

        public void Add(string name, string path)
        {
            using (LogManager.MeasureTime($"Add recent File : {name} | {path}", TraceLevel.Debug))
            {
                _ = _recentFilesService.Add(name, path);
                Messenger.Default.Send(new RecentFilesChangedMessage());
            }
        }

        public void Remove(string file)
        {
            _recentFilesService.Remove(file);

            Messenger.Default.Send(new RecentFilesChangedMessage());

            LogManager.Debug($"Recent file removed : {file}");
        }

        public void Update(string file, bool isPinned) => _recentFilesService.Update(file, isPinned);
    }
}
