// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DynamicData;
using MyNet.Observable.Attributes;
using MyNet.UI.ViewModels.Sorting;

namespace MyNet.UI.ViewModels.List.Sorting
{

    [CanBeValidated(false)]
    [CanSetIsModified(false)]
    public class ExtendedSortingViewModel : SortingViewModel
    {
        public ExtendedSortingViewModel(IEnumerable<string> properties) : this(properties, []) { }

        public ExtendedSortingViewModel(IEnumerable<string> properties, IEnumerable<string> defaultProperties)
            : this(new SortingPropertiesCollection(properties), defaultProperties.ToDictionary(x => x, _ => ListSortDirection.Ascending)) { }

        public ExtendedSortingViewModel(IEnumerable<string> properties, IDictionary<string, ListSortDirection> defaultProperties)
            : this(new SortingPropertiesCollection(properties), defaultProperties) { }

        public ExtendedSortingViewModel(IDictionary<string, string> properties) : this(properties, []) { }

        public ExtendedSortingViewModel(IDictionary<string, string> properties, IEnumerable<string> defaultProperties)
            : this(new SortingPropertiesCollection(properties), defaultProperties.ToDictionary(x => x, _ => ListSortDirection.Ascending)) { }

        public ExtendedSortingViewModel(IDictionary<string, string> properties, IDictionary<string, ListSortDirection> defaultProperties)
            : this(new SortingPropertiesCollection(properties), defaultProperties) { }

        public ExtendedSortingViewModel(IEnumerable<ISortingPropertyViewModel> properties, IDictionary<string, ListSortDirection> defaultProperties)
            : base(defaultProperties)
        {
            SortingProperties.AddRange(properties);
            Reset();
        }

        public override bool IsReadOnly => true;

        public override void Add(string propertyName, ListSortDirection listSortDirection = ListSortDirection.Ascending, int? order = null)
        {
            if (SortingProperties[propertyName] is ISortingPropertyViewModel property && !property.IsEnabled)
            {
                using (DeferChanged())
                {
                    property.Order = order ?? ActiveCount + 1;
                    property.IsEnabled = true;
                    property.Direction = listSortDirection;
                }
            }
        }

        public override void Remove(string propertyName)
        {
            if (SortingProperties[propertyName] is ISortingPropertyViewModel property && property.IsEnabled)
            {
                using (DeferChanged())
                {
                    property.IsEnabled = false;
                    property.Order = -1;
                }

                var list = SortingProperties.Where(x => x.IsEnabled).OrderBy(x => x.Order).ToList();
                list.ForEach(x => x.Order = list.IndexOf(x) + 1);
            }
        }

        public override void Set(IEnumerable<ISortingPropertyViewModel> properties)
        {
            using (DeferChanged())
            {
                foreach (var item in SortingProperties)
                {
                    var similarProperty = properties.FirstOrDefault(x => x.PropertyName == item.PropertyName);
                    item.IsEnabled = similarProperty?.IsEnabled ?? false;
                    item.Order = similarProperty?.Order is not int order ? -1 : order < 0 ? properties.IndexOf(similarProperty) : order;
                    item.Direction = similarProperty?.Direction ?? ListSortDirection.Ascending;
                }
            }
        }

        public override void Clear()
        {
            using (DeferChanged())
            {
                SortingProperties.ToList().ForEach(x =>
                {
                    x.IsEnabled = false;
                    x.Order = -1;
                });
            }
        }
    }
}
