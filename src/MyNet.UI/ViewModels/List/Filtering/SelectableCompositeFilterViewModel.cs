// -----------------------------------------------------------------------
// <copyright file="SelectableCompositeFilterViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Linq;
using MyNet.Observable;
using MyNet.Utilities.Comparison;
using PropertyChanged;

namespace MyNet.UI.ViewModels.List.Filtering;

public class SelectableCompositeFilterViewModel : Wrapper<IFilterViewModel>, ICompositeFilterViewModel
{
    public SelectableCompositeFilterViewModel(ObservableCollection<FilterProviderViewModel> filters, LogicalOperator logicalOperator = LogicalOperator.And)
        : this(filters, null, logicalOperator) { }

    public SelectableCompositeFilterViewModel(ObservableCollection<FilterProviderViewModel> filters, IFilterViewModel? filter, LogicalOperator logicalOperator = LogicalOperator.And)
        : base(filter!)
    {
        Operator = logicalOperator;
        Filters = filters;
        SelectedItem = filter is not null ? Filters.First(x => Equals(x.Item().PropertyName, filter.PropertyName)) : Filters[0];
    }

    public ObservableCollection<FilterProviderViewModel> Filters { get; }

    [DoNotCheckEquality]
    public FilterProviderViewModel SelectedItem { get; set; }

    public bool IsEnabled { get; set; } = true;

    public LogicalOperator Operator { get; set; }

    protected virtual void OnSelectedItemChanged() => Item = SelectedItem.Item.Invoke();

    public void Reset()
    {
        Item.Reset();
        IsEnabled = true;
    }

    protected override Wrapper<IFilterViewModel> CreateCloneInstance(IFilterViewModel item) => new SelectableCompositeFilterViewModel(Filters, item, Operator) { IsEnabled = IsEnabled };
}
