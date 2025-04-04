// -----------------------------------------------------------------------
// <copyright file="CountriesFilterViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Utilities.Geography;

namespace MyNet.UI.ViewModels.List.Filtering.Filters;

public class CountriesFilterViewModel(string propertyName) : EnumClassValuesFilterViewModel<Country>(propertyName)
{
    protected override FilterViewModel CreateCloneInstance() => new CountriesFilterViewModel(PropertyName);
}
