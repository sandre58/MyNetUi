// -----------------------------------------------------------------------
// <copyright file="EnumValueFilterViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using MyNet.Observable.Translatables;

namespace MyNet.UI.ViewModels.List.Filtering.Filters;

public class EnumValueFilterViewModel<T>(string propertyName) : EnumValueFilterViewModel(propertyName, typeof(T))
    where T : Enum
{
    protected override FilterViewModel CreateCloneInstance() => new EnumValueFilterViewModel<T>(PropertyName);
}

public class EnumValueFilterViewModel(string propertyName, Type type) : SelectedValueFilterViewModel<Enum, EnumTranslatable<Enum>>(propertyName, Enum.GetValues(type).Cast<object>().Select(x => new EnumTranslatable((Enum)x)));
