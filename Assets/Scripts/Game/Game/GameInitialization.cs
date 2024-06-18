using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
            gameManager.canvasObjects.settingsCanvas.worldCamera = gameManager.ovrObjects.centerEyeAnchor.GetComponent<Camera>();

            
            // Unable GraphicRaycaster for the keyboard
            gameManager.cardObjects.card.GetComponent<GraphicRaycaster>().enabled = false;
            gameManager.cardObjects.card.GetComponent<OVRRaycaster>().enabled = true;
            
            gameManager.cardObjects.grabbableCard.GetComponentInChildren<GraphicRaycaster>().enabled = false;
            gameManager.cardObjects.grabbableCard.GetComponentInChildren<OVRRaycaster>().enabled = true;
            
            gameManager.canvasObjects.settingsCanvas.GetComponent<GraphicRaycaster>().enabled = false;
            gameManager.canvasObjects.settingsCanvas.GetComponent<OVRRaycaster>().enabled = true;
            
            gameManager.canvasObjects.dashboardCanvas.GetComponent<GraphicRaycaster>().enabled = false;
            gameManager.canvasObjects.dashboardCanvas.GetComponent<OVRRaycaster>().enabled = true;
            
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
            gameManager.canvasObjects.settingsCanvas.worldCamera = gameManager.keyboardObjects.camera.GetComponent<Camera>();

            gameManager.cardObjects.grabbableCard.GetComponentInChildren<Canvas>().worldCamera = gameManager.keyboardObjects.camera.GetComponent<Camera>();
            
            // Unable OVR Raycaster for OVR
            gameManager.cardObjects.card.GetComponent<GraphicRaycaster>().enabled = true;
            gameManager.cardObjects.card.GetComponent<OVRRaycaster>().enabled = false;
            
            gameManager.cardObjects.grabbableCard.GetComponentInChildren<GraphicRaycaster>().enabled = true;
            gameManager.cardObjects.grabbableCard.GetComponentInChildren<OVRRaycaster>().enabled = false;
            
            gameManager.canvasObjects.settingsCanvas.GetComponent<GraphicRaycaster>().enabled = true;
            gameManager.canvasObjects.settingsCanvas.GetComponent<OVRRaycaster>().enabled = false;
            
            gameManager.canvasObjects.dashboardCanvas.GetComponent<GraphicRaycaster>().enabled = true;
            gameManager.canvasObjects.dashboardCanvas.GetComponent<OVRRaycaster>().enabled = false;
            
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
        gameManager.cardManager.depot_zone = gameManager.otherObjects.cloud;
        gameManager.cardManager.draftCounterSelectedCardsText = gameManager.otherObjects.draftCounter.GetComponent<TextMeshProUGUI>(); 


        gameManager.objectManager = GameObject.FindObjectOfType<ObjectManager>();
        gameManager.objectManager.gameManager = gameManager;
        gameManager.objectManager.mesh = gameManager.otherObjects.mesh;
        gameManager.agentManager = GameObject.FindObjectOfType<AgentManager>();
        
        gameManager.statManager = GameObject.FindObjectOfType<StatManager>();
        gameManager.statManager.gameManager = gameManager;
        
        gameManager.fillMapManager = GameObject.FindObjectOfType<FillMapManager>();
        
        // gameManager.audioManager = GameObject.FindObjectOfType<AudioManager>();
        
        gameManager.animationManager = GameObject.FindObjectOfType<AnimationManager>();
        gameManager.animationManager.gameManager = gameManager;
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
        gameManager.deckController.mesh = gameManager.otherObjects.mesh;


        gameManager.deckController.InitializeDeckController();
        
        //// Dashboard controller
        //gameManager.dashboardController = GameObject.FindObjectOfType<DashboardController>();
        
        //gameManager.dashboardController.centerEyeAnchorTransform = gameManager.ovrObjects.centerEyeAnchor.transform;
        //gameManager.dashboardController.meshTransform = gameManager.otherObjects.mesh.transform;
        
        //gameManager.dashboardController.controlMode = gameManager.controlMode;
        //gameManager.dashboardController.updateTerrainRenderer = gameManager.otherObjects.mesh.GetComponent<UpdateTerrainRenderer>();
        //gameManager.dashboardController.InitializeDashboardController();
        
        //// Draft controller
        //gameManager.draftController = GameObject.FindObjectOfType<DraftController>();
        
        //gameManager.draftController.centerEyeAnchorTransform = gameManager.ovrObjects.centerEyeAnchor.transform;
        //gameManager.draftController.meshTransform = gameManager.otherObjects.mesh.transform;
        
        //gameManager.draftController.controlMode = gameManager.controlMode;
        //gameManager.draftController.updateTerrainRenderer = gameManager.otherObjects.mesh.GetComponent<UpdateTerrainRenderer>();
        //gameManager.draftController.InitializeDraftController();
        

        // Canvas Controller 
        gameManager.canvasController = GameObject.FindObjectOfType<CanvasController>();

        gameManager.canvasController.centerEyeAnchorTransform = gameManager.ovrObjects.centerEyeAnchor.transform;        
        gameManager.canvasController.meshTransform = gameManager.otherObjects.mesh.transform;
        gameManager.canvasController.controlMode = gameManager.controlMode;
        gameManager.canvasController.updateTerrainRenderer = gameManager.otherObjects.mesh.GetComponent<UpdateTerrainRenderer>();

        gameManager.canvasController.InitializeCanvasController();


        // Settings controller
        gameManager.settingsController = GameObject.FindObjectOfType<SettingsController>();
        gameManager.settingsController.gameManager = gameManager;


        // Cloud controller
        gameManager.cloudController = GameObject.FindObjectOfType<CloudController>();

        if (gameManager.controlMode == ControlMode.keyboard)
        {
            gameManager.cloudController.GameObject().SetActive(false);
        }
        else
        {
            gameManager.cloudController.GameObject().SetActive(true);
            
            gameManager.cloudController.centerEyeAnchorTransform = gameManager.ovrObjects.centerEyeAnchor.transform;
            gameManager.cloudController.meshTransform = gameManager.otherObjects.mesh.transform;
        
            gameManager.cloudController.updateTerrainRenderer = gameManager.otherObjects.mesh.GetComponent<UpdateTerrainRenderer>();
            gameManager.cloudController.InitializeCloudController();
        }
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
    
    
    
    
    
    public static void InitializeGame(GameManager gameManager)
    {
        // ----------------- Game Initialization -----------------
        // Game time
        gameManager.timer.TimeInitialization();
        
        // Map
        gameManager.fillMapManager.GenerateMap();
        
        // Scripts
        
        // Objects / Cards Manager
        // CSV
        gameManager.objectManager.ObjectsStartInitialization();
        gameManager.cardManager.CardsStartInitialization();
        gameManager.statManager.StatsStartInitialization();
        gameManager.animationManager.AnimationsStartInitialization();
        
        // Transfer informations to other scripts
        gameManager.objectManager.SetMapInformations(gameManager.fillMapManager);
        gameManager.cardManager.SetCardsProperties(gameManager.objectManager.listObjectsProperties, gameManager.objectManager.subObjectsProperties);
        
        // Audio Source Areas
        
        // objectManager
        
        
        // Stats
        // Citizens and Dashboard
        gameManager.statManager.citizensGestion.GenerateInitialsCitizens(gameManager.objectManager.GetMaxPopSize());
        gameManager.statManager.citizensGestion.UpdateDailyTransportsStats();
        gameManager.statManager.InitDashboard(gameManager.objectManager);
        
        // Agents
        gameManager.agentManager.SetMapInformations(gameManager.fillMapManager);
        gameManager.agentManager.SetTimerInformations(gameManager.timer);
        
        gameManager.agentManager.InitAgentManager(gameManager.statManager, gameManager.objectManager, gameManager.fillMapManager);
        
        
        // Events
        InstantiatePeriodicEvents(gameManager);
        
        
        
        
    }
    
    public static void InstantiatePeriodicEvents(GameManager gameManager)
    {
       PeriodicEvent draftEvent = new PeriodicEvent(
            "DraftEvent",
            gameManager.timer.currentTime,

            new Interval { days = 5, months = 0, years = 0 }, 

            () => gameManager.cardManager.DraftEvent(),
            false);

        PeriodicEvent updateGlobalStatsFromObjectsEvent = new PeriodicEvent(
            "UpdateGlobalStatsFromObjectsEvent",
            gameManager.timer.currentTime,
            new Interval { days = 5, months = 0, years = 0 },
            () => gameManager.statManager.UpdateGlobalStatsFromObjectsEvent(gameManager.objectManager.GetAllObjectScripts()));

        PeriodicEvent updateObjectStatsFromObjectsAndCitizensEvent = new PeriodicEvent(
            "UpdateObjectStatsFromObjectsAndCitizensEvent",
            gameManager.timer.currentTime,
            new Interval { days = 1, months = 0, years = 0 }, 
            () => gameManager.statManager.DailyUpdateDashboardEvent());
        
        gameManager.eventsGestion.periodicEvents.Add(draftEvent);
        gameManager.eventsGestion.periodicEvents.Add(updateGlobalStatsFromObjectsEvent);
        gameManager.eventsGestion.periodicEvents.Add(updateObjectStatsFromObjectsAndCitizensEvent);
    }
    
    
    
}
