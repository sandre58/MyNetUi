﻿// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using MyNet.Utilities.Helpers;

namespace MyNet.UI.Locators
{
    /// <summary>
    /// Base class for all locators. This class implements the shared logic so only custom logic has to
    /// be implemented by new locator classes.
    /// </summary>
    public abstract class ResolverBase : IResolver
    {
        #region Fields

        private readonly Dictionary<string, HashSet<string>> _cache = [];

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        protected ResolverBase() => NamingConventions = new List<string>(GetDefaultNamingConventionsInternal());

        private IEnumerable<string> GetDefaultNamingConventionsInternal() => GetDefaultNamingConventions();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the naming conventions to use to locate types.
        /// <para/>
        /// By adding or removing conventions to this property, the service can use custom resolving of types.
        /// <para/>
        /// Each implementation should add its own default naming convention.
        /// </summary>
        /// <value>The naming conventions.</value>
        /// <remarks></remarks>
        public List<string> NamingConventions { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Registers the specified type in the local cache. This cache will also be used by the <see cref="Resolve"/>
        /// method.
        /// </summary>
        /// <param name="valueToResolve">The value to resolve.</param>
        /// <param name="resolvedValue">The resolved value.</param>
        /// <exception cref="ArgumentException">The <paramref name="valueToResolve"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentException">The <paramref name="resolvedValue"/> is <c>null</c> or whitespace.</exception>
        protected void Register(string valueToResolve, string resolvedValue) => AddItemToCache(valueToResolve, resolvedValue);

        /// <summary>
        /// Resolves the specified values. It uses both the <see cref="NamingConventions"/> and the manually registered
        /// values registered via the <see cref="Register"/> method to resolve the value.
        /// </summary>
        /// <param name="valueToResolve">The value to resolve.</param>
        /// <returns>A list of resolved values (can contain multiple items).</returns>
        /// <remarks>
        /// This method can be overriden to implement custom behavior. Don't forget to register the value using the 
        /// <see cref="Register"/> method if the result should be cached in a custom implementation.
        /// <para />
        /// By default, this value will assume the <paramref name="valueToResolve"/> is a type and will cast it as so. If the 
        /// <paramref name="valueToResolve"/> is not a type, override this method and register the result using the <see cref="Register"/>
        /// method manually.
        /// </remarks>
        /// <exception cref="ArgumentException">The <paramref name="valueToResolve"/> is <c>null</c> or whitespace.</exception>
        protected virtual IEnumerable<string> ResolveValues(string valueToResolve)
        {
            lock (_cache)
            {
                if (_cache.TryGetValue(valueToResolve, out var existingSet) && existingSet.Count > 0)
                {
                    return existingSet;
                }

                Type? resolvedType = null;

                var assembly = TypeHelper.GetAssemblyName(valueToResolve);
                var typeToResolveName = TypeHelper.GetTypeName(valueToResolve);

                foreach (var namingConvention in NamingConventions)
                {
                    var resolvedTypeName = ResolveNamingConvention(assembly, typeToResolveName, namingConvention);
                    resolvedType = TypeHelper.GetTypeFrom(resolvedTypeName);
                    if (resolvedType is not null)
                    {
                        break;
                    }
                }

                var fullResolvedTypeName = (resolvedType is not null && resolvedType.AssemblyQualifiedName is not null) ? TypeHelper.GetTypeNameWithAssembly(resolvedType.AssemblyQualifiedName) : null;

                var newSet = new HashSet<string>();

                if (!string.IsNullOrEmpty(fullResolvedTypeName))
                {
                    newSet.Add(fullResolvedTypeName!);
                    _cache.Add(valueToResolve, newSet);
                }

                return newSet;
            }
        }

        /// <summary>
        /// Resolves the specified value. It uses both the <see cref="NamingConventions"/> and the manually registered
        /// values registered via the <see cref="Register"/> method to resolve the value.
        /// </summary>
        /// <param name="valueToResolve">The value to resolve.</param>
        /// <returns>The resolved value or <c>null</c> if the value could not be resolved.</returns>
        /// <remarks>
        /// This method can be overriden to implement custom behavior. Don't forget to register the value using the 
        /// <see cref="Register"/> method if the result should be cached in a custom implementation.
        /// <para />
        /// By default, this value will assume the <paramref name="valueToResolve"/> is a type and will cast it as so. If the 
        /// <paramref name="valueToResolve"/> is not a type, override this method and register the result using the <see cref="Register"/>
        /// method manually.
        /// </remarks>
        /// <exception cref="ArgumentException">The <paramref name="valueToResolve"/> is <c>null</c> or whitespace.</exception>
        protected virtual string? Resolve(string valueToResolve)
        {
            var values = ResolveValues(valueToResolve);
            return values.LastOrDefault();
        }

        /// <summary>
        /// Gets the item from the cache.
        /// </summary>
        /// <param name="valueToResolve">The value to resolve.</param>
        /// <returns>The item or <c>null</c> if the item was not found in the cache.</returns>
        /// <exception cref="ArgumentException">The <paramref name="valueToResolve"/> is <c>null</c> or whitespace.</exception>
        protected string? GetItemFromCache(string valueToResolve)
        {
            lock (_cache)
            {
                return _cache.TryGetValue(valueToResolve, out var existingSet) && existingSet.Count > 0 ? existingSet.Last() : null;
            }
        }

        /// <summary>
        /// Adds the item to the cache.
        /// </summary>
        /// <param name="valueToResolve">The value to resolve.</param>
        /// <param name="resolvedValue">The resolved value.</param>
        /// <exception cref="ArgumentException">The <paramref name="valueToResolve"/> is <c>null</c> or whitespace.</exception>
        protected void AddItemToCache(string valueToResolve, string resolvedValue)
        {
            lock (_cache)
            {

                if (!_cache.TryGetValue(valueToResolve, out var set))
                {
                    set = [];

                    _cache[valueToResolve] = set;
                }

                set.Add(resolvedValue);
            }
        }

        /// <summary>
        /// Clears the cache of the resolved naming conventions.
        /// </summary>
        /// <remarks>
        /// Note that clearing the cache will also clear all manually registered values registered via the 
        /// <see cref="Register"/> method.
        /// </remarks>
        public void ClearCache()
        {
            lock (_cache)
            {
                _cache.Clear();
            }
        }

        /// <summary>
        /// Resolves a single naming convention.
        /// <para />
        /// This method is abstract because each locator should or could use its own naming convention to resolve
        /// the type. The <see cref="Resolve"/> method has prepared all the values such as the assembly name and the
        /// only thing this method has to do is to actually resolve a string value based on the specified naming convention.
        /// </summary>
        /// <param name="assembly">The assembly name.</param>
        /// <param name="typeToResolveName">The full type name of the type to resolve.</param>
        /// <param name="namingConvention">The naming convention to use for resolving.</param>
        /// <returns>The resolved naming convention.</returns>
        protected abstract string ResolveNamingConvention(string assembly, string typeToResolveName, string namingConvention);

        /// <summary>
        /// Gets the default naming conventions.
        /// </summary>
        /// <returns>An enumerable of default naming conventions.</returns>
        protected abstract IEnumerable<string> GetDefaultNamingConventions();

        #endregion
    }
}
