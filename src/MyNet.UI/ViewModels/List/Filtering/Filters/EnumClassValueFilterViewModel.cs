// -----------------------------------------------------------------------
// <copyright file="EnumClassValueFilterViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using MyNet.Observable.Translatables;
using MyNet.Utilities;

namespace MyNet.UI.ViewModels.List.Filtering.Filters;

public class EnumClassValueFilterViewModel<T>(string propertyName) : SelectedValueFilterViewModel<EnumClass<T>, EnumClassTranslatable<T>>(propertyName, EnumClass.GetAll<T>().Select(x => new EnumClassTranslatable<T>(x)))
    where T : EnumClass<T>;

public class EnumerationValueFilterViewModel(string propertyName, Type type) : SelectedValueFilterViewModel<IEnumeration, EnumClassTranslatable<IEnumeration>>(propertyName, EnumClass.GetAll(type).Cast<IEnumeration>().Select(x => new EnumClassTranslatable(x)));
