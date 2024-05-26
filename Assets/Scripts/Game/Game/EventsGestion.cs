using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class EventsGestion 
{

    public List<PeriodicEvent> periodicEvents = new List<PeriodicEvent>();
    public void EventsGestionStartInitialization()
    {
        //
    }

    public void AddEvent(PeriodicEvent periodicEvent)
    {
        periodicEvents.Add(periodicEvent);
    }
    
    public PeriodicEvent GetEventByName(string eventName)
    {
        foreach (PeriodicEvent periodicEvent in periodicEvents)
        {
            if (periodicEvent.eventName == eventName)
            {
                return periodicEvent;
            }
        }
        Debug.LogError("No event found with the name: " + eventName);
        return null;
    }
    
    public void CheckEvents(DateTime currentTime)
    {
        foreach (PeriodicEvent periodicEvent in periodicEvents)
        {
            periodicEvent.CheckToStartEvent(currentTime);
        }
    }
    
    public void SetNextDraftEventTime(DateTime currentTime)
    {
        PeriodicEvent draftEvent = GetEventByName("Draft");
        draftEvent.SetNextEventDateTime(currentTime);
    }
}
