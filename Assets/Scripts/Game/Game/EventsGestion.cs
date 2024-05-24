using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class EventsGestion 
{

    public Interval draftInterval;
    private DateTime nextDraftEventTime;
    public bool isDraftEvent = false;
    
    public Interval statsInterval;
    private DateTime nextStatsEventTime;
    public bool isStatsEvent = false;

    public void DraftEvent(CardManager cardManager)
    {
        cardManager.Draft();
    }
    
    public void StatsEvent(GameStats gameStats,  List<ObjectScript> objects, DisplayDashboard displayDashboard)
    {
        gameStats.UpdateGlobalStatsFromObject(objects);
        gameStats.UpdateObjectStatsFromObjectsAndCitizens(objects);
        displayDashboard.UpdateFromStats(gameStats);
    }


    public void CheckDraftEvent(Timer timer)
    {
        if (timer.currentTime >= nextDraftEventTime)
        {
            Debug.Log("Événement DRAFT déclenché le : " + timer.currentTime.ToString("yyyy-MM-dd-HH"));

            isDraftEvent = true;
            timer.stopTime = true;

            SetNextDraftEventTime(timer.currentTime);
            
        }
        else
        {
            isDraftEvent = false;
        }
    }
    
    public void CheckStatsEvent(Timer timer)
    {
        if (timer.currentTime >= nextStatsEventTime)
        {
            Debug.Log("Événement STATS déclenché le : " + timer.currentTime.ToString("yyyy-MM-dd-HH"));
            
            isStatsEvent = true;
            timer.stopTime = true;
            
            SetNextStatsEventTime(timer.currentTime);
        }
        else
        {
            isStatsEvent = false;
        }
    }
    
    
    public void SetNextDraftEventTime(DateTime currentTime)
    {
        nextDraftEventTime = currentTime.AddDays(draftInterval.days).AddMonths(draftInterval.months).AddYears(draftInterval.years);
        Debug.Log("Prochain événement DRAFT prévu pour : " + nextDraftEventTime.ToString("yyyy-MM-dd-HH"));
    }
    
    public void SetNextStatsEventTime(DateTime currentTime)
    {
        nextStatsEventTime = currentTime.AddDays(statsInterval.days).AddMonths(statsInterval.months).AddYears(statsInterval.years);
        Debug.Log("Prochain événement STATS prévu pour : " + nextStatsEventTime.ToString("yyyy-MM-dd-HH"));
    }
}
