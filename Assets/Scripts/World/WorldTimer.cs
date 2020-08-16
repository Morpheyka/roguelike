using System;
using UnityEngine;

public class WorldTimer
{
    public event Action OnDayChange;

    public uint CurrentMinutes { get; private set; } = 0;
    public uint CurrentHour { get; private set; } = 0;
    public uint CurrentDay { get; private set; } = 1;
    public uint DaysInMonth { get; private set; } = 0;
    public Month CurrentMonth => _seasons[_seasonIndex].months[_monthIndex];
    public uint MonthsInSeason { get; private set; } = 0;
    public SeasonData CurrentSeason => _seasons[_seasonIndex];
    public uint SeasonsInYear { get; private set; } = 0;
    public uint CurrentYear { get; private set; } = 1;

    private int _monthIndex = 0;
    private int _seasonIndex = 0;
    private readonly SeasonData[] _seasons = null;
    private readonly uint _minutesPerSimulate = 0;
    private const int MINUTES_PER_HOUR = 60;
    private const int HOURS_PER_DAY = 24;

    public WorldTimer(TimeData data)
    {
        _seasons = data.seasons;
        _minutesPerSimulate = data.minutesPerSimulate;

        DaysInMonth = _seasons[_seasonIndex].months[_monthIndex].daysCount;
        MonthsInSeason = (uint)_seasons[_seasonIndex].months.Length;
        SeasonsInYear = (uint)_seasons.Length;
    }

    public void Simulate()
    {
        CurrentMinutes += _minutesPerSimulate;

        if(CurrentMinutes >= MINUTES_PER_HOUR)
        {
            CurrentMinutes = 0;
            CurrentHour++;
        }

        if (CurrentHour >= HOURS_PER_DAY)
        {
            CurrentHour = 0;
            CurrentDay++;

            OnDayChange?.Invoke();
        }

        if(CurrentDay > DaysInMonth)
        {
            CurrentDay = 1;
            _monthIndex++;
        }

        if(_monthIndex > MonthsInSeason)
        {
            _monthIndex = 0;
            _seasonIndex++;
        }

        Debug.Log($"Date: {CurrentYear} - {CurrentSeason.name} - {CurrentMonth.name}" +
            $" Time: {CurrentDay} : {CurrentHour} : {CurrentMinutes}");
    }
}