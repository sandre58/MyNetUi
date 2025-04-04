// -----------------------------------------------------------------------
// <copyright file="FileExportViewModelBase.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using MyNet.Observable.Attributes;
using MyNet.UI.Commands;
using MyNet.UI.Dialogs;
using MyNet.UI.Dialogs.Settings;
using MyNet.UI.Resources;
using MyNet.Utilities;
using MyNet.Utilities.IO.FileExtensions;

namespace MyNet.UI.ViewModels.Export;

public abstract class FileExportViewModelBase<T> : ExportViewModelBase<T>
{
    private readonly string _defaultFolder;
    private readonly Func<string> _defaultExportName;
    private readonly FileExtensionInfo _fileExtensionInfo;

    [IsFilePath(false)]
    [FolderExists]
    [Display(Name = nameof(Destination), ResourceType = typeof(UiResources))]
    public string? Destination { get; set; }

    public ICommand SetFilePathCommand { get; private set; }

    protected FileExportViewModelBase(FileExtensionInfo fileExtensionInfo,
        Func<string> defaultExportName,
        string? defaultFolder = null)
    {
        _fileExtensionInfo = fileExtensionInfo;
        _defaultExportName = defaultExportName;
        _defaultFolder = !string.IsNullOrEmpty(defaultFolder) ? defaultFolder : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        SetFilePathCommand = CommandsManager.Create(async () => await SetFilePathAsync().ConfigureAwait(false));

        ValidationRules.Add<FileExportViewModelBase<T>, string?>(x => x.Destination, MessageResources.FileHasInvalidExtensionError, x => string.IsNullOrEmpty(x) || _fileExtensionInfo.IsValid(x));
    }

    public override void Load(IEnumerable<T> items)
    {
        base.Load(items);
        var directory = Path.GetDirectoryName(Destination) ?? _defaultFolder;
        Destination = Path.Combine(directory, Path.ChangeExtension(_defaultExportName(), _fileExtensionInfo.GetDefaultExtension()));
    }

    private async Task SetFilePathAsync()
    {
        var settings = new SaveFileDialogSettings
        {
            FileName = Path.GetFileNameWithoutExtension(Destination) ?? string.Empty,
            InitialDirectory = !string.IsNullOrEmpty(Destination) ? Directory.Exists(Path.GetDirectoryName(Destination)) ? Path.GetDirectoryName(Destination) ?? string.Empty : string.Empty : string.Empty,
            Filters = _fileExtensionInfo.GetFileFilters(),
            DefaultExtension = _fileExtensionInfo.GetDefaultExtension()
        };
        var (result, filename) = await DialogManager.ShowSaveFileDialogAsync(settings).ConfigureAwait(false);

        if (result.IsTrue() && !string.IsNullOrEmpty(filename))
            Destination = filename;
    }
}
