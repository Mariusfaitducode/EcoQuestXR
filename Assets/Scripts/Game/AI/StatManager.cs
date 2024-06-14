using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalStats
{
    internal float currentMoneyInBank = 0;
    internal float currentEnergyInStock = 0;
    internal float currentEmittedCo2 = 0;
    internal float currentWasteProduced = 0;
    
    internal float overallEcologyRate = 0;
    internal float overallSocietyRate = 0;
}

public class StatManager : MonoBehaviour
{
    internal GameManager gameManager;
    public DisplayDashboard displayDashboard;
    
    public float initialMoneyInBank = 0;
    public float initialEnergyInStock = 0;
    public float initialEmittedCo2 = 0;
    public float initialWasteProduced = 0;
    
    public float maxGreenSpaces = 5000f;
    public float maxEmittedCo2 = 5000000f;
    public float maxWasteProduced = 100000f;
    public float maxCo2EmissionPerMonth = 1000000f;
    public float maxWasteProductionPerMonth = 10000f;
    
    internal Stat objectsStats = new Stat();
    internal GlobalStats globalStats = new GlobalStats();
    
    internal Stat dashboardStats = new Stat();
    
    internal CitizensGestion citizensGestion = new CitizensGestion();
    
    public void StatsStartInitialization()
    {
        // Initialize the class citizensGestion
        citizensGestion.CitizensGestionStartInitialization();
        
        // Initialize of all the stats
        // objects
        objectsStats.Reset();
        
        // global
        globalStats.currentMoneyInBank = initialMoneyInBank;
        globalStats.currentEnergyInStock = initialEnergyInStock;
        globalStats.currentEmittedCo2 = initialEmittedCo2;
        globalStats.currentWasteProduced = initialWasteProduced;
        
        // citizens
        // done
        
        
    }
    
    // Called every second to update the time on the dashboard
    public void UpdateDashboardTimeEvent(DateTime currentTime)
    {
        displayDashboard.UpdateTime(currentTime);
    }
    
    // Called by the card event to update the global stats
    public void UpdateStatsFromCardEvent(Card card)
    {
        // Update global stats : money, energy, co2, waste
        StatUtils.UpdateGlobalStatsFromCard(globalStats, card);
        
        // Update object stats depending on objects placed or removed
        StatUtils.UpdateObjectStatsFromObjects(this, gameManager.objectManager.GetAllObjectScripts());
        citizensGestion.totalHouseholds = gameManager.objectManager.GetMaxPopSize();
        
        // Update population stats depending on the card
        citizensGestion.UpdatePopulationStatsFromCard(card.stats);
        
        // Update population size if needed
        if (card.stats.populationSize != 0)
        {
            citizensGestion.UpdatePopulationSize(card.stats.populationSize);
        }
        
    }

    // Called every day to update the global stats
    public void DailyUpdateDashboardEvent()
    {
        // Get new transports users and apply transport effects on them
        citizensGestion.UpdateDailyTransportsStats();
        
        // Update global citizens stats depending on their transports
        citizensGestion.UpdateCitizensStats();
        
        // Update global stats due to pullution, energy consumption, etc.
        StatUtils.DailyUpdateGlobalStatsFromCitizens(globalStats, citizensGestion);
        
        // Don't update objects stats here no buildings are placed or removed
        
        // Update dashboard
        UpdateDashboard();
    }
    
    
    // Called by the periodic event every month to apply profits, losses, etc.
    public void UpdateGlobalStatsFromObjectsEvent(List<ObjectScript> objects)
    {
        // Update global stats
        StatUtils.UpdateGlobalStatsFromObjects(globalStats, objects);
        
        // Update dashboard
        UpdateDashboard();
    }
    
    public void InitDashboard(ObjectManager objectManager)
    {
        
        StatUtils.UpdateObjectStatsFromObjects(this, gameManager.objectManager.GetAllObjectScripts());
        StatUtils.UpdateGlobalStatsFromObjects(globalStats, objectManager.GetAllObjectScripts());
        citizensGestion.UpdateCitizensStats();
        
        UpdateDashboard();
    }
    
    private void UpdateDashboard()
    {
        // Compute dashboard objects stats which is the combination of objects stats and citizens transports stats
        StatUtils.UpdateDashboardObjectStats(dashboardStats, objectsStats, citizensGestion.dailyTransportsStats);
        
        // Compute rates
        StatUtils.ComputeRates(this);
        
        // Update the dashboard
        displayDashboard.UpdateFromStats(this);
    }
}
