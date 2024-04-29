// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.Observable;
using MyNet.Observable.Translatables;
using MyNet.Utilities.Comparaison;

namespace MyNet.UI.ViewModels.List.Filtering
{
    public class CompositeFilterViewModel : DisplayWrapper<IFilterViewModel>, ICompositeFilterViewModel
    {
        public bool IsEnabled { get; set; } = true;

        public LogicalOperator Operator { get; set; }

        public CompositeFilterViewModel(IFilterViewModel filter, LogicalOperator logicalOperator = LogicalOperator.And)
            : this(filter.PropertyName, filter, logicalOperator) { }

        public CompositeFilterViewModel(string resourceKey, IFilterViewModel filter, LogicalOperator logicalOperator = LogicalOperator.And)
            : this(new TranslatableString(resourceKey), filter, logicalOperator) { }

        public CompositeFilterViewModel(IProvideValue<string> displayName, IFilterViewModel filter, LogicalOperator logicalOperator = LogicalOperator.And)
            : base(filter, displayName) => Operator = logicalOperator;

        public void Reset() => Item.Reset();

        protected override Wrapper<IFilterViewModel> CreateCloneInstance(IFilterViewModel item) => new CompositeFilterViewModel(DisplayName, Item, Operator) { IsEnabled = IsEnabled };
    }
}
