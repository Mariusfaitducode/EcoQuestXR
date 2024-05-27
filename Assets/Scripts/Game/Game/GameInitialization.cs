using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInitialization
{
    public static void InitializeGame(
        Timer timer,
        FillMapManager fillMapManager,
        ObjectManager objectManager,
        CardManager cardManager,
        StatManager statManager,
        AgentManager agentManager,
        EventsGestion eventsGestion)
    {
        // ----------------- Game Initialization -----------------
        // Game time
        timer.TimeInitialization();
        
        // Map
        fillMapManager.GenerateMap();
        
        // Scripts
        
        // CSV
        objectManager.ObjectsStartInitialization();
        cardManager.CardsStartInitialization();
        
        // statManager.StatsStartInitialization();
        
        // Transfer informations to other scripts
        objectManager.SetMapInformations(fillMapManager);
        cardManager.SetCardsProperties(objectManager.listObjectsProperties);
        
        agentManager.SetMapInformations(fillMapManager);
        agentManager.SetTimerInformations(timer);
        
        // Citizens and Dashboard
        // statManager.InitDashboardEvent(objectManager);
        
        // Events
        InstantiatePeriodicEvents(eventsGestion, timer.currentTime, cardManager, statManager, objectManager);
    }
    
    public static void InstantiatePeriodicEvents(
        EventsGestion eventsGestion,
        DateTime currentTime,
        CardManager cardManager,
        StatManager statManager,
        ObjectManager objectManager)
    {
       PeriodicEvent draftEvent = new PeriodicEvent(
            "DraftEvent",
            currentTime,
            new Interval { days = 0, months = 2, years = 0 }, 
            () => cardManager.DraftEvent(),
            false);
        
        PeriodicEvent updateGlobalStatsFromObjectsEvent = new PeriodicEvent(
            "UpdateGlobalStatsFromObjectsEvent",
            currentTime,
            new Interval { days = 5, months = 0, years = 0 }, 
            () => statManager.UpdateGlobalStatsFromObjectsEvent(objectManager.GetAllObjectScripts()));
        
        PeriodicEvent updateObjectStatsFromObjectsAndCitizensEvent = new PeriodicEvent(
            "UpdateObjectStatsFromObjectsAndCitizensEvent",
            currentTime,
            new Interval { days = 1, months = 0, years = 0 }, 
            () => statManager.UpdateObjectStatsFromObjectsAndCitizensEvent(objectManager));
        
        eventsGestion.periodicEvents.Add(draftEvent);
        eventsGestion.periodicEvents.Add(updateGlobalStatsFromObjectsEvent);
        eventsGestion.periodicEvents.Add(updateObjectStatsFromObjectsAndCitizensEvent);
    }
}
