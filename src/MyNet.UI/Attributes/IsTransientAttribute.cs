// -----------------------------------------------------------------------
// <copyright file="IsTransientAttribute.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.UI.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class IsTransientAttribute : Attribute;
