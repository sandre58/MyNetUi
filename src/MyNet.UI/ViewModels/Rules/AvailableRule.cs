// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using MyNet.Observable.Translatables;

namespace MyNet.UI.ViewModels.Rules
{
    public class AvailableRule<T> : TranslatableString, IAvailableRule<T>
        where T : IEditableRule
    {
        private readonly Func<bool>? _canAdd;
        private readonly Func<T> _create;

        public AvailableRule(string resourceKey, Func<T> create, Func<bool>? canAdd = null) : base(resourceKey)
        {
            _canAdd = canAdd;
            _create = create;
        }

        public bool IsEnabled => _canAdd?.Invoke() ?? true;

        public virtual T Create() => _create.Invoke();
    }
}
