// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace MyNet.UI.Navigation.Models
{
    public interface INavigationParameters : IEnumerable<KeyValuePair<string, object?>>
    {
        void AddOrUpdate<T>(string key, T value);

        bool Has(string key);

        T? Get<T>(string key, T? defaultValue = default);

        void Clear();

        void Remove(string[] keys);
    }
}
