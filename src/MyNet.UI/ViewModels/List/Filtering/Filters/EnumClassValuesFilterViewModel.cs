// -----------------------------------------------------------------------
// <copyright file="EnumClassValuesFilterViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using MyNet.Observable.Translatables;
using MyNet.Utilities;

namespace MyNet.UI.ViewModels.List.Filtering.Filters;

public class EnumClassValuesFilterViewModel<T>(string propertyName) : SelectedValuesFilterViewModel<EnumClassTranslatable<T>>(propertyName, EnumClass.GetAll<T>().Select(x => new EnumClassTranslatable<T>(x)))
    where T : EnumClass<T>;

public class EnumerationValuesFilterViewModel(string propertyName, Type type) : SelectedValuesFilterViewModel<EnumClassTranslatable<IEnumeration>>(propertyName, EnumClass.GetAll(type).Cast<IEnumeration>().Select(x => new EnumClassTranslatable(x)));
