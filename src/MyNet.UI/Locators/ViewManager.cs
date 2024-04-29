// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.UI.Locators
{
    public static class ViewManager
    {
        private static IViewResolver? _viewResolver;
        private static IViewLocator? _viewLocator;

        public static IViewResolver ViewResolver => _viewResolver ?? throw new InvalidOperationException("Call Initialize method before use static methods.");

        public static IViewLocator ViewLocator => _viewLocator ?? throw new InvalidOperationException("Call Initialize method before use static methods.");

        public static void Initialize(IViewResolver viewResolver, IViewLocator viewLocator)
        {
            _viewResolver = viewResolver;
            _viewLocator = viewLocator;
        }

        public static object? GetNullableView(Type viewModelType)
        {
            var viewType = ViewResolver.Resolve(viewModelType);

            return viewType is null ? null : ViewLocator.Get(viewType);
        }

        public static object GetView(Type viewModelType) => GetNullableView(viewModelType) ?? throw new InvalidOperationException($"{viewModelType} could not be resolved.");

        public static object Get(Type viewType) => ViewLocator.Get(viewType);
    }
}
