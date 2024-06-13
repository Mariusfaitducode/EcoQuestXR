using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInitialization
{
    
    public static void InitManager(GameManager gameManager)
    {
        
        gameManager.cardManager = GameObject.FindObjectOfType<CardManager>();
        gameManager.cardManager.gameManager = gameManager;
        
        gameManager.objectManager = GameObject.FindObjectOfType<ObjectManager>();
        gameManager.objectManager.gameManager = gameManager;
        
        gameManager.agentManager = GameObject.FindObjectOfType<AgentManager>();
        // agentManager.gameManager = this;
        
        gameManager.statManager = GameObject.FindObjectOfType<StatManager>();
        gameManager.statManager.gameManager = gameManager;
        
        gameManager.fillMapManager = GameObject.FindObjectOfType<FillMapManager>();
        // fillMapManager.gameManager = this;
        
    }


    public static void InitializeController(GameManager gameManager)
    {
        // Map controller
        gameManager.mapController = GameObject.FindObjectOfType<MapController>();
        gameManager.mapController.gameManager = gameManager;
        
        gameManager.mapController.ovrCameraRig = gameManager.ovrObjects.ovrCameraRig;
        gameManager.mapController.centerEyeAnchor = gameManager.ovrObjects.centerEyeAnchor;
        
        gameManager.mapController.InitializeController();
        
        // Card controller
        gameManager.deckController = GameObject.FindObjectOfType<DeckController>();
        
        gameManager.deckController.leftControllerAnchor = gameManager.ovrObjects.leftControllerAnchor.transform;
        gameManager.deckController.centerEyeAnchor = gameManager.ovrObjects.centerEyeAnchor.transform;
        
        gameManager.deckController.controlMode = gameManager.controlMode;
        gameManager.deckController.InitializeDeckController();
    }


    public static void InitializeMapGenerator(GameManager gameManager)
    {
        
        gameManager.mapGenerator = GameObject.FindObjectOfType<MapGenerator>();

        if (gameManager.controlMode == ControlMode.ovr)
        {
            // Scale = 0.007
            
            gameManager.mapGenerator.terrainData.uniformScale = 0.007f;
            
            gameManager.mapGenerator.updateTerrainRenderer.InitShaderLimitTerrain(
                gameManager.mapGenerator.terrainData.uniformScale);
            
        }
        else
        {
            gameManager.mapGenerator.terrainData.uniformScale = 0.007f;
            
            gameManager.mapGenerator.updateTerrainRenderer.InitShaderLimitTerrain(
                gameManager.mapGenerator.terrainData.uniformScale);
        }
    }
    
    
    
    
    
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
        
        statManager.StatsStartInitialization();
        
        // Transfer informations to other scripts
        objectManager.SetMapInformations(fillMapManager);
        cardManager.SetCardsProperties(objectManager.listObjectsProperties);
        
        agentManager.SetMapInformations(fillMapManager);
        agentManager.SetTimerInformations(timer);
        
        // Citizens and Dashboard
        statManager.citizensGestion.GenerateInitialsCitizens(objectManager.GetMaxPopSize());
        statManager.InitDashboard(objectManager);
        
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

            // new Interval { days = 50, months = 0, years = 0 }, 

            new Interval { days = 5, months = 0, years = 0 }, 

            () => cardManager.DraftEvent(),
            false);

        //PeriodicEvent updateGlobalStatsFromObjectsEvent = new PeriodicEvent(
        //    "UpdateGlobalStatsFromObjectsEvent",
        //    currentTime,
        //    new Interval { days = 5, months = 0, years = 0 },
        //    () => statManager.UpdateGlobalStatsFromObjectsEvent(objectManager.GetAllObjectScripts()));

        //PeriodicEvent updateObjectStatsFromObjectsAndCitizensEvent = new PeriodicEvent(
        //    "UpdateObjectStatsFromObjectsAndCitizensEvent",
        //    currentTime,
        //    new Interval { days = 1, months = 0, years = 0 }, 
        //    () => statManager.DailyUpdateDashboardEvent());
        
        eventsGestion.periodicEvents.Add(draftEvent);
        //eventsGestion.periodicEvents.Add(updateGlobalStatsFromObjectsEvent);
        //eventsGestion.periodicEvents.Add(updateObjectStatsFromObjectsAndCitizensEvent);
    }
    
    
    
}
