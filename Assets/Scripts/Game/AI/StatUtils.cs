using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class StatUtils
{
    public static string ConvertPercentToText(float number)
    {
        // Display 2 decimal after the comma( ex: 0.12789456 -> 12.79%)
        return (100 * number).ToString("F2");
    }
    
    public static string ConvertFloatToText(float number)
    {
        return number.ToString("F1");
    }
    
    // Fonction de transformation de la distance
    public static float GetFloatWeight(float number, float mean, float stddev) {
        return Mathf.Exp(-Mathf.Pow((number - mean) / stddev, 2));
    }
    
    public static void UpdateGlobalStatsFromObjects(GlobalStats globalStats, List<ObjectScript> objects)
    {
        foreach (ObjectScript objScript in objects)
        {
            if (objScript.objectProperties != null && objScript.objectProperties.stats != null)
            {
                globalStats.currentMoneyInBank += objScript.objectProperties.stats.profitsPerMonth;
                globalStats.currentEnergyInStock += objScript.objectProperties.stats.energyProductionPerMonth;
                globalStats.currentEmittedCo2 += objScript.objectProperties.stats.co2EmissionPerMonth;
                globalStats.currentWasteProduced += objScript.objectProperties.stats.wasteProductionPerMonth;
                
                globalStats.currentMoneyInBank -= objScript.objectProperties.stats.lossesPerMonth;
                globalStats.currentEnergyInStock -= objScript.objectProperties.stats.energyConsumptionPerMonth;
                globalStats.currentEmittedCo2 -= objScript.objectProperties.stats.co2AbsorptionPerMonth;
                globalStats.currentWasteProduced -= objScript.objectProperties.stats.wasteDestructionPerMonth;
            }
            else
            {
                Debug.LogWarning("Object " + objScript.gameObject.name + " doesn't have properties");
            }
        }
    }
    
    public static void UpdateDashboardObjectStats(Stat dashboardStats, Stat objectsStats, Stat dailyTransportsStats)
    {
        dashboardStats.Reset();
        dashboardStats.Add(objectsStats);
        dashboardStats.Add(dailyTransportsStats);
    }
    
    
    
    public static void UpdateObjectStatsFromObjects(StatManager statManager, List<ObjectScript> objects)
    {
        // Set all object stats to 0
        statManager.objectsStats.Reset();
        
        // Add stats from all the objects on the map
        foreach (ObjectScript objScript in objects)
        {
            if (objScript.objectProperties != null && objScript.objectProperties.stats != null)
            {
                statManager.objectsStats.Add(objScript.objectProperties.stats);
            }
            else
            {
                Debug.LogWarning("Object " + objScript.gameObject.name + " doesn't have properties");
            }
            
        }
        
        // Update stats depending on population size
        float factor = (float)statManager.citizensGestion.totalCitizens / statManager.citizensGestion.maxPopSize;
        statManager.objectsStats.profitsPerMonth *= factor;
        
    }

    public static void ComputeRates(StatManager statManager)
    {
        // Overall society rate
        statManager.globalStats.overallSocietyRate = statManager.citizensGestion.citizensStats.health +
                                                     statManager.citizensGestion.citizensStats.happiness +
                                                     statManager.citizensGestion.citizensStats.sensibilisation;
        statManager.globalStats.overallSocietyRate /= 3;
        
        // Overall ecology rate
        // positive scores
        float populationSensitiveScore = statManager.objectsStats.sensibilisation;
        float netGreenSpacesScore = statManager.objectsStats.greenSpaces / statManager.maxGreenSpaces;
        
        // global pollutions scores
        float netCo2EmittedScore = 1 - (statManager.globalStats.currentEmittedCo2 / statManager.maxEmittedCo2);
        float netWasteProducedScore = 1 - (statManager.globalStats.currentWasteProduced / statManager.maxWasteProduced);
        
        // monthly pollutions scores
        float co2EmissionPerMonthScore = 1 - ((statManager.objectsStats.co2EmissionPerMonth - statManager.objectsStats.co2AbsorptionPerMonth) / statManager.maxCo2EmissionPerMonth);
        float wasteProductionPerMonthScore = 1 - ((statManager.objectsStats.wasteProductionPerMonth - statManager.objectsStats.wasteDestructionPerMonth) / statManager.maxWasteProductionPerMonth);
        
        
        statManager.globalStats.overallEcologyRate = netCo2EmittedScore +
                                                     netWasteProducedScore + 
                                                     netGreenSpacesScore + 
                                                     co2EmissionPerMonthScore + 
                                                     wasteProductionPerMonthScore + 
                                                     populationSensitiveScore;
        statManager.globalStats.overallEcologyRate /= 6;
        
    }
    
    public static void UpdateGlobalStatsFromCard(GlobalStats globalStats, Card card)
    {
        globalStats.currentMoneyInBank -= card.cardStats.actionCost;
        globalStats.currentEnergyInStock -= card.cardStats.actionEnergyCost;
        globalStats.currentEmittedCo2 += card.cardStats.actionCo2Emission;
        globalStats.currentWasteProduced += card.cardStats.actionWasteProduction;
    }
    
    public static void DailyUpdateGlobalStatsFromCitizens(GlobalStats globalStats, CitizensGestion citizensGestion)
    {
        globalStats.currentMoneyInBank -= (citizensGestion.dailyTransportsStats.lossesPerMonth / 30);
        globalStats.currentEnergyInStock -= (citizensGestion.dailyTransportsStats.energyConsumptionPerMonth / 30);
        globalStats.currentEmittedCo2 += (citizensGestion.dailyTransportsStats.co2EmissionPerMonth / 30);
        globalStats.currentWasteProduced += (citizensGestion.dailyTransportsStats.wasteProductionPerMonth / 30);
    }
    
    public static TransportMode GetTransportModeByName(List<TransportMode> transportModes, string name)
    {
        foreach (TransportMode transportMode in transportModes)
        {
            if (transportMode.name == name)
            {
                return transportMode;
            }
        }
        Debug.LogError("Transport mode " + name + " not found");
        return new TransportMode();
    }
    
    
}
