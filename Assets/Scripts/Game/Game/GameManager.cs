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
    public StatManager statManager;
    public FillMapManager fillMapManager;
    
    public Timer timer = new Timer();
    public EventsGestion eventsGestion = new EventsGestion();
    
    
    void Start()
    {
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
