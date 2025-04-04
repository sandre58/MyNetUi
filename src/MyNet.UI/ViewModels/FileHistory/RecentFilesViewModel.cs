// -----------------------------------------------------------------------
// <copyright file="RecentFilesViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNet.UI.Services.Providers;
using MyNet.UI.ViewModels.List;

namespace MyNet.UI.ViewModels.FileHistory;

public class RecentFilesViewModel(RecentFilesProvider recentFilesProvider) : ListViewModel<RecentFileViewModel>(recentFilesProvider.Connect(),
    parametersProvider: new RecentFilesControllerProvider())
{
    protected override async void OpenCore(RecentFileViewModel item, int? selectedTab = null) => await item.OpenAsync().ConfigureAwait(false);

    public override async Task RemoveRangeAsync(IEnumerable<RecentFileViewModel> oldItems) => await Task.Run(() => oldItems.ToList().ForEach(x => x.Remove())).ConfigureAwait(false);
}
