using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalStats
{
    internal float currentMoneyInBank;
    internal float currentEnergyInStock;
    internal float currentEmittedCo2;
    internal float currentWasteProduced;
    
    internal float overallEcologyRate;
    internal float overallSocietyRate;
}

public class StatManager : MonoBehaviour
{
    public GameManager gameManager;
    public DisplayDashboard displayDashboard;
    
    public float initialMoneyInBank = 0;
    public float initialEnergyInStock = 0;
    public float initialEmittedCo2 = 0;
    public float initialWasteProduced = 0;
    
    public float maxCo2Emitted = 1000000f;
    public float maxWasteProduced = 1000000f;
    public float maxGreenSpaces = 1000000f;
    
    internal Stat objectsStats = new Stat();
    internal GlobalStats globalStats = new GlobalStats();
    
    internal CitizensGestion citizensGestion = new CitizensGestion();
    
    public void StatsStartInitialization()
    {
        // Initialize the global stats and objects stats
        objectsStats.Reset();
        
        globalStats.currentMoneyInBank = initialMoneyInBank;
        globalStats.currentEnergyInStock = initialEnergyInStock;
        globalStats.currentEmittedCo2 = initialEmittedCo2;
        globalStats.currentWasteProduced = initialWasteProduced;
        
        // Initialize the citizens gestion
        citizensGestion.CitizensGestionStartInitialization();
    }
    
    public void UpdateDashboardTimeEvent(DateTime currentTime)
    {
        displayDashboard.UpdateTime(currentTime);
    }
    
    public void UpdateGlobalStatsFromCardEvent(Card card)
    {
        StatUtils.UpdateGlobalStatsFromCard(globalStats, card);
        
    }

    public void UpdateObjectStatsFromObjectsAndCitizensEvent(ObjectManager objectManager)
    {
        StatUtils.UpdateObjectStatsFromObjectsAndCitizens(objectsStats, objectManager.GetAllObjectScripts(), citizensGestion);
        UpdateDashboard();
    }
    
    public void UpdateGlobalStatsFromObjectsEvent(List<ObjectScript> objects)
    {
        StatUtils.UpdateGlobalStatsFromObjects(globalStats, objects);
        UpdateDashboard();
    }
    
    public void InitDashboardEvent(ObjectManager objectManager)
    {
        citizensGestion.GenerateInitialsCitizens(objectManager.GetMaxPopSize());
        StatUtils.UpdateObjectStatsFromObjectsAndCitizens(objectsStats, objectManager.GetAllObjectScripts(), citizensGestion);
        StatUtils.UpdateGlobalStatsFromObjects(globalStats, objectManager.GetAllObjectScripts());
        UpdateDashboard();
    }
    
    private void UpdateDashboard()
    {
        StatUtils.ComputeRates(globalStats, objectsStats, maxWasteProduced, maxCo2Emitted, maxGreenSpaces);
        displayDashboard.UpdateFromStats(globalStats, objectsStats, citizensGestion.maxPopSize, citizensGestion.citizens.Count, 0.5f, citizensGestion.GetDailyTransportModeUsers());
    }
}