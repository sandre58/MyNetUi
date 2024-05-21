// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
using MyNet.Observable.Attributes;
using MyNet.Observable.Translatables;
using MyNet.UI.Commands;
using MyNet.UI.Resources;
using MyNet.UI.Selection.Models;
using MyNet.Utilities;
using MyNet.Utilities.Exceptions;
using MyNet.Utilities.IO.FileExtensions;

namespace MyNet.UI.ViewModels.Export
{
    internal abstract class FileExportByColumnsViewModelBase<T, TColumnMapping> : FileExportViewModelBase<T>
        where TColumnMapping : TranslatableString
    {
        private readonly IDictionary<TColumnMapping, bool> _defaultColumns;

        public bool ShowHeaderColumnTraduction { get; set; } = true;

        public ICollection<DisplayWrapper<ICollection<string>>> PresetColumns { get; }

        public bool? AreAllSelected
        {
            get
            {
                var selected = Columns.Select(item => item.IsSelected).Distinct().ToList();
                return selected.Count == 1 ? selected.Single() : null;
            }
            set
            {
                if (value.HasValue)
                    Columns.ForEach(x => x.IsSelected = value.Value);
            }
        }

        public ICommand SetSelectedColumnsCommand { get; private set; }

        [HasAnyItems]
        [Display(Name = nameof(Columns), ResourceType = typeof(UiResources))]
        public ObservableCollection<SelectedWrapper<TColumnMapping>> Columns { get; }

        protected FileExportByColumnsViewModelBase(FileExtensionInfo fileExtensionInfo,
                                                   Func<string> defaultExportName,
                                                   IDictionary<TColumnMapping, bool> defaultColumns,
                                                   IEnumerable<DisplayWrapper<ICollection<string>>>? presetColumns = null,
                                                   string? defaultFolder = null)
            : base(fileExtensionInfo, defaultExportName, defaultFolder)
        {
            _defaultColumns = defaultColumns;
            Columns = new(_defaultColumns.Select(x => new SelectedWrapper<TColumnMapping>(x.Key, x.Value)));
            PresetColumns = presetColumns?.ToList() ?? [];

            SetSelectedColumnsCommand = CommandsManager.CreateNotNull<ICollection<string>>(SetSelectedColumns);

            Disposables.Add(Columns.ToObservableChangeSet(x => x.Id).WhenPropertyChanged(x => x.IsSelected).Subscribe(_ => RaisePropertyChanged(nameof(AreAllSelected))));
        }

        private void SetSelectedColumns(IEnumerable<string> columnNames)
            => Columns.ForEach(x => x.IsSelected = columnNames.Contains(x.Item.Key));

        protected override async Task ExportAndCloseAsync()
        {
            if (!Columns.Any(x => x.IsSelected))
                throw new TranslatableException(UiResources.ExportNoColumnsError);

            await base.ExportAndCloseAsync().ConfigureAwait(false);
        }

        private void SetColumnsOrder(IEnumerable<string> columnKeys)
        {
            var newIndex = 0;
            foreach (var key in columnKeys)
            {
                var currentIndex = Columns.Select(x => x.Item.Key).ToList().IndexOf(key);

                if (currentIndex > -1)
                {
                    Columns.Move(currentIndex, newIndex);
                    newIndex++;
                }
            }
        }

        protected override void ResetCore() => SetColumnsOrder(_defaultColumns.Keys.Select(x => x.Key).ToList());
    }
}
