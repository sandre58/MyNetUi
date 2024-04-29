// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using MyNet.UI.Busy.Models;

namespace MyNet.UI.Busy
{
    public interface IBusyService : INotifyPropertyChanged
    {
        Task WaitAsync<TBusy>(Action<TBusy> action) where TBusy : class, IBusy, new();

        Task WaitAsync<TBusy>(Func<TBusy, Task> action) where TBusy : class, IBusy, new();

        TBusy Wait<TBusy>() where TBusy : class, IBusy, new();

        void Resume();

        TBusy? GetCurrent<TBusy>() where TBusy : class, IBusy;

        bool IsBusy { get; }
    }
}
