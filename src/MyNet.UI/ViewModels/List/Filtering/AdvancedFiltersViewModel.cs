﻿// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MyNet.UI.Commands;
using MyNet.UI.ViewModels.Filters;
using MyNet.Utilities;

namespace MyNet.UI.ViewModels.List.Filtering
{
    public class AdvancedFiltersViewModel : FiltersViewModel
    {
        public AdvancedFiltersViewModel(IEnumerable<FilterProviderViewModel> allowedFilters)
        {
            AllowedFilters = new(allowedFilters);

            AddFilterCommand = CommandsManager.CreateNotNull<Func<IFilterViewModel>>(x => Add(x.Invoke()), _ => AllowedFilters.Count > 0);
            MoveUpCommand = CommandsManager.CreateNotNull<ICompositeFilterViewModel>(MoveUp, CanMoveUp);
            MoveDownCommand = CommandsManager.CreateNotNull<ICompositeFilterViewModel>(MoveDown, CanMoveDown);
        }

        public ObservableCollection<FilterProviderViewModel> AllowedFilters { get; }

        public ICommand MoveDownCommand { get; }

        public ICommand MoveUpCommand { get; }

        public ICommand AddFilterCommand { get; }

        public bool AddReadOnlyFilter { get; set; } = true;

        public override void Add() => Add(new SelectableCompositeFilterViewModel(AllowedFilters, null));

        public virtual void MoveUp(ICompositeFilterViewModel filter)
        {
            var index = CompositeFilters.IndexOf(filter);

            if (index > 0)
                CompositeFilters.Swap(index, index - 1);
        }

        protected virtual bool CanMoveUp(ICompositeFilterViewModel filter) => CompositeFilters.IndexOf(filter) > 0;

        public virtual void MoveDown(ICompositeFilterViewModel filter)
        {
            var index = CompositeFilters.IndexOf(filter);

            if (index > -1 && index < CompositeFilters.Count - 1)
                CompositeFilters.Swap(index, index + 1);
        }

        protected virtual bool CanMoveDown(ICompositeFilterViewModel filter) => CompositeFilters.IndexOf(filter) < CompositeFilters.Count - 1;
    }
}
