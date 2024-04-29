// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;

namespace MyNet.UI.Locators
{
    public interface IViewLocator
    {
        void Register(Type type, Func<object> createInstance);

        object Get(Type viewType);
    }
}
