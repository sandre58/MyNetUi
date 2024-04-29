// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using MyNet.UI.Extensions;

namespace MyNet.UI.Locators
{
    public static class ViewModelManager
    {
        private static IViewModelResolver? _viewModelResolver;
        private static IViewModelLocator? _viewModelLocator;

        public static IViewModelResolver ViewModelResolver => _viewModelResolver ?? throw new InvalidOperationException("Call Initialize method before use static methods.");

        public static IViewModelLocator ViewModelLocator => _viewModelLocator ?? throw new InvalidOperationException("Call Initialize method before use static methods.");

        public static T Get<T>() => ViewModelLocator.Get<T>();

        public static object Get(Type viewType) => ViewModelLocator.Get(viewType);

        public static void Initialize(IViewModelResolver viewModelResolver, IViewModelLocator viewModelLocator)
        {
            _viewModelResolver = viewModelResolver;
            _viewModelLocator = viewModelLocator;
        }

        public static object GetViewModel(Type viewType)
        {
            var viewModelType = ViewModelResolver.Resolve(viewType);

            return viewModelType is null
                ? throw new InvalidOperationException($"{viewModelType} could not be resolved.")
                : ViewModelLocator.Get(viewModelType);
        }
    }
}
