// Copyright (c) Stéphane ANDRE. All Right Reserved.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MyNet.Observable;
using MyNet.UI.Commands;

namespace MyNet.UI.ViewModels.Display
{
    public class DisplayViewModel : ObservableObject, IDisplayViewModel
    {
        public ObservableCollection<IDisplayMode> AllowedModes { get; }

        public IDisplayMode? Mode { get; set; }

        public ICommand SetModeCommand { get; }

        public DisplayViewModel() : this([]) { }

        public DisplayViewModel(IEnumerable<IDisplayMode> allowedModes, IDisplayMode? defaultMode = null)
        {
            AllowedModes = new ObservableCollection<IDisplayMode>(allowedModes);
            Mode = defaultMode;
            SetModeCommand = CommandsManager.CreateNotNull<IDisplayMode>(x => Mode = x);
        }

        public void SetMode<T>() where T : IDisplayMode => Mode = AllowedModes.OfType<T>().First();

        public DisplayViewModel AddMode(IDisplayMode mode, bool isDefault = false)
        {
            AllowedModes.Add(mode);

            if (isDefault)
                Mode = mode;

            return this;
        }

        public DisplayViewModel AddMode<T>(bool isDefault = false, Action<T>? action = null) where T : IDisplayMode, new()
        {
            var mode = new T();
            action?.Invoke(mode);

            return AddMode(mode, isDefault);
        }
    }
}
