// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using MyNet.Observable.Attributes;
using PropertyChanged;

namespace MyNet.UI.ViewModels.Workspace
{
    [CanBeValidatedForDeclaredClassOnly(false)]
    [CanSetIsModifiedAttributeForDeclaredClassOnly(false)]
    public class SubItemViewModel<T> : NavigableWorkspaceViewModel, IItemViewModel<T>
    {
        protected CompositeDisposable? ItemSubscriptions { get; private set; }

        [DoNotCheckEquality]
        public T? Item { get; private set; }

        public Subject<T?> ItemChanged { get; } = new();

        protected virtual void OnItemChanged()
        {
            ItemSubscriptions?.Dispose();
            ItemSubscriptions = [];

            ItemChanged.OnNext(Item);
        }


        public override void SetParentPage(IWorkspaceViewModel parentPage)
        {
            base.SetParentPage(parentPage);

            var itemViewModelParent = parentPage as IItemViewModel<T>;

            if (itemViewModelParent is not null)
                Disposables.Add(itemViewModelParent.ItemChanged.Subscribe(x => Item = x));
        }

        protected override void Cleanup()
        {
            base.Cleanup();

            ItemSubscriptions?.Dispose();
        }
    }
}
