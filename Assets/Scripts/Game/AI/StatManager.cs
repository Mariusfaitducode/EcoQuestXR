using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public struct GlobalStats
{
    internal int currentMoneyInBank;
    internal int currentEnergyInStock;
    internal int overallEcologyRate;
    internal int overallPopulationAcceptationRate;
}

public class StatManager : MonoBehaviour
{
    public GameManager gameManager;
    public DisplayDashboard displayDashboard;
    public int initialMoneyInBank = 0;
    public int initialEnergyInStock = 0;
    
    internal Stat objectsStats = new Stat();
    internal GlobalStats globalStats = new GlobalStats();
    
    internal CitizensGestion citizensGestion = new CitizensGestion();
    
    public void StatsStartInitialization()
    {
        // Initialize the global stats and objects stats
        objectsStats.Reset();
        globalStats.currentMoneyInBank = initialMoneyInBank;
        globalStats.currentEnergyInStock = initialEnergyInStock;
        
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
        UpdateDashboard();
    }
    
    private void UpdateDashboard()
    {
        displayDashboard.UpdateFromStats(globalStats, objectsStats);
    }
}
