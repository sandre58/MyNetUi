// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using MyNet.Observable;
using MyNet.Observable.Attributes;
using MyNet.UI.Busy;
using MyNet.UI.Commands;
using MyNet.UI.Extensions;
using MyNet.UI.Navigation;
using MyNet.UI.Navigation.Models;
using MyNet.UI.Threading;
using MyNet.Utilities;
using PropertyChanged;

namespace MyNet.UI.ViewModels.Workspace
{
    [CanBeValidatedForDeclaredClassOnly(false)]
    [CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
    public abstract class WorkspaceViewModel : EditableObject, IWorkspaceViewModel
    {
        public const string TabParameterKey = "Tab";

        private readonly ReadOnlyObservableCollection<INavigableWorkspaceViewModel> _subworkspaces;
        private readonly SourceCache<INavigableWorkspaceViewModel, Guid> _allSubworkspaces = new(x => x.Id);

        public event EventHandler? RefreshCompleted;

        #region Members

        public Guid Id { get; } = Guid.NewGuid();

        public bool IsLoaded { get; protected set; }

        [UpdateOnCultureChanged]
        public string? Title { get; set; }

        public bool IsEnabled { get; set; } = true;

        public ScreenMode Mode { get; set; } = ScreenMode.Read;

        public IBusyService BusyService { get; set; }

        public SubWorkspaceNavigationService NavigationService { get; }

        INavigationService IWorkspaceViewModel.NavigationService => NavigationService;

        [CanBeValidated]
        [CanSetIsModified]
        public ReadOnlyObservableCollection<INavigableWorkspaceViewModel> SubWorkspaces => _subworkspaces;

        protected IEnumerable<INavigableWorkspaceViewModel> AllWorkspaces => _allSubworkspaces.Items;

        public INavigableWorkspaceViewModel? SelectedWorkspace => NavigationService.CurrentContext?.Page as INavigableWorkspaceViewModel;

        public int SelectedWorkspaceIndex => SelectedWorkspace is not null ? _subworkspaces.IndexOf(SelectedWorkspace) : -1;

        public ICommand GoToTabCommand { get; set; }

        public ICommand GoToPreviousTabCommand { get; set; }

        public ICommand GoToNextTabCommand { get; set; }

        public ICommand RefreshCommand { get; private set; }

        public ICommand RefreshAsyncCommand { get; private set; }

        public ICommand ResetCommand { get; private set; }

        public ICommand ResetAsyncCommand { get; private set; }

        #endregion Members

        protected WorkspaceViewModel()
        {
            BusyService = BusyManager.Create();
            GoToTabCommand = CommandsManager.CreateNotNull<object>(GoToTab, CanGoToTab);
            GoToNextTabCommand = CommandsManager.Create(GoToNextTab, CanGoToNextTab);
            GoToPreviousTabCommand = CommandsManager.Create(GoToPreviousTab, CanGoToPreviousTab);
            RefreshCommand = CommandsManager.Create(Refresh, CanRefresh);
            RefreshAsyncCommand = CommandsManager.Create(async () => await RefreshAsync().ConfigureAwait(false), CanRefresh);
            ResetCommand = CommandsManager.Create(ResetWithChecking, CanReset);
            ResetAsyncCommand = CommandsManager.Create(async () => await ResetAsync().ConfigureAwait(false), CanReset);

            var obs = _allSubworkspaces.Connect();
            Disposables.AddRange([

                obs.ForEachChange(x =>
                {
                    switch(x.Reason)
                    {
                        case ChangeReason.Add:
                            x.Current.SetParentPage(this);
                            break;
                    }
                }).Subscribe(),

                obs.DisposeMany()
                   .AutoRefresh(x => x.IsEnabled)
                   .Filter(x => x.IsEnabled)
                   .ObserveOn(Scheduler.GetUIOrCurrent())
                   .Bind(out _subworkspaces)
                   .Subscribe(),
            ]);

            NavigationService = new SubWorkspaceNavigationService(this);
            NavigationService.Navigated += OnSelectedSubWorkspaceChangedCallBack;

            UpdateTitle();
        }

        protected void AddSubWorkspace(INavigableWorkspaceViewModel workspace) => AddSubWorkspaces([workspace]);

        protected void AddSubWorkspaces(IEnumerable<INavigableWorkspaceViewModel> workspaces) => _allSubworkspaces.AddOrUpdate(workspaces);

        protected void RemoveSubWorkspace(INavigableWorkspaceViewModel workspace) => _allSubworkspaces.Remove(workspace);

        protected void RemoveSubWorkspaces(IEnumerable<INavigableWorkspaceViewModel> workspaces) => _allSubworkspaces.Remove(workspaces);

        protected void ClearSubWorkspaces() => _allSubworkspaces.Clear();

        protected void SetSubWorkspaces(IEnumerable<INavigableWorkspaceViewModel> workspaces) => _allSubworkspaces.Edit(x =>
                                                                                      {
                                                                                          x.Clear();
                                                                                          x.AddOrUpdate(workspaces);
                                                                                      });

        public T? GetSubWorkspace<T>() where T : INavigableWorkspaceViewModel => AllWorkspaces.OfType<T>().FirstOrDefault();

        private void OnSelectedSubWorkspaceChangedCallBack(object? sender, NavigationEventArgs e)
        {
            RaisePropertyChanged(nameof(SelectedWorkspace));
            RaisePropertyChanged(nameof(SelectedWorkspaceIndex));

            OnSelectedSubWorkspaceChanged(new NavigatingContext(e.OldPage, e.NewPage, e.Mode, e.Parameters));
        }

        [SuppressPropertyChangedWarnings]
        protected virtual void OnSelectedSubWorkspaceChanged(NavigatingContext navigatingContext) { }

        #region Refresh

        public virtual void Refresh()
        {
            using (IsModifiedSuspender.Suspend())
            {
                OnRefreshRequested();

                RefreshCore();

                SubWorkspaces.ToList().ForEach(x => x.Refresh());

                ResetValidation();

                ResetIsModified();

                OnRefreshCompleted();

                NavigationService.CheckSelectedWorkspace();

                RefreshCompleted?.Invoke(this, EventArgs.Empty);
                IsLoaded = true;
            }
        }

        public virtual async Task RefreshAsync()
        {
            using (IsModifiedSuspender.Suspend())
            {
                OnRefreshRequested();

                await ExecuteAsync(() =>
                {
                    RefreshCore();

                    SubWorkspaces.ToList().ForEach(x => x.Refresh());

                    ResetValidation();

                    ResetIsModified();

                    OnRefreshCompleted();

                });

                NavigationService.CheckSelectedWorkspace();

                RefreshCompleted?.Invoke(this, EventArgs.Empty);
                IsLoaded = true;
            }
        }

        protected virtual async Task ExecuteAsync(Action action) => await BusyService.WaitIndeterminateAsync(action).ConfigureAwait(false);

        protected virtual void RefreshCore() { }

        protected virtual void OnRefreshRequested() { }

        protected virtual void OnRefreshCompleted() { }

        protected virtual bool CanRefresh() => true;

        #endregion Refresh

        #region GoToTab

        public virtual void GoToTab(object indexOrSubWorkspace)
        {
            if (!Equals(indexOrSubWorkspace, SelectedWorkspace) && !Equals(indexOrSubWorkspace, SelectedWorkspaceIndex))
                NavigationService.NavigateTo(indexOrSubWorkspace);
        }

        public virtual void GoToPreviousTab()
        {
            if (SelectedWorkspace is null) return;

            var currentIndex = SubWorkspaces.IndexOf(SelectedWorkspace);

            GoToTab(currentIndex - 1);
        }

        public virtual void GoToNextTab()
        {
            if (SelectedWorkspace is null) return;

            var currentIndex = SubWorkspaces.IndexOf(SelectedWorkspace);

            GoToTab(currentIndex + 1);
        }

        protected virtual bool CanGoToTab(object? indexOrSubWorkspace) => true;

        protected virtual bool CanGoToNextTab()
        {
            if (SelectedWorkspace is null) return false;

            var currentIndex = SubWorkspaces.IndexOf(SelectedWorkspace);

            return SubWorkspaces.GetByIndex(currentIndex + 1) is not null;
        }

        protected virtual bool CanGoToPreviousTab()
        {
            if (SelectedWorkspace is null) return false;

            var currentIndex = SubWorkspaces.IndexOf(SelectedWorkspace);

            return SubWorkspaces.GetByIndex(currentIndex - 1) is not null;
        }

        #endregion GoToTab

        #region Reset

        protected void ResetWithChecking()
        {
            if (CheckCanReset())
                Reset();
        }

        public void Reset()
        {
            using (IsModifiedSuspender.Suspend())
            {
                ResetCore();

                SubWorkspaces.ToList().ForEach(x => x.Reset());

                ResetIsModified();
            }
        }

        public virtual async Task ResetAsync()
        {
            using (IsModifiedSuspender.Suspend())
            {
                await ExecuteAsync(() =>
                {
                    ResetCore();

                    SubWorkspaces.ToList().ForEach(x => x.Reset());

                    ResetIsModified();
                }).ConfigureAwait(false);
            }
        }

        protected virtual void ResetCore() { }

        protected virtual bool CheckCanReset() => true;

        protected virtual bool CanReset() => true;

        #endregion Reset

        #region Culture Management

        private void LocalizationManager_CultureChanged(object? sender, EventArgs e)
        {
            GetType().GetPublicPropertiesWithAttribute<UpdateOnCultureChangedAttribute>().ForEach(x => RaisePropertyChanged(x.Name));
            OnCultureChanged();
        }

        protected virtual string CreateTitle() => string.Empty;

        protected void UpdateTitle()
        {
            var newTitle = CreateTitle();

            if (!string.IsNullOrEmpty(newTitle))
                Title = newTitle;
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            NavigationService.Navigated -= OnSelectedSubWorkspaceChangedCallBack;
            NavigationService.Dispose();
        }

        protected virtual void OnModeChanged() => UpdateTitle();

        protected override void OnCultureChanged()
        {
            base.OnCultureChanged();

            UpdateTitle();
        }
        #endregion

        public override bool Equals(object? obj) => obj is WorkspaceViewModel workspace && Id == workspace.Id;

        public override int GetHashCode() => Id.GetHashCode();
    }
}
