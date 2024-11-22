using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;
using UniLaunch.Core.Rules;
using UniLaunch.UI.CodeGeneration;
using UniLaunch.UI.Controls.Rules;
using static UniLaunch.UI.Assets.Resources;

namespace UniLaunch.UI.ViewModels.Rules;

[GenerateViewModel(typeof(WeekDayRule), typeof(WeekdayRuleControl))]
public partial class WeekdayRuleViewModel
{
    public WeekdayOption[] Options =>
    [
        new WeekdayOption { Name = WeekdayRuleControlLabelMonday, Value = DayOfWeek.Monday },
        new WeekdayOption { Name = WeekdayRuleControlLabelTuesday, Value = DayOfWeek.Tuesday },
        new WeekdayOption { Name = WeekdayRuleControlLabelWednesday, Value = DayOfWeek.Wednesday },
        new WeekdayOption { Name = WeekdayRuleControlLabelThursday, Value = DayOfWeek.Thursday },
        new WeekdayOption { Name = WeekdayRuleControlLabelFriday, Value = DayOfWeek.Friday },
        new WeekdayOption { Name = WeekdayRuleControlLabelSaturday, Value = DayOfWeek.Saturday },
        new WeekdayOption { Name = WeekdayRuleControlLabelSunday, Value = DayOfWeek.Sunday },
    ];

    public ObservableCollection<DayOfWeek> DaysOfWeekToRunListProperty
    {
        get => new(_model.DaysOfWeekToRun);
        set
        {
            _model.DaysOfWeekToRun = value.Select(m => m).ToList();
            this.RaisePropertyChanged();
        }
    }

    public ICommand ToggleWeekday { get; private set; }

    partial void InitViewModel()
    {
        _propertiesToWatchForChanges = _propertiesToWatchForChanges
            .Append(nameof(DaysOfWeekToRunListProperty))
            .ToArray();
        
        ToggleWeekday = ReactiveCommand.Create<DayOfWeek>(_ToggleWeekday);
    }


    private void _ToggleWeekday(DayOfWeek day)
    {
        if (_model.DaysOfWeekToRun.Contains(day))
        {
            _model.DaysOfWeekToRun.Remove(day);
            DaysOfWeekToRunListProperty.Remove(day);
        }
        else
        {
            _model.DaysOfWeekToRun.Add(day);
            DaysOfWeekToRunListProperty.Add(day);
        }
        
        this.RaisePropertyChanged(nameof(DaysOfWeekToRunListProperty));
    }
}

public class WeekdayOption
{
    public string Name { get; init; } = null!;
    public DayOfWeek Value { get; init; }
}