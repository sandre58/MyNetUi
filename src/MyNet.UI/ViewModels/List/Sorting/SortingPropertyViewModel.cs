// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using MyNet.Observable;
using MyNet.Observable.Translatables;

namespace MyNet.UI.ViewModels.List.Sorting
{
    public class SortingPropertyViewModel : DisplayWrapper<string>, ISortingPropertyViewModel
    {
        public SortingPropertyViewModel(string propertyName, ListSortDirection direction = ListSortDirection.Ascending, int order = -1)
            : this(propertyName, propertyName, direction, order) { }

        public SortingPropertyViewModel(string resourceKey, string propertyName, ListSortDirection direction = ListSortDirection.Ascending, int order = -1)
            : this(new TranslatableString(resourceKey), propertyName, direction, order) { }

        public SortingPropertyViewModel(IProvideValue<string> displayName, string propertyName, ListSortDirection direction = ListSortDirection.Ascending, int order = -1)
            : base(propertyName, displayName)
        {
            Direction = direction;
            Order = order;
        }

        public string PropertyName => Item;

        public ListSortDirection Direction { get; set; }

        public bool IsEnabled { get; set; } = true;

        public int Order { get; set; }

        protected override DisplayWrapper<string> CreateCloneInstance(string item) => new SortingPropertyViewModel(DisplayName, item, Direction) { IsEnabled = IsEnabled, Order = Order };
    }
}
