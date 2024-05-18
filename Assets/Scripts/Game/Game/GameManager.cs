using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public CardManager cardManager;
    public ObjectManager objectManager;
    
    
    public Timer timer = new Timer();
    
    public EventsGestion eventsGestion = new EventsGestion();

    
    
    private bool hasTransferredObjectsPropertiesFromOMToCM = false;
    
    
    void Start()
    {
        timer.TimeInitialization();
        eventsGestion.SetNextEventTime(timer.currentTime);
    }
    
    void Update()
    {

        // Transfers objects properties from GM to CM when CM has initialized its cards
        GameInitialisation.TransfersObjectsPropertiesToCards(cardManager, objectManager, hasTransferredObjectsPropertiesFromOMToCM);
        
        
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
    
    
    public void DraftFinished()
    {
        Debug.Log("Draft Finished In GM");
        eventsGestion.isDraftEvent = false;
        timer.stopTime = false;
    }
    
    public void ExecuteCardEvent(Card card)
    {
        Actions.ExecuteCardAction(card, objectManager);
    }
}
