// -----------------------------------------------------------------------
// <copyright file="ExtendedFiltersViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using MyNet.Observable.Translatables;
using MyNet.Utilities;
using PropertyChanged;

namespace MyNet.UI.ViewModels.List.Filtering;

public class ExtendedFiltersViewModel : FiltersViewModel
{
    private ReadOnlyCollection<IFilterViewModel> _defaultFilters;

    public override bool IsReadOnly => true;

    public bool HasDefaultFilters => _defaultFilters.Count > 0;

    public SpeedFiltersViewModel SpeedFilters { get; }

    public AdvancedFiltersViewModel AdvancedFilters { get; }

    public ObservableCollection<DisplayWrapper<IEnumerable<IFilterViewModel>>> PresetFilters { get; } = [];

    public DisplayWrapper<IEnumerable<IFilterViewModel>>? SelectedPresetFilter { get; set; }

    public ExtendedFiltersViewModel(IDictionary<string, Func<IFilterViewModel>> allowedFilters, SpeedFiltersViewModel speedFilters, IEnumerable<IFilterViewModel>? defaultFilters = null)
        : this(allowedFilters.Select(x => new FilterProviderViewModel(x.Key, x.Value)), speedFilters, defaultFilters) { }

    public ExtendedFiltersViewModel(IEnumerable<FilterProviderViewModel> allowedFilters, SpeedFiltersViewModel speedFilters, IEnumerable<IFilterViewModel>? defaultFilters = null)
    {
        _defaultFilters = (defaultFilters ?? []).ToList().AsReadOnly();
        SpeedFilters = speedFilters;
        AdvancedFilters = new(allowedFilters);

        Disposables.AddRange(
        [
            System.Reactive.Linq.Observable.FromEventPattern<FiltersChangedEventArgs>(x => SpeedFilters.FiltersChanged += x, x => SpeedFilters.FiltersChanged -= x)
                .Merge(System.Reactive.Linq.Observable.FromEventPattern<FiltersChangedEventArgs>(x => AdvancedFilters.FiltersChanged += x, x => AdvancedFilters.FiltersChanged -= x))
                .Subscribe(_ => OnSubFiltersChanged())
        ]);
    }

    [SuppressPropertyChangedWarnings]
    private void OnSubFiltersChanged()
    {
        using (Defer())
            CompositeFilters.Set(SpeedFilters.Concat(AdvancedFilters));
    }

    public override void Add() => AdvancedFilters.Add();

    public override void Clear()
    {
        using (Defer())
        {
            SpeedFilters.ToList().ForEach(x => x.Reset());
            AdvancedFilters.Clear();
        }
    }

    public override void Set(IEnumerable<IFilterViewModel> filters)
    {
        using (Defer())
        {
            Clear();

            foreach (var filter in filters)
            {
                var similarSpeedFilter = SpeedFilters.FirstOrDefault(x => filter.IsSimilar(x.Item));

                if (similarSpeedFilter is not null)
                    similarSpeedFilter.Item.SetFrom(filter);
                else
                    AdvancedFilters.Add(filter);
            }
        }
    }

    public override void Reset() => Set(_defaultFilters.Select(x => x.Clone<IFilterViewModel>()));

    public void SetDefaultFilters(IEnumerable<IFilterViewModel> defaultFilters) => _defaultFilters = defaultFilters.ToList().AsReadOnly();
}
