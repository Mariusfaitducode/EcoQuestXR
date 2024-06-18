using System;
using System.Collections;
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
    // GameObjects
    internal GameManager gameManager;
    internal CloudController cloudController;
    public DisplayDashboard displayDashboard;
    
    // Stats
    public InitialStats initialStats;
    public MaxStats maxStats;
    public ClassStats stats = new ClassStats();
    
    // Citizens
    internal CitizensGestion citizensGestion = new CitizensGestion();
    
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
        
        // check animation cloud
        CheckCloudAndGameoverEvents();
        
        // Update the dashboard
        displayDashboard.UpdateDashboardStats(stats, citizensGestion, maxStats);
    }
    
    public void CheckCloudAndGameoverEvents()
    {
        if (gameManager.controlMode == ControlMode.keyboard) return;
        float duration;
        if (stats.currentGlobalStats.money == 0 && !cloudController.cloudDones.Find(x => x.cloudEvent == CloudEvent.loseMoney).done)
        {
            //lose money
            duration = cloudController.DisplayNewText(CloudEvent.loseMoney);
            StartCoroutine(DelayEndGame(duration));
            Debug.Log("Animation cloud lose money");
        }
        else if (stats.currentGlobalStats.energy == 0 && !cloudController.cloudDones.Find(x => x.cloudEvent == CloudEvent.loseEnergy).done)
        {
            //lose energy
            duration = cloudController.DisplayNewText(CloudEvent.loseEnergy);
            StartCoroutine(DelayEndGame(duration));
            Debug.Log("Animation cloud lose energy");
        }
        else if (stats.overallSocietyRate < 0.2 && !cloudController.cloudDones.Find(x => x.cloudEvent == CloudEvent.loseSociety).done)
        {
            //lose society
            duration = cloudController.DisplayNewText(CloudEvent.loseSociety);
            StartCoroutine(DelayEndGame(duration));
            Debug.Log("Animation cloud lose society");
        } 
        else if (stats.overallEcologyRate > 0.9 && !cloudController.cloudDones.Find(x => x.cloudEvent == CloudEvent.winEcology).done)
        {
            //win ecology
            duration = cloudController.DisplayNewText(CloudEvent.winEcology);
            StartCoroutine(DelayEndGame(duration));
            Debug.Log("Animation cloud win ecology");
        }
        else if (stats.currentGlobalStats.money < maxStats.globalStats.money * 0.1 && !cloudController.cloudDones.Find(x => x.cloudEvent == CloudEvent.warningMoney).done)
        {
            //warning money
            cloudController.DisplayNewText(CloudEvent.warningMoney);
            Debug.Log("Animation cloud warning money");
        }
        else if (stats.currentGlobalStats.energy < maxStats.globalStats.energy * 0.1 && !cloudController.cloudDones.Find(x => x.cloudEvent == CloudEvent.warningEnergy).done)
        {
            //warning energy
            cloudController.DisplayNewText(CloudEvent.warningEnergy);
            Debug.Log("Animation cloud warning energy");
        }
        else if (stats.overallSocietyRate < 0.3 && !cloudController.cloudDones.Find(x => x.cloudEvent == CloudEvent.warningSociety).done)
        {
            //warning society
            cloudController.DisplayNewText(CloudEvent.warningSociety);
        }
        else if (stats.overallEcologyRate < 0.2 && !cloudController.cloudDones.Find(x => x.cloudEvent == CloudEvent.warningEcology).done)
        {
            //warning ecology
            cloudController.DisplayNewText(CloudEvent.warningEcology);
            Debug.Log("Animation cloud warning ecology");
        }
        else if (stats.overallEcologyRate > 0.8 && !cloudController.cloudDones.Find(x => x.cloudEvent == CloudEvent.encouragementEcology).done)
        {
            //encouragement ecologie
            cloudController.DisplayNewText(CloudEvent.encouragementEcology);
            Debug.Log("Animation cloud encouragement ecology");
        }
        
    }
    
    public IEnumerator DelayEndGame(float duration)
    {
        duration += 1;
        Debug.Log("Wait for " + duration + " seconds before end animation.");
        yield return new WaitForSeconds(duration);
        duration = cloudController.DisplayNewText(CloudEvent.lose);
        duration += 1;
        Debug.Log("Wait for " + duration + " seconds before end game.");
        yield return new WaitForSeconds(duration);
        gameManager.settingsController.end_game();
    }
}
