// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.UI.Locators
{
    public class ViewModelLocator(IServiceProvider serviceProvider) : IViewModelLocator
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public object Get(Type viewModelType) => _serviceProvider.GetService(viewModelType) ?? Activator.CreateInstance(viewModelType)!;
    }
}
