using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public CardManager cardManager;
    
    [SerializeField]
    public Timer timer = new Timer();
    
    public EventsGestion eventsGestion = new EventsGestion();

    
    
    
    void Start()
    {
        timer.TimeInitialization();
        eventsGestion.SetNextEventTime(timer.currentTime);
    }
    
    void Update()
    {
        Debug.Log(timer.currentTime.ToString("yyyy-MM-dd"));

        if (timer.IsCheckTime())
        {
            timer.TimeIncrement(eventsGestion);
            eventsGestion.CheckDraftEvent(timer);
            if (eventsGestion.isDraftEvent)
            {
                eventsGestion.DraftEvent(cardManager);
            }
        }
    }
}
