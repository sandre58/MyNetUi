// -----------------------------------------------------------------------
// <copyright file="FilterProviderViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Observable.Translatables;

namespace MyNet.UI.ViewModels.List.Filtering;

public class FilterProviderViewModel(string resourceKey, Func<IFilterViewModel> item) : DisplayWrapper<Func<IFilterViewModel>>(item, resourceKey);
