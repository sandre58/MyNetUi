// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MyNet.Observable;
using MyNet.Observable.Attributes;
using MyNet.UI.Collections;
using MyNet.UI.Commands;
using MyNet.UI.Resources;
using MyNet.Utilities;

namespace MyNet.UI.ViewModels.Rules
{
    public partial class EditableRulesViewModel<T> : EditableObject
        where T : IEditableRule
    {
        public EditableRulesViewModel() : this([]) { }

        public EditableRulesViewModel(IEnumerable<IAvailableRule<T>> availableRules)
        {
            AvailableRules.AddRange(availableRules);

            AddCommand = CommandsManager.CreateNotNull<IAvailableRule<T>>(AddRule, x => x.IsEnabled);
            RemoveCommand = CommandsManager.CreateNotNull<T>(RemoveDateRule, x => x.CanRemove);
            MoveUpCommand = CommandsManager.CreateNotNull<T>(MoveUp, CanMoveUp);
            MoveDownCommand = CommandsManager.CreateNotNull<T>(MoveDown, CanMoveDown);
        }

        [CanSetIsModified(false)]
        [CanBeValidated(false)]
        public UiObservableCollection<IAvailableRule<T>> AvailableRules { get; } = [];

        [HasAnyItems]
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
}
