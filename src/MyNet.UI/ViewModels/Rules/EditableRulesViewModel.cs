// -----------------------------------------------------------------------
// <copyright file="EditableRulesViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using MyNet.Observable;
using MyNet.Observable.Attributes;
using MyNet.Observable.Resources;
using MyNet.UI.Collections;
using MyNet.UI.Commands;
using MyNet.UI.Resources;
using MyNet.Utilities;

namespace MyNet.UI.ViewModels.Rules;

public class EditableRulesViewModel<T> : EditableObject
    where T : IEditableRule
{
    public EditableRulesViewModel(bool addValidationRuleForBlockEmptyRules = false)
        : this([], addValidationRuleForBlockEmptyRules) { }

    public EditableRulesViewModel(IEnumerable<IAvailableRule<T>> availableRules, bool addValidationRuleForBlockEmptyRules = false)
    {
        AvailableRules.AddRange(availableRules);

        AddCommand = CommandsManager.CreateNotNull<IAvailableRule<T>>(AddRule, x => x.IsEnabled);
        RemoveCommand = CommandsManager.CreateNotNull<T>(RemoveDateRule, x => x.CanRemove);
        MoveUpCommand = CommandsManager.CreateNotNull<T>(MoveUp, CanMoveUp);
        MoveDownCommand = CommandsManager.CreateNotNull<T>(MoveDown, CanMoveDown);

        if (addValidationRuleForBlockEmptyRules)
            ValidationRules.Add<EditableRulesViewModel<T>, UiObservableCollection<T>>(x => x.Rules, ValidationResources.FieldXMustBeContainOneItemAtLeastError.FormatWith(UiResources.Rules), x => x is not null && x.Count > 0);
    }

    [CanSetIsModified(false)]
    [CanBeValidated(false)]
    public UiObservableCollection<IAvailableRule<T>> AvailableRules { get; } = [];

    [Display(Name = nameof(Rules), ResourceType = typeof(UiResources))]
    public UiObservableCollection<T> Rules { get; } = [];

    public ICommand AddCommand { get; }

    public ICommand RemoveCommand { get; }

    public ICommand MoveDownCommand { get; }

    public ICommand MoveUpCommand { get; }

    protected virtual void AddRule(IAvailableRule<T> rule) => Rules.Add(rule.Create());

    protected virtual void RemoveDateRule(T rule) => Rules.Remove(rule);

    protected virtual void MoveUp(T rule)
    {
        var index = Rules.IndexOf(rule);

        if (index > 0)
            Rules.Swap(index, index - 1);
    }

    protected virtual bool CanMoveUp(T rule) => rule.CanMove && Rules.IndexOf(rule) > 0;

    protected virtual void MoveDown(T rule)
    {
        var index = Rules.IndexOf(rule);

        if (index > -1 && index < Rules.Count - 1)
            Rules.Swap(index, index + 1);
    }

    protected virtual bool CanMoveDown(T rule) => rule.CanMove && Rules.IndexOf(rule) < Rules.Count - 1;
}
