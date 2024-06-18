using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;


public enum ControlMode
{
    ovr, keyboard
}

[Serializable]
public struct OvrObjects
{
    public GameObject leftControllerAnchor;
    public GameObject ovrCameraRig;
    public GameObject centerEyeAnchor;
}

[Serializable]
public struct KeyboardObjects
{
    public GameObject camera;
    public GameObject play;
}

[Serializable]
public struct CanvasObjects
{
    public Canvas deckCanvas;
    public Canvas draftCanvas;
    public Canvas dashboardCanvas;
    public Canvas settingsCanvas; 
}

[Serializable]
public struct CardObjects
{
    public GameObject card;
    public GameObject grabbableCard;
}

[Serializable]
public struct OtherObjects
{
    public GameObject leftArmDeck;
    public GameObject mesh;
    public GameObject cloud;
    public GameObject draftCounter;
}

public class GameManager : MonoBehaviour
{
    
    public ControlMode controlMode;

    public GameObject oculusParent;
    public GameObject keyboardParent;
    
    public Timer timer = new Timer();
    public EventsGestion eventsGestion = new EventsGestion();
    
    // OVR
    public OvrObjects ovrObjects;
    
    // Keyboard
    public KeyboardObjects keyboardObjects;
    
    // Canvas
    public CanvasObjects canvasObjects;
    
    // Card
    public CardObjects cardObjects;
    
    // Other
    public OtherObjects otherObjects;
    
    // Map Generator

    internal MapGenerator mapGenerator;
    
    
    // Manager
    internal CardManager cardManager;
    internal ObjectManager objectManager;
    internal AgentManager agentManager;
    internal StatManager statManager;
    internal FillMapManager fillMapManager;
    // internal AudioManager audioManager;
    internal AnimationManager animationManager;
    

    // Controller
    internal MapController mapController;
    internal DeckController deckController;
    //internal DraftController draftController;
    //internal DashboardController dashboardController;
    internal CloudController cloudController;
    internal SettingsController settingsController;
    internal CanvasController canvasController;


    void Start()
    {
        GameInitialization.setupControlMode(this);
        GameInitialization.InitManager(this);
        GameInitialization.InitializeController(this);
        GameInitialization.InitializeMapGenerator(this);
        GameInitialization.InitializeGame(this);

        UpdateTerrainRenderer updateTerrainRenderer = FindObjectOfType<UpdateTerrainRenderer>();
        updateTerrainRenderer.SetObjectsVisibility(fillMapManager);
        updateTerrainRenderer.SetRoadsVisibility(fillMapManager);


    }


    
    
    void Update()
    {
        if (controlMode == ControlMode.keyboard)
        {
            objectManager.AreasSounds(keyboardObjects.camera, timer.isTimePaused);
        }
        else
        {
            objectManager.AreasSounds(ovrObjects.centerEyeAnchor, timer.isTimePaused);
        }

        if (!timer.isTimePaused && timer.IsCheckTime())
        {
            
            timer.TimeIncrement();
            
            // Check events
            eventsGestion.CheckEvents(timer.currentTime);
            
            // Update dashboard
            statManager.UpdateDashboardTimeEvent(timer.currentTime);
        }
    }
    
    public void ExecuteCardEvent(Card card)
    {
        // GameActions.ExecuteCardAction(card, objectManager, statManager, audioManager);
        GameActions.ExecuteCardAction(card, this);
    }
}
