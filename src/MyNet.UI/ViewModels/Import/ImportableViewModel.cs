// -----------------------------------------------------------------------
// <copyright file="ImportableViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Observable;
using MyNet.Observable.Attributes;

namespace MyNet.UI.ViewModels.Import;

public class ImportableViewModel(ImportMode mode = ImportMode.Add, bool import = false) : EditableObject
{
    [CanBeValidated(false)]
    [CanSetIsModified(false)]
    public ImportMode Mode { get; } = mode;

    [CanBeValidated(false)]
    public bool Import { get; set; } = import;
}
