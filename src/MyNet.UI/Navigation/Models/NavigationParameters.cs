// -----------------------------------------------------------------------
// <copyright file="NavigationParameters.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using MyNet.Utilities;

namespace MyNet.UI.Navigation.Models;

public class NavigationParameters : INavigationParameters, ICloneable<NavigationParameters>, ISimilar<NavigationParameters>
{
    private readonly List<KeyValuePair<string, object?>> _entries = [];

    public static NavigationParameters Empty => [];

    public NavigationParameters() { }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S127:\"for\" loop stop conditions should be invariant", Justification = "False positive")]
    public NavigationParameters(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return;
        var num = query.Length;
        for (var i = query.Length > 0 && query[0] == '?' ? 1 : 0; i < num; i++)
        {
            var startIndex = i;
            var num4 = -1;
            while (i < num)
            {
                var ch = query[i];
                if (ch == '=')
                {
                    if (num4 < 0)
                        num4 = i;
                }
                else if (ch == '&')
                {
                    break;
                }

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
            {
                value = query.Substring(startIndex, i);
            }

            if (key is not null)
                Add(Uri.UnescapeDataString(key), Uri.UnescapeDataString(value));
        }
    }

    public int Count => _entries.Count;

    public IEnumerable<string> Keys => _entries.Select(x => x.Key);

    public object? this[string key] => _entries.FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.Ordinal)).Value;

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() => _entries.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(string key, object? value) => _entries.Add(new KeyValuePair<string, object?>(key, value));

    public void AddOrUpdate<T>(string key, T? value)
    {
        Remove([key]);
        Add(key, value);
    }

    public bool ContainsKey(string key) => _entries.Any(kvp => string.Equals(kvp.Key, key, StringComparison.Ordinal));

    public bool Has(string key) => Keys.Contains(key);

    public T? Get<T>(string key, T? defaultValue = default)
    {
        var item = _entries.FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.Ordinal));

        return item.Value is null
            ? defaultValue
            : item.Value.GetType() == typeof(T) || typeof(T).GetTypeInfo().IsAssignableFrom(item.Value.GetType().GetTypeInfo())
                ? (T)item.Value
                : defaultValue;
    }

    public bool TryGetValue<T>(string key, out T? value)
    {
        var item = _entries.FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.Ordinal));

        value = item.Value is null
            ? default
            : item.Value.GetType() == typeof(T) || typeof(T).GetTypeInfo().IsAssignableFrom(item.Value.GetType().GetTypeInfo())
                ? (T)item.Value
                : (T)Convert.ChangeType(item.Value, typeof(T), CultureInfo.InvariantCulture);

        return !Equals(value, null);
    }

    public IEnumerable<T?> GetValues<T>(string key)
    {
        var values = new List<T?>();
        foreach (var value in _entries.Where(kvp => string.Equals(kvp.Key, key, StringComparison.Ordinal)).Select(x => x.Value))
        {
            if (value is null)
                values.Add(default);
            else if (value.GetType() == typeof(T))
                values.Add((T)value);
            else if (typeof(T).GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo()))
                values.Add((T)value);
            else
                values.Add((T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture));
        }

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
