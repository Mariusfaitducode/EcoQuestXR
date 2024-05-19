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
    
    
    internal GameStats gameStats = new GameStats();
    public DisplayDashboard displayDashboard;
    
    
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
        
        objectManager.SetAreas(fillMapManager.areas);
        cardManager.SetCardsProperties(objectManager.objectsProperties);
        
        // Dashboard initialization
        // TODO : Initialize objects already on map script properties and update dashboard
        displayDashboard.InitialUpdate(gameStats);
    }
    
    void Update()
    {


        if (!timer.stopTime && timer.IsCheckTime())
        {
            timer.TimeIncrement(eventsGestion);
            eventsGestion.CheckDraftEvent(timer);
            if (eventsGestion.isDraftEvent)
            {
                eventsGestion.DraftEvent(cardManager);
            }
            
            // Update dashboard
            displayDashboard.UpdateTime(timer.currentTime);
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
        gameStats.UpdateFromCard(card);
        displayDashboard.UpdateFromGameStats(gameStats);
    }
}
