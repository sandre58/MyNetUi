// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using MyNet.Observable.Translatables;

namespace MyNet.UI.ViewModels.List.Filtering.Filters
{
    public class EnumValuesFilterViewModel<T> : EnumValuesFilterViewModel where T : Enum
    {
        public EnumValuesFilterViewModel(string propertyName) : base(propertyName, typeof(T)) { }

        protected override FilterViewModel CreateCloneInstance() => new EnumValuesFilterViewModel<T>(PropertyName);
    }

    public class EnumValuesFilterViewModel : SelectedValuesFilterViewModel<TranslatableEnum<Enum>>
    {
        public EnumValuesFilterViewModel(string propertyName, Type type) : base(propertyName, Enum.GetValues(type).Cast<object>().Select(x => new TranslatableEnum((Enum)x)), "Value") { }
    }
}
