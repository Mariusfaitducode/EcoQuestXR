using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GameInitialization
{
    public static void setupControlMode(GameManager gameManager)
    {
        if (gameManager.controlMode == ControlMode.ovr)
        {
            // OVR activation
            gameManager.oculusParent.SetActive(true);
            gameManager.keyboardParent.SetActive(false);
            
            // Camera setup for canvas (UI detection)
            gameManager.canvasObjects.deckCanvas.worldCamera = gameManager.ovrObjects.centerEyeAnchor.GetComponent<Camera>();
            gameManager.canvasObjects.draftCanvas.worldCamera = gameManager.ovrObjects.centerEyeAnchor.GetComponent<Camera>();
            gameManager.canvasObjects.dashboardCanvas.worldCamera = gameManager.ovrObjects.centerEyeAnchor.GetComponent<Camera>();
            
            // Unable GraphicRaycaster for the keyboard
            gameManager.cardObjects.card.GetComponent<GraphicRaycaster>().enabled = false;
            gameManager.cardObjects.card.GetComponent<OVRRaycaster>().enabled = true;
            
            gameManager.cardObjects.grabbableCard.GetComponentInChildren<GraphicRaycaster>().enabled = false;
            gameManager.cardObjects.grabbableCard.GetComponentInChildren<OVRRaycaster>().enabled = true;
            
            // Remove play button
            gameManager.keyboardObjects.play.SetActive(false);
        }
        else
        {
            // Keyboard activation
            gameManager.oculusParent.SetActive(false);
            gameManager.keyboardParent.SetActive(true);
            
            // Camera setup for canvas (UI detection)
            gameManager.canvasObjects.deckCanvas.worldCamera = gameManager.keyboardObjects.camera.GetComponent<Camera>();
            gameManager.canvasObjects.draftCanvas.worldCamera = gameManager.keyboardObjects.camera.GetComponent<Camera>();
            gameManager.canvasObjects.dashboardCanvas.worldCamera = gameManager.keyboardObjects.camera.GetComponent<Camera>();
            gameManager.cardObjects.grabbableCard.GetComponentInChildren<Canvas>().worldCamera = gameManager.keyboardObjects.camera.GetComponent<Camera>();
            
            // Unable OVR Raycaster for OVR
            gameManager.cardObjects.card.GetComponent<GraphicRaycaster>().enabled = true;
            gameManager.cardObjects.card.GetComponent<OVRRaycaster>().enabled = false;
            
            gameManager.cardObjects.grabbableCard.GetComponentInChildren<GraphicRaycaster>().enabled = true;
            gameManager.cardObjects.grabbableCard.GetComponentInChildren<OVRRaycaster>().enabled = false;
            
            // Set listener for the grabbable card
            gameManager.cardObjects.grabbableCard.GetComponentInChildren<Button>().onClick.AddListener(() => gameManager.cardManager.SelectUnselectEvent(gameManager.cardManager.grabbableCardPrefab.GetComponentInChildren<DisplayCard>()));
            
            // Set listener for the play button
            gameManager.keyboardObjects.play.GetComponent<Button>().onClick.AddListener(() => gameManager.cardManager.PlayEvent());
            
        }
    }
    
    public static void InitManager(GameManager gameManager)
    {
        
        gameManager.cardManager = GameObject.FindObjectOfType<CardManager>();
        gameManager.cardManager.gameManager = gameManager;
        gameManager.cardManager.deck = gameManager.otherObjects.leftArmDeck;
        gameManager.cardManager.deckCanvas = gameManager.canvasObjects.deckCanvas;
        gameManager.cardManager.draftCanvas = gameManager.canvasObjects.draftCanvas;
        gameManager.cardManager.cardPrefab = gameManager.cardObjects.card;
        gameManager.cardManager.grabbableCardPrefab = gameManager.cardObjects.grabbableCard;
        
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
        
        // Deck controller
        gameManager.deckController = GameObject.FindObjectOfType<DeckController>();
        
        gameManager.deckController.leftControllerAnchor = gameManager.ovrObjects.leftControllerAnchor.transform;
        gameManager.deckController.centerEyeAnchor = gameManager.ovrObjects.centerEyeAnchor.transform;
        
        gameManager.deckController.controlMode = gameManager.controlMode;
        gameManager.deckController.InitializeDeckController();
        
        // Dashboard controller
        gameManager.dashboardController = GameObject.FindObjectOfType<DashboardController>();
        
        gameManager.dashboardController.meshTransform = gameManager.otherObjects.mesh.transform;
        gameManager.dashboardController.centerEyeAnchorTransform = gameManager.ovrObjects.centerEyeAnchor.transform;
        
        gameManager.dashboardController.controlMode = gameManager.controlMode;
        gameManager.dashboardController.InitializeDashboardController();
        
        // Draft controller
        gameManager.draftController = GameObject.FindObjectOfType<DraftController>();
        
        gameManager.draftController.meshTransform = gameManager.otherObjects.mesh.transform;
        gameManager.draftController.centerEyeAnchorTransform = gameManager.ovrObjects.centerEyeAnchor.transform;
        
        gameManager.draftController.controlMode = gameManager.controlMode;
        gameManager.draftController.InitializeDraftController();
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
