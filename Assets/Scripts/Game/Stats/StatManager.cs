using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct StructGlobalStats
{
    public float money;
    public float energy;
}

[Serializable]
public struct StructEcologyStats
{
    public float co2;
    public float waste;
    public int greenSpaces;
}

[Serializable]
public struct Population
{
    int populationSize;
}

[Serializable]
public struct MaxStats
{
    public StructGlobalStats globalStats;
    public StructEcologyStats globalEcologyStats;
    public StructEcologyStats monthlyEcologyStats;
}

[Serializable]
public struct InitialStats
{
    public StructGlobalStats globalStats;
    public StructEcologyStats ecologyStats;
}

[Serializable]
public struct CompensationStats
{
    public float co2AbsorptionPerMonth;
}

// public class GlobalStats
// {
//     internal float currentMoneyInBank = 0;
//     internal float currentEnergyInStock = 0;
//     internal float currentEmittedCo2 = 0;
//     internal float currentWasteProduced = 0;
//     
//     internal float overallEcologyRate = 0;
//     internal float overallSocietyRate = 0;
// }

public class ClassEcologyStats
{
    public AreaType areaType;
    public float co2 = 0;
    public float waste = 0;
    public int greenSpaces = 0;
    
    public void Reset()
    {
        co2 = 0;
        waste = 0;
        greenSpaces = 0;
    }
}

public class ClassGlobalStats
{
    public float money = 0;
    public float energy = 0;
    
    public void Reset()
    {
        money = 0;
        energy = 0;
    }
}

public class ClassStats
{
    // Global Rates
    public float overallEcologyRate = 0;
    public float overallSocietyRate = 0;
    
    // Current Global Stats
    public ClassGlobalStats currentGlobalStats = new ClassGlobalStats();
    // Monthly Global Stats
    public ClassGlobalStats monthlyGlobalStats = new ClassGlobalStats();
    
    // Current Ecology Stats
    public ClassEcologyStats currentEcologyStats = new ClassEcologyStats();
    // Monthly Ecology Stats
    public ClassEcologyStats monthlyEcologyStats = new ClassEcologyStats();
    
    // stats by area
    public List<ClassEcologyStats> ecologyStatsByArea = new List<ClassEcologyStats>();
}

public class StatManager : MonoBehaviour
{
    internal GameManager gameManager;
    public DisplayDashboard displayDashboard;
    
    // public float initialMoneyInBank = 0;
    // public float initialEnergyInStock = 0;
    // public float initialEmittedCo2 = 0;
    // public float initialWasteProduced = 0;
    public InitialStats initialStats;
    public MaxStats maxStats;
    
    public ClassStats stats = new ClassStats();
    internal CitizensGestion citizensGestion = new CitizensGestion();
    
    // public int maxPopSize = 1000;
    //
    // public float maxGreenSpaces = 5000f;
    // public float maxEmittedCo2 = 5000000f;
    // public float maxWasteProduced = 100000f;
    // public float maxCo2EmissionPerMonth = 1000000f;
    // public float maxWasteProductionPerMonth = 10000f;
    
    // internal Stat objectsStats = new Stat();
    // internal GlobalStats globalStats = new GlobalStats();
    
    // internal Stat dashboardStats = new Stat();
    
    
    
    public void StatsStartInitialization()
    {
        // Initialize the class citizensGestion
        citizensGestion.CitizensGestionStartInitialization();
        
        // Initialize of all the stats
        stats.currentGlobalStats.money = initialStats.globalStats.money;
        stats.currentGlobalStats.energy = initialStats.globalStats.energy;
        stats.currentEcologyStats.co2 = initialStats.ecologyStats.co2;
        stats.currentEcologyStats.waste = initialStats.ecologyStats.waste;
        
        // Create the stats by area
        foreach (AreaType areaType in Enum.GetValues(typeof(AreaType)))
        {
            ClassEcologyStats ecologyStats = new ClassEcologyStats();
            ecologyStats.areaType = areaType;
            stats.ecologyStatsByArea.Add(ecologyStats);
        }
        // // objects
        // objectsStats.Reset();
        //
        // // global
        // globalStats.currentMoneyInBank = initialStats.globalStats.money;
        // globalStats.currentEnergyInStock = initialStats.globalStats.energy;
        // globalStats.currentEmittedCo2 = initialStats.ecologyStats.co2;
        // globalStats.currentWasteProduced = initialStats.ecologyStats.waste;
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
        StatUtils.UpdateGlobalStatsFromCard(stats, card);
        
        // Update object stats depending on objects placed or removed
        StatUtils.UpdateObjectStatsFromObjects(stats, citizensGestion, gameManager.objectManager.GetAllObjectScripts());
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
        StatUtils.DailyUpdateGlobalStatsFromCitizens(stats, citizensGestion);
        
        // Don't update objects stats here no buildings are placed or removed
        
        // Update dashboard
        UpdateDashboard();
    }
    
    
    // Called by the periodic event every month to apply profits, losses, etc.
    public void UpdateGlobalStatsFromObjectsEvent(List<ObjectScript> objects)
    {
        // Update global stats
        StatUtils.UpdateGlobalStatsFromObjects(stats, objects);
        
        
        // Todo : check cloud event
        
        // Update dashboard
        UpdateDashboard();
    }
    
    public void InitDashboard(ObjectManager objectManager)
    {
        
        StatUtils.UpdateObjectStatsFromObjects(stats, citizensGestion, gameManager.objectManager.GetAllObjectScripts());
        StatUtils.UpdateGlobalStatsFromObjects(stats, objectManager.GetAllObjectScripts());
        citizensGestion.UpdateCitizensStats();
        // Debug.Log();
        UpdateDashboard();
    }
    
    private void UpdateDashboard()
    {
        // // Compute dashboard objects stats which is the combination of objects stats and citizens transports stats
        // StatUtils.UpdateDashboardObjectStats(dashboardStats, stats, citizensGestion.dailyTransportsStats);
        
        // Compute rates
        StatUtils.ComputeRates(stats, citizensGestion, maxStats);
        
        // Update the dashboard
        displayDashboard.UpdateDashboardStats(stats, citizensGestion, maxStats);
    }
}
