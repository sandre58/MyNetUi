// -----------------------------------------------------------------------
// <copyright file="EnumValuesFilterViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using MyNet.Observable.Translatables;

namespace MyNet.UI.ViewModels.List.Filtering.Filters;

public class EnumValuesFilterViewModel<T>(string propertyName) : EnumValuesFilterViewModel(propertyName, typeof(T))
    where T : Enum
{
    protected override FilterViewModel CreateCloneInstance() => new EnumValuesFilterViewModel<T>(PropertyName);
}

public class EnumValuesFilterViewModel(string propertyName, Type type) : SelectedValuesFilterViewModel<EnumTranslatable<Enum>>(propertyName, Enum.GetValues(type).Cast<object>().Select(x => new EnumTranslatable((Enum)x)), "Value");
