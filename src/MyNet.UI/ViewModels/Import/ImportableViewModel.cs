// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.Observable;
using MyNet.Observable.Attributes;

namespace MyNet.UI.ViewModels.Import
{
    public class ImportableViewModel : EditableObject
    {
        [CanBeValidated(false)]
        [CanSetIsModified(false)]
        public ImportMode Mode { get; }

        [CanBeValidated(false)]
        public bool Import { get; set; }

        public ImportableViewModel(ImportMode mode = ImportMode.Add, bool import = false)
        {
            Mode = mode;
            Import = import;
        }
    }
}
