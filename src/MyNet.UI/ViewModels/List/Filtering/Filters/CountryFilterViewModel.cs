// -----------------------------------------------------------------------
// <copyright file="CountryFilterViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Utilities.Geography;

namespace MyNet.UI.ViewModels.List.Filtering.Filters;

public class CountryFilterViewModel(string propertyName) : EnumClassValueFilterViewModel<Country>(propertyName)
{
    protected override FilterViewModel CreateCloneInstance() => new CountryFilterViewModel(PropertyName);
}
