// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.Observable;
using MyNet.Observable.Translatables;

namespace MyNet.UI.ViewModels.List.Grouping
{
    public class GroupingPropertyViewModel : DisplayWrapper<string>, IGroupingPropertyViewModel
    {
        public GroupingPropertyViewModel(string resourceKey, string propertyName, string? sortingPropertyName = null, int order = -1)
            : this(new TranslatableString(resourceKey), propertyName, sortingPropertyName ?? propertyName, order) { }

        public GroupingPropertyViewModel(IProvideValue<string> displayName, string propertyName, string sortingPropertyName, int order = -1)
            : base(propertyName, displayName)
        {
            SortingPropertyName = sortingPropertyName;
            Order = order;
        }

        public string PropertyName => Item;

        public string SortingPropertyName { get; }

        public bool IsEnabled { get; set; }

        public int Order { get; set; }
    }
}
