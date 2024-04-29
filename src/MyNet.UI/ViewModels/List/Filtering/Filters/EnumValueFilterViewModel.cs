// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using MyNet.Observable.Translatables;

namespace MyNet.UI.ViewModels.List.Filtering.Filters
{
    public class EnumValueFilterViewModel<T> : EnumValueFilterViewModel where T : Enum
    {
        public EnumValueFilterViewModel(string propertyName) : base(propertyName, typeof(T)) { }

        protected override FilterViewModel CreateCloneInstance() => new EnumValueFilterViewModel<T>(PropertyName);
    }

    public class EnumValueFilterViewModel : SelectedValueFilterViewModel<Enum, TranslatableEnum<Enum>>
    {
        public EnumValueFilterViewModel(string propertyName, Type type) : base(propertyName, Enum.GetValues(type).Cast<object>().Select(x => new TranslatableEnum((Enum)x))) { }
    }
}
