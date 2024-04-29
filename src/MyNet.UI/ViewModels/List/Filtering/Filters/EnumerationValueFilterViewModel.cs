// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using MyNet.Observable.Translatables;
using MyNet.Utilities;

namespace MyNet.UI.ViewModels.List.Filtering.Filters
{
    public class EnumerationValueFilterViewModel<T> : SelectedValueFilterViewModel<Enumeration<T>, TranslatableEnumeration<T>>
        where T : Enumeration<T>
    {
        public EnumerationValueFilterViewModel(string propertyName) : base(propertyName, Enumeration.GetAll<T>().Select(x => new TranslatableEnumeration<T>(x))) { }
    }

    public class EnumerationValueFilterViewModel : SelectedValueFilterViewModel<IEnumeration, TranslatableEnumeration<IEnumeration>>
    {
        public EnumerationValueFilterViewModel(string propertyName, Type type) : base(propertyName, Enumeration.GetAll(type).Cast<IEnumeration>().Select(x => new TranslatableEnumeration(x))) { }
    }
}
