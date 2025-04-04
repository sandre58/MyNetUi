// -----------------------------------------------------------------------
// <copyright file="INavigationParameters.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace MyNet.UI.Navigation.Models;

public interface INavigationParameters : IEnumerable<KeyValuePair<string, object?>>
{
    void AddOrUpdate<T>(string key, T value);

    bool Has(string key);

    T? Get<T>(string key, T? defaultValue = default);

    void Clear();

    void Remove(string[] keys);
}
