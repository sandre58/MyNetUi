﻿// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

namespace MyNet.UI.ViewModels.List.Filtering.Filters
{
    public class BooleanFilterViewModel : FilterViewModel
    {
        public BooleanFilterViewModel(string propertyName) : base(propertyName) { }

        public bool? Value { get; set; }

        public bool AllowNullValue { get; set; }

        protected override bool IsMatchProperty(object toCompare) => (bool)toCompare == Value;

        public override bool IsEmpty() => Value is null;

        public override void Reset() => Value = AllowNullValue ? null : false;

        public override void SetFrom(object? from)
        {
            if (from is BooleanFilterViewModel other)
            {
                AllowNullValue = other.AllowNullValue;
                Value = other.Value;
            }
        }

        protected override FilterViewModel CreateCloneInstance() => new BooleanFilterViewModel(PropertyName) { AllowNullValue = AllowNullValue };
    }
}
