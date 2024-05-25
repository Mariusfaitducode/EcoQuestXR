using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicEvent
{
    public string eventName;
    
    public DateTime startDateTime;
    public DateTime endDateTime;
    public Interval interval;
    
    private DateTime nextEventDateTime;
    public bool isEventActive = false;
    
    private bool autoEnd;
    private Action eventAction;
    
    public PeriodicEvent(string eventName, DateTime startDateTime, Interval interval, Action action, bool autoEnd = true)
    {
        this.eventName = eventName;
        this.startDateTime = startDateTime;
        this.interval = interval;
        this.autoEnd = autoEnd;
        SetNextEventDateTime(startDateTime);
        eventAction = action;
    }

    public void CheckToStartEvent(DateTime currentTime)
    {
        if (currentTime >= nextEventDateTime)
        {
            eventAction.Invoke();
            isEventActive = true;

            if (autoEnd)
            {
                EndEvent(currentTime);
            }
        }
    }
    
    public void EndEvent(DateTime currentTime)
    {
        Debug.Log(eventName + " ended");
        isEventActive = false;
        SetNextEventDateTime(currentTime);
    }
    
    public void SetNextEventDateTime(DateTime currentTime)
    {
        nextEventDateTime = currentTime.AddDays(interval.days).AddMonths(interval.months).AddYears(interval.years);
    }
}
