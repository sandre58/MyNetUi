﻿// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using DynamicData;
using MyNet.Observable.Attributes;
using MyNet.UI.ViewModels.Grouping;

namespace MyNet.UI.ViewModels.List.Grouping
{

    [CanBeValidated(false)]
    [CanSetIsModified(false)]
    public class ExtendedGroupingViewModel : GroupingViewModel
    {
        public ExtendedGroupingViewModel(IEnumerable<string> properties, IEnumerable<string> defaultProperties)
            : this(new GroupingPropertiesCollection(properties), defaultProperties) { }

        public ExtendedGroupingViewModel(IEnumerable<string> properties)
            : this(new GroupingPropertiesCollection(properties), []) { }

        public ExtendedGroupingViewModel(IDictionary<string, string> properties) : this(properties.Select(x => new GroupingPropertyViewModel(x.Key, x.Value, x.Value)), []) { }

        public ExtendedGroupingViewModel(IEnumerable<IGroupingPropertyViewModel> properties)
            : this(properties, []) { }

        public ExtendedGroupingViewModel(IEnumerable<IGroupingPropertyViewModel> properties, IEnumerable<string> defaultProperties)
            : base(defaultProperties)
        {
            GroupingProperties.AddRange(properties);
            Reset();
        }

        public override bool IsReadOnly => true;

        public override void Add(string propertyName, string? sortingPropertyName = null, int? order = null)
        {
            if (GroupingProperties[propertyName] is IGroupingPropertyViewModel property && !property.IsEnabled)
            {
                using (DeferChanged())
                {
                    property.Order = order ?? ActiveCount + 1;
                    property.IsEnabled = true;
                }
            }
        }

        public override void Remove(string propertyName)
        {
            if (GroupingProperties[propertyName] is IGroupingPropertyViewModel property && property.IsEnabled)
            {
                using (DeferChanged())
                {
                    property.IsEnabled = false;
                    property.Order = -1;
                }

                var list = GroupingProperties.Where(x => x.IsEnabled).OrderBy(x => x.Order).ToList();
                list.ForEach(x => x.Order = list.IndexOf(x));
            }
        }

        public override void Set(IEnumerable<IGroupingPropertyViewModel> properties)
        {
            using (DeferChanged())
            {
                foreach (var item in GroupingProperties)
                {
                    var similarProperty = properties.FirstOrDefault(x => x.PropertyName == item.PropertyName);
                    item.IsEnabled = similarProperty?.IsEnabled ?? false;
                    item.Order = similarProperty?.Order is not int order ? -1 : order < 0 ? properties.IndexOf(similarProperty) : order;
                }
            }
        }

        public override void Clear()
        {
            using (DeferChanged())
            {
                GroupingProperties.ToList().ForEach(x =>
                {
                    x.IsEnabled = false;
                    x.Order = -1;
                });
            }
        }
    }
}
