using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public CardManager cardManager;
    public ObjectManager objectManager;
    
    public FillMapManager fillMapManager;
    
    
    public Timer timer = new Timer();
    
    public EventsGestion eventsGestion = new EventsGestion();

    
    
    // private bool hasTransferredObjectsPropertiesFromOMToCM = false;
    
    
    void Start()
    {
        // Timing initialization
        timer.TimeInitialization();
        eventsGestion.SetNextEventTime(timer.currentTime);
        
        // Map initialization
        fillMapManager.GenerateMap();
        
        // Objects and Cards initialization
        objectManager.ObjectsStartInitialization();
        cardManager.CardsStartInitialization();
        
        objectManager.SetMapInformations(   fillMapManager);
        cardManager.SetCardsProperties(objectManager.objectsProperties);
        
        // TODO : Initialize objects already on map script properties 
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
