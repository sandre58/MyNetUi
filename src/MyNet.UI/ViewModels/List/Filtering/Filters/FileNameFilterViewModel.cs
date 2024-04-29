// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.IO;
using MyNet.Utilities.Comparaison;

namespace MyNet.UI.ViewModels.List.Filtering.Filters
{
    public class FileNameFilterViewModel : StringFilterViewModel
    {
        public FileNameFilterViewModel(string propertyName, StringOperator filterMode = StringOperator.Contains, bool caseSensitive = false)
            : base(propertyName, filterMode, caseSensitive) { }

        protected override bool IsMatchProperty(object toCompare) => base.IsMatchProperty(Path.GetFileName(toCompare?.ToString()) ?? string.Empty);

        protected override FilterViewModel CreateCloneInstance() => new FileNameFilterViewModel(PropertyName, Operator, CaseSensitive);
    }
}
