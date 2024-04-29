// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using MyNet.Utilities;

namespace MyNet.UI.Navigation.Models
{
    public class NavigationParameters : INavigationParameters, ICloneable<NavigationParameters>, ISimilar<NavigationParameters>
    {
        private readonly List<KeyValuePair<string, object?>> _entries = [];

        public static NavigationParameters Empty => [];

        public NavigationParameters() { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S127:\"for\" loop stop conditions should be invariant", Justification = "False positive")]
        public NavigationParameters(string query)
        {
            if (!string.IsNullOrWhiteSpace(query))
            {
                var num = query.Length;
                for (var i = query.Length > 0 && query[0] == '?' ? 1 : 0; i < num; i++)
                {
                    var startIndex = i;
                    var num4 = -1;
                    while (i < num)
                    {
                        var ch = query[i];
                        if (ch == '=')
                            if (num4 < 0)
                                num4 = i;
                            else if (ch == '&')
                                break;

                        i++;
                    }

                    string? key = null;
                    string value;
                    if (num4 >= 0)
                    {
                        key = query.Substring(startIndex, num4);
                        value = query.Substring(num4 + 1, i - num4 - 1);
                    }
                    else
                        value = query.Substring(startIndex, i);

                    if (key is not null)
                        Add(Uri.UnescapeDataString(key), Uri.UnescapeDataString(value));
                }
            }
        }

        public int Count => _entries.Count;

        public IEnumerable<string> Keys => _entries.Select(x => x.Key);

        public object? this[string key]
        {
            get
            {
                foreach (var kvp in _entries)
                    if (string.Compare(kvp.Key, key, StringComparison.Ordinal) == 0)
                        return kvp.Value;

                return null;
            }
        }

        public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() => _entries.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(string key, object? value) => _entries.Add(new KeyValuePair<string, object?>(key, value));

        public void AddOrUpdate<T>(string key, T? value)
        {
            Remove([key]);
            Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            foreach (var kvp in _entries)
                if (string.Compare(kvp.Key, key, StringComparison.Ordinal) == 0)
                    return true;

            return false;
        }

        public bool Has(string key) => Keys.Contains(key);

        public T? Get<T>(string key, T? defaultValue = default)
        {
            foreach (var kvp in _entries)
                if (string.Compare(kvp.Key, key, StringComparison.Ordinal) == 0)
                {
                    return kvp.Value is null
                        ? defaultValue
                        : kvp.Value.GetType() == typeof(T) || typeof(T).GetTypeInfo().IsAssignableFrom(kvp.Value.GetType().GetTypeInfo())
                        ? (T)kvp.Value
                        : defaultValue;
                }

            return defaultValue;
        }

        public bool TryGetValue<T>(string key, out T? value)
        {
            foreach (var kvp in _entries)
                if (string.Compare(kvp.Key, key, StringComparison.Ordinal) == 0)
                {
                    value = kvp.Value is null
                        ? default
                        : kvp.Value.GetType() == typeof(T)
                        ? (T)kvp.Value
                        : typeof(T).GetTypeInfo().IsAssignableFrom(kvp.Value.GetType().GetTypeInfo())
                        ? (T)kvp.Value
                        : (T)Convert.ChangeType(kvp.Value, typeof(T), CultureInfo.InvariantCulture);

                    return true;
                }

            value = default;
            return false;
        }

        public IEnumerable<T?> GetValues<T>(string key)
        {
            var values = new List<T?>();

            foreach (var kvp in _entries)
                if (string.Compare(kvp.Key, key, StringComparison.Ordinal) == 0)
                    if (kvp.Value is null)
                        values.Add(default);
                    else if (kvp.Value.GetType() == typeof(T))
                        values.Add((T)kvp.Value);
                    else if (typeof(T).GetTypeInfo().IsAssignableFrom(kvp.Value.GetType().GetTypeInfo()))
                        values.Add((T)kvp.Value);
                    else
                        values.Add((T)Convert.ChangeType(kvp.Value, typeof(T), CultureInfo.InvariantCulture));

            return values.AsEnumerable();
        }

        public void Clear() => _entries.Clear();

        public void Remove(string[] keys)
        {
            var toRemove = _entries.Where(x => keys.Contains(x.Key)).ToList();
            toRemove.ForEach(x => _entries.Remove(x));
        }

        public override string ToString() => $"[{string.Join(", ", _entries.Select(x => $"{x.Key} = {x.Value}"))}]";

        public override bool Equals(object? obj) => obj is NavigationParameters p && _entries.Count == p.Count && _entries.TrueForAll(x => p.ContainsKey(x.Key) && Equals(p[x.Key], x.Value));

        public override int GetHashCode() => _entries.GetHashCode();

        public NavigationParameters Clone()
        {
            var result = new NavigationParameters();

            foreach (var item in this)
                result.Add(item.Key, item.Value);

            return result;
        }

        public bool IsSimilar(NavigationParameters? obj) => obj is not null && obj.All(x => Equals(Get<object>(x.Key), x.Value));
    }
}
