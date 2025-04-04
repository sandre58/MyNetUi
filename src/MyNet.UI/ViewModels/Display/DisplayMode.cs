// -----------------------------------------------------------------------
// <copyright file="DisplayMode.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows.Input;
using MyNet.Observable.Attributes;
using MyNet.Observable.Translatables;
using MyNet.UI.Commands;
using MyNet.UI.Layout;
using MyNet.Utilities;
using MyNet.Utilities.Collections;
using MyNet.Utilities.Localization;
using MyNet.Utilities.Units;

namespace MyNet.UI.ViewModels.Display;

public class DisplayMode : StringTranslatable, IDisplayMode
{
    public virtual bool OverrideEmptySourceTemplate => false;

    public virtual bool OverrideEmptyItemsTemplate => false;

    public ICommand ResetCommand { get; }

    public DisplayMode(string key)
        : base(key) => ResetCommand = CommandsManager.Create(Reset);

    public virtual void Reset() { }
}

public class DisplayModeGrid : DisplayMode
{
    public DisplayModeGrid()
        : base(nameof(DisplayModeGrid)) { }
}

public class DisplayModeDetailled : DisplayMode
{
    public DisplayModeDetailled()
        : base(nameof(DisplayModeDetailled)) { }
}

public class DisplayModeChart : DisplayMode
{
    public DisplayModeChart()
        : base(nameof(DisplayModeChart)) { }
}

public class DisplayModeList : DisplayMode
{
    public override bool OverrideEmptyItemsTemplate => true;

    public ColumnLayoutCollection ColumnLayouts { get; }

    public OptimizedObservableCollection<DisplayWrapper<string[]>> PresetColumns { get; private set; } = [];

    public ICommand SetDisplayedColumnsCommand { get; }

    public DisplayModeList()
        : this(null) { }

    public DisplayModeList(string[]? defaultColumns)
        : base(nameof(DisplayModeList))
    {
        ColumnLayouts = new(defaultColumns);

        SetDisplayedColumnsCommand = CommandsManager.CreateNotNull<IEnumerable<string>>(ColumnLayouts.SetDisplayedColumns);
    }

    public override void Reset() => ColumnLayouts.Reset();
}

public class DisplayModeCalendar : DisplayMode
{
    private readonly TimeUnit _unit;
    private readonly int _changeValue;

    public override bool OverrideEmptySourceTemplate => true;

    public override bool OverrideEmptyItemsTemplate => true;

    public DisplayModeCalendar(TimeUnit timeUnit, int changeValue = 1, string key = nameof(DisplayModeCalendar))
        : base(key)
    {
        _unit = timeUnit;
        _changeValue = changeValue;
        DisplayDate = DateTime.Now;

        MoveToPreviousDateCommand = CommandsManager.Create(MoveToPreviousDate);
        MoveToNextDateCommand = CommandsManager.Create(MoveToNextDate);
        MoveToTodayCommand = CommandsManager.Create(MoveToToday);
    }

    [UpdateOnCultureChanged]
    public virtual DayOfWeek FirstDayOfWeek => GlobalizationService.Current.Culture.DateTimeFormat.FirstDayOfWeek;

    public DateTime DisplayDate { get; set; }

    public ICommand MoveToPreviousDateCommand { get; }

    public ICommand MoveToNextDateCommand { get; }

    public ICommand MoveToTodayCommand { get; }

    public void MoveToToday() => DisplayDate = DateTime.Now;

    public void MoveToNextDate() => DisplayDate = GetNextDate();

    public void MoveToPreviousDate() => DisplayDate = GetPreviousDate();

    public DateTime GetNextDate() => GetNextDate(DisplayDate);

    public DateTime GetPreviousDate() => GetPreviousDate(DisplayDate);

    protected virtual DateTime GetNextDate(DateTime date) => date.Add(_changeValue, _unit);

    protected virtual DateTime GetPreviousDate(DateTime date) => date.Add(-_changeValue, _unit);
}

public class DisplayModeHour : DisplayModeCalendar
{
    public DisplayModeHour()
        : base(TimeUnit.Hour, 1, nameof(DisplayModeHour)) { }
}

public class DisplayModeDay : DisplayModeCalendar
{
    public DisplayModeDay(int displayDaysCount = 1, TimeSpan? displayTimeStart = null, TimeSpan? displayTimeEnd = null)
        : base(TimeUnit.Day, 1, nameof(DisplayModeDay))
    {
        DisplayDaysCount = displayDaysCount;
        if (displayTimeStart.HasValue)
            DisplayTimeStart = displayTimeStart.Value;
        if (displayTimeEnd.HasValue)
            DisplayTimeEnd = displayTimeEnd.Value;
    }

    public int DisplayDaysCount { get; set; }

    public TimeSpan DisplayTimeStart { get; set; } = 0.Hours().TimeSpan;

    public TimeSpan DisplayTimeEnd { get; set; } = 24.Hours().TimeSpan;

    protected override DateTime GetNextDate(DateTime date) => date.Add(DisplayDaysCount, TimeUnit.Day);

    protected override DateTime GetPreviousDate(DateTime date) => date.Add(-DisplayDaysCount, TimeUnit.Day);
}

public class DisplayModeWeek : DisplayModeCalendar
{
    public DisplayModeWeek()
        : base(TimeUnit.Week, 1, nameof(DisplayModeWeek)) { }
}

public class DisplayModeMonth : DisplayModeCalendar
{
    public DisplayModeMonth()
        : base(TimeUnit.Month, 1, nameof(DisplayModeMonth)) { }
}

public class DisplayModeYear : DisplayModeCalendar
{
    public DisplayModeYear()
        : base(TimeUnit.Year, 1, nameof(DisplayModeYear)) { }
}
