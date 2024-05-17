using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventsGestion 
{

    public Interval draftInterval;
    private DateTime nextEventTime;
    
    public bool isDraftEvent = false;


    public void DraftEvent(CardManager cardManager)
    {
        // Gestion coroutine time stop etc...
        
        cardManager.Draft();
    }


    public void CheckDraftEvent(Timer timer)
    {
        if (timer.currentTime >= nextEventTime)
        {
            Debug.Log("Événement déclenché le : " + timer.currentTime.ToString("yyyy-MM-dd"));

            isDraftEvent = true;
            SetNextEventTime(timer.currentTime);
            
            timer.stopTime = true;
        }
        else
        {
            isDraftEvent = false;
        }
    }
    
    
    public void SetNextEventTime(DateTime currentTime)
    {
        nextEventTime = currentTime.AddDays(draftInterval.days).AddMonths(draftInterval.months).AddYears(draftInterval.years);
        Debug.Log("Prochain événement prévu pour : " + nextEventTime.ToString("yyyy-MM-dd"));
    }
}
