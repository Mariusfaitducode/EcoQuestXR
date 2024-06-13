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


public class GameManager : MonoBehaviour
{
    
    public ControlMode controlMode = ControlMode.ovr;

    public GameObject oculusParent;
    public GameObject keyboardParent;
    
    public Timer timer = new Timer();
    public EventsGestion eventsGestion = new EventsGestion();
    
    // OVR
    public OvrObjects ovrObjects;
    
    // Map Generator

    internal MapGenerator mapGenerator;
    
    
    // Manager
    internal CardManager cardManager;
    internal ObjectManager objectManager;
    internal AgentManager agentManager;
    internal StatManager statManager;
    internal FillMapManager fillMapManager;

    // Controller
    internal MapController mapController;

    internal DeckController deckController;
    
    
    
    
    void Start()
    {
        if (controlMode == ControlMode.ovr)
        {
            oculusParent.SetActive(true);
            keyboardParent.SetActive(false);
        }
        else
        {
            oculusParent.SetActive(false);
            keyboardParent.SetActive(true);
        }
        
        GameInitialization.InitManager(this);
        GameInitialization.InitializeController(this);
        GameInitialization.InitializeMapGenerator(this);
        GameInitialization.InitializeGame(
            timer,
            fillMapManager,
            objectManager,
            cardManager,
            statManager,
            agentManager,
            eventsGestion);
    }


    
    
    void Update()
    {

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
        GameUtils.ExecuteCardAction(card, objectManager, statManager);
    }
}
