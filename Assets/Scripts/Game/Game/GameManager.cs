using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public CardManager cardManager;
    public ObjectManager objectManager;
    public AgentManager agentManager;
    
    public FillMapManager fillMapManager;
    
    
    public Timer timer = new Timer();
    
    public EventsGestion eventsGestion = new EventsGestion();
    
    
    internal GameStats gameStats = new GameStats();
    public DisplayDashboard displayDashboard;
    
    
    void Start()
    {
        // Timing initialization
        timer.TimeInitialization();
        eventsGestion.SetNextDraftEventTime(timer.currentTime);
        eventsGestion.SetNextStatsEventTime(timer.currentTime);
        
        // Map initialization
        fillMapManager.GenerateMap();
        
        // Objects and Cards initialization
        objectManager.ObjectsStartInitialization();
        cardManager.CardsStartInitialization();
        gameStats.StatsStartInitialization();
        
        objectManager.SetMapInformations(fillMapManager);
        cardManager.SetCardsProperties(objectManager.listObjectsProperties);

        
        agentManager.SetMapInformations(fillMapManager);
        agentManager.SetTimerInformations(timer);
        
        // Stats and Dashboard initialization
        gameStats.UpdateObjectStatsFromObject(objectManager.GetAllObjectScripts());
        displayDashboard.UpdateFromStats(gameStats);
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
            
            eventsGestion.CheckStatsEvent(timer);
            if (eventsGestion.isStatsEvent)
            {
                eventsGestion.StatsEvent(gameStats, objectManager.GetAllObjectScripts(), displayDashboard);
                Debug.Log("Stats Finished In GM");
                eventsGestion.isStatsEvent = false;
                timer.stopTime = false;
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
        // Execute card action on map
        Actions.ExecuteCardAction(card, objectManager);
        
        // Update global and object stats
        gameStats.UpdateGlobalStatsFromCard(card);
        gameStats.UpdateObjectStatsFromObject(objectManager.GetAllObjectScripts());
        
        // Update dashboard
        displayDashboard.UpdateFromStats(gameStats);
    }
}
