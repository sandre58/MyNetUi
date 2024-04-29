// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DynamicData.Binding;
using MyNet.Observable;
using MyNet.UI.ViewModels.Filters;
using PropertyChanged;

namespace MyNet.UI.ViewModels.List.Filtering.Filters
{
    public abstract class FilterViewModel : ObservableObject, IFilterViewModel, IFiltersViewModel
    {
        private static readonly string[] Separator = ["."];

        private event EventHandler<FiltersChangedEventArgs>? FiltersChanged;

        event EventHandler<FiltersChangedEventArgs>? IFiltersViewModel.FiltersChanged
        {
            add => FiltersChanged += value;
            remove => FiltersChanged -= value;
        }

        protected FilterViewModel(string propertyName)
        {
            PropertyName = propertyName;

            Disposables.Add(this.WhenAnyPropertyChanged().Subscribe(_ => OnFilterChanged()));
        }

        public string PropertyName { get; }

        public bool IsReadOnly { get; set; }

        public virtual bool IsMatch(object? target) => IsMatchInternal(target, PropertyName.Split(Separator, StringSplitOptions.RemoveEmptyEntries));

        private bool IsMatchInternal(object? target, IList<string> propertyNames)
        {
            if (target is null)
                return false;

            var toCompare = target;

            if (propertyNames.Any())
            {
                var newPropertyNames = propertyNames.ToList();

                foreach (var propertyName in propertyNames)
                {
                    var propertyInfo = toCompare?.GetType().GetProperty(propertyName);
                    if (propertyInfo is null)
                        return false;

                    toCompare = propertyInfo.GetValue(toCompare, null);

                    _ = newPropertyNames.Remove(propertyName);

                    if (newPropertyNames.Count > 0 && toCompare is IList toCompareEnumerableRecursive)
                        return toCompareEnumerableRecursive.Cast<object>().Any(x => IsMatchInternal(x, newPropertyNames));
                }
            }

            return toCompare is IList toCompareEnumerable ? IsMatchPropertyList(toCompareEnumerable.Cast<object>()) : IsMatchProperty(toCompare!);
        }

        protected virtual bool IsMatchPropertyList(IEnumerable<object> toCompareEnumerable) => toCompareEnumerable.Any(IsMatchProperty);

        protected abstract bool IsMatchProperty(object toCompare);

        public abstract bool IsEmpty();

        public abstract void Reset();

        public void Refresh() => OnFilterChanged();

        void IFiltersViewModel.Clear() => Reset();

        [SuppressPropertyChangedWarnings]
        private void OnFilterChanged() => FiltersChanged?.Invoke(this, new FiltersChangedEventArgs([new CompositeFilterViewModel(this)]));

        public override bool Equals(object? obj) => obj is FilterViewModel o && GetType() == obj.GetType() && PropertyName == o.PropertyName;

        public override int GetHashCode() => PropertyName.GetHashCode();

        public object Clone()
        {
            using (PropertyChangedSuspender.Suspend())
            {
                var newEntity = CreateCloneInstance();
                newEntity.SetFrom(this);

                return newEntity;
            }
        }

        protected abstract FilterViewModel CreateCloneInstance();

        public abstract void SetFrom(object? from);

        public virtual bool IsSimilar(IFilterViewModel? obj) => GetType() == obj?.GetType() && PropertyName == obj.PropertyName;
    }
}
