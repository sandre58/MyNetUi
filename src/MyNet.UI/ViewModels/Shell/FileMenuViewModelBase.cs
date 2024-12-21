// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MyNet.Observable;
using MyNet.UI.Commands;
using MyNet.UI.Dialogs;
using MyNet.UI.Dialogs.Messages;
using MyNet.UI.Messages;
using MyNet.UI.Services;
using MyNet.Utilities;
using MyNet.Utilities.Messaging;

namespace MyNet.UI.ViewModels.Shell
{
    public class FileMenuViewModelBase : ObservableObject
    {
        private readonly HashSet<IWorkspaceViewModel> _contentViewModels = [];

        public bool IsVisible { get; set; }

        public IWorkspaceViewModel? Content { get; set; }

        public ICommand ToggleFileMenuContentCommand { get; }

        public ICommand ExitCommand { get; }

        public FileMenuViewModelBase(IEnumerable<IWorkspaceViewModel> contentViewModels, IAppCommandsService appCommandsService)
        {
            _contentViewModels.AddRange(contentViewModels);

            ToggleFileMenuContentCommand = CommandsManager.CreateNotNull<Type>(ToggleContent);
            ExitCommand = CommandsManager.Create(appCommandsService.Exit, () => !DialogManager.HasOpenedDialogs);

            Messenger.Default.Register<UpdateFileMenuVisibilityRequestedMessage>(this, x => SetVisibility(x.VisibilityAction));
            Messenger.Default.Register<UpdateFileMenuContentVisibilityRequestedMessage>(this, x => SetContentVisibility(x.ContentType, x.VisibilityAction));
            Messenger.Default.Register<OpenDialogMessage>(this, OnOpenDialog);
        }

        protected void ShowContent(Type contentType)
        {
            if (!ContentIsVisible(contentType))
                Content = _contentViewModels.FirstOrDefault(contentType.IsInstanceOfType);
        }

        protected void HideContent()
        {
            if (Content != null) Content = null;
        }

        protected void ToggleContent(Type contentType)
        {
            if (!ContentIsVisible(contentType))
                ShowContent(contentType);
            else
                HideContent();
        }

        protected bool ContentIsVisible(Type contentType) => contentType.IsInstanceOfType(Content);

        protected bool ContentIsVisible<T>() => ContentIsVisible(typeof(T));

        private void SetContentVisibility(Type contentType, VisibilityAction visibilityAction)
        {
            if (visibilityAction == VisibilityAction.Show)
            {
                SetVisibility(visibilityAction);
                ShowContent(contentType);
            }
            else if (visibilityAction == VisibilityAction.Hide)
            {
                SetVisibility(visibilityAction);
                HideContent();
            }
            else
            {
                if (IsVisible)
                {
                    if (!ContentIsVisible(contentType))
                        ShowContent(contentType);
                    else
                    {
                        SetVisibility(VisibilityAction.Hide);
                        HideContent();
                    }
                }
                else
                {
                    SetVisibility(VisibilityAction.Show);
                    ShowContent(contentType);
                }
            }
        }

        private void SetVisibility(VisibilityAction visibilityAction)
            => IsVisible = visibilityAction == VisibilityAction.Toggle ? !IsVisible : visibilityAction != VisibilityAction.Hide;

        private void OnOpenDialog(OpenDialogMessage message)
        {
            if (message.Type != DialogType.FileDialog)
            {
                SetVisibility(VisibilityAction.Hide);
            }
        }

        protected virtual void OnContentChanged() { }
    }
}
