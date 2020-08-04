using UnityEngine;  

public class WorldTimer
{
    public int CurrentMinutes { get; private set; } = 0;
    public int CurrentHour { get; private set; } = 0;
    public int CurrentDay { get; private set; } = 1;
    public int DaysInMonth { get; private set; }
    public int CurrentYear { get; private set; } = 1;

    private int _monthIndex = 0;
    private int _minutesPerSimulate = 0;
    private const int MINUTES_PER_HOUR = 60;
    private const int HOURS_PER_DAY = 24;

    public void Init(WorldConfig.TimeData data)
    {
        //DaysInMonth = data.months[_monthIndex].daysCount;
        _minutesPerSimulate = data.minutesPerSimulate;
    }

    public void Simulate()
    {
        CurrentMinutes += _minutesPerSimulate;

        if(CurrentMinutes >= MINUTES_PER_HOUR)
        {
            CurrentMinutes = 0;
            CurrentHour++;
        }

        if (CurrentHour > HOURS_PER_DAY)
        {
            CurrentHour = 0;
            CurrentDay++;
        }

        //Debug.Log($"Time {CurrentDay} : {CurrentHour} : {CurrentMinutes}");
    }
}