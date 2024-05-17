using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public CardManager cardManager;
    
    
    public Timer timer = new Timer();
    
    public EventsGestion eventsGestion = new EventsGestion();

    
    
    
    void Start()
    {
        timer.TimeInitialization();
        eventsGestion.SetNextEventTime(timer.currentTime);
    }
    
    void Update()
    {
        

        if (!timer.stopTime && timer.IsCheckTime())
        {
            Debug.Log(timer.currentTime.ToString("yyyy-MM-dd"));
            timer.TimeIncrement(eventsGestion);
            eventsGestion.CheckDraftEvent(timer);
            if (eventsGestion.isDraftEvent)
            {
                eventsGestion.DraftEvent(cardManager);
            }
        }
        else
        {
            Debug.Log("Timer stopped");
        }
    }
    
    
}
