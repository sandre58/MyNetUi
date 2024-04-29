// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using MyNet.Utilities.Geography;

namespace MyNet.UI.ViewModels.List.Filtering.Filters
{
    public class CountriesFilterViewModel : EnumerationValuesFilterViewModel<Country>
    {
        public CountriesFilterViewModel(string propertyName) : base(propertyName) { }

        protected override FilterViewModel CreateCloneInstance() => new CountriesFilterViewModel(PropertyName);
    }
}
