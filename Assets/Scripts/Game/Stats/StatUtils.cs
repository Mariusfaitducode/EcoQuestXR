using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class StatUtils
{
    public static string ConvertPercentToText(float number)
    {
        // Display 2 decimal after the comma( ex: 0.12789456 -> 12.79%)
        return (100 * number).ToString("F1");
    }
    
    public static string ConvertFloatToText(float number)
    {
        if (Mathf.Abs(number) > 1000000)
        {
            return (number / 1000000).ToString("F1") + "M";
        }
        if (Mathf.Abs(number) > 1000)
        {
            return (number / 1000).ToString("F1") + "K";
        }
        return number.ToString("F1");
    }
    
    public static string ConvertIntToText(int number)
    {
        if (Mathf.Abs(number) > 1000000)
        {
            return (number / 1000000).ToString("F1") + "M ";
        }
        if (Mathf.Abs(number) > 1000)
        {
            return (number / 1000).ToString("F1") + "K ";
        }
        return number.ToString("F1");
    }
    
    // Fonction de transformation de la distance
    public static float GetFloatWeight(float number, float mean, float stddev) {
        return Mathf.Exp(-Mathf.Pow((number - mean) / stddev, 2));
    }
    
    public static void UpdateGlobalStatsFromObjects(ClassStats stats, List<ObjectScript> objects)
    {
        foreach (ObjectScript objScript in objects)
        {
            if (objScript.objectProperties != null && objScript.objectProperties.stats != null)
            {
                stats.currentGlobalStats.money += (objScript.objectProperties.stats.profitsPerMonth - objScript.objectProperties.stats.lossesPerMonth);
                stats.currentGlobalStats.energy += (objScript.objectProperties.stats.energyProductionPerMonth - objScript.objectProperties.stats.energyConsumptionPerMonth);
                stats.currentEcologyStats.co2 += (objScript.objectProperties.stats.co2EmissionPerMonth - objScript.objectProperties.stats.co2AbsorptionPerMonth);
                stats.currentEcologyStats.waste += (objScript.objectProperties.stats.wasteProductionPerMonth - objScript.objectProperties.stats.wasteDestructionPerMonth);
                
                // globalStats.currentMoneyInBank += objScript.objectProperties.stats.profitsPerMonth;
                // globalStats.currentEnergyInStock += objScript.objectProperties.stats.energyProductionPerMonth;
                // globalStats.currentEmittedCo2 += objScript.objectProperties.stats.co2EmissionPerMonth;
                // globalStats.currentWasteProduced += objScript.objectProperties.stats.wasteProductionPerMonth;
                //
                // globalStats.currentMoneyInBank -= objScript.objectProperties.stats.lossesPerMonth;
                // globalStats.currentEnergyInStock -= objScript.objectProperties.stats.energyConsumptionPerMonth;
                // globalStats.currentEmittedCo2 -= objScript.objectProperties.stats.co2AbsorptionPerMonth;
                // globalStats.currentWasteProduced -= objScript.objectProperties.stats.wasteDestructionPerMonth;
            }
            else
            {
                Debug.LogWarning("Object " + objScript.gameObject.name + " doesn't have properties");
            }
        }
    }
    
    // public static void UpdateDashboardObjectStats(Stat dashboardStats, Stat objectsStats, Stat dailyTransportsStats)
    // {
    //     dashboardStats.Reset();
    //     dashboardStats.Add(objectsStats);
    //     dashboardStats.Add(dailyTransportsStats);
    // }
    
    
    
    public static void UpdateObjectStatsFromObjects(ClassStats stats, CitizensGestion citizensGestion, List<ObjectScript> objects)
    {
        // Reset calculated stats from objects
        // Reset global monthly stats
        stats.monthlyGlobalStats.Reset();
        // Reset global ecology stats
        stats.monthlyEcologyStats.Reset();
        
        // Reset stats by area
        foreach (ClassEcologyStats ecologyStats in stats.ecologyStatsByArea)
        {
            ecologyStats.Reset();
        }
        
        // Add stats from all the objects on the map
        foreach (ObjectScript objScript in objects)
        {
            if (objScript.objectProperties != null && objScript.objectProperties.stats != null)
            {
                // Add global monthly stats
                stats.monthlyGlobalStats.money += (objScript.objectProperties.stats.profitsPerMonth - objScript.objectProperties.stats.lossesPerMonth);
                stats.monthlyGlobalStats.energy += (objScript.objectProperties.stats.energyProductionPerMonth - objScript.objectProperties.stats.energyConsumptionPerMonth);
                
                // Add global ecology stats
                stats.monthlyEcologyStats.co2 += (objScript.objectProperties.stats.co2EmissionPerMonth - objScript.objectProperties.stats.co2AbsorptionPerMonth);
                stats.monthlyEcologyStats.waste += (objScript.objectProperties.stats.wasteProductionPerMonth - objScript.objectProperties.stats.wasteDestructionPerMonth);
                stats.monthlyEcologyStats.greenSpaces += objScript.objectProperties.stats.greenSpaces;
                
                // Add stats by area
                foreach (ClassEcologyStats ecologyStats in stats.ecologyStatsByArea)
                {
                    if (ecologyStats.areaType == objScript.objectProperties.areaType)
                    {
                        ecologyStats.co2 += (objScript.objectProperties.stats.co2EmissionPerMonth - objScript.objectProperties.stats.co2AbsorptionPerMonth);
                        ecologyStats.waste += (objScript.objectProperties.stats.wasteProductionPerMonth - objScript.objectProperties.stats.wasteDestructionPerMonth);
                        ecologyStats.greenSpaces += objScript.objectProperties.stats.greenSpaces;
                    }
                }
            }
            else
            {
                Debug.LogWarning("Object " + objScript.gameObject.name + " doesn't have properties");
            }
            
        }
        
        // Update stats depending on population size
        float factor = (float)citizensGestion.totalCitizens / citizensGestion.maxPopSize;
        stats.monthlyGlobalStats.money *= factor;
        
    }

    public static void ComputeRates(ClassStats stats, CitizensGestion citizensGestion, MaxStats maxStats)
    {
        // Overall society rate
        stats.overallSocietyRate = citizensGestion.citizensStats.health +
                                   citizensGestion.citizensStats.happiness +
                                   citizensGestion.citizensStats.sensibilisation;
        stats.overallSocietyRate /= 3;
        
        // Overall ecology rate
        float ratio1 = (float)stats.currentEcologyStats.greenSpaces / maxStats.monthlyEcologyStats.greenSpaces;
        float ratio2 = (float)stats.currentEcologyStats.co2 / maxStats.globalEcologyStats.co2;
        float ratio3 = (float)stats.currentEcologyStats.waste / maxStats.globalEcologyStats.waste;
        float ratio4 = (float)stats.monthlyEcologyStats.co2 / maxStats.monthlyEcologyStats.co2;
        float ratio5 = (float)stats.monthlyEcologyStats.waste / maxStats.monthlyEcologyStats.waste;
        
        stats.overallEcologyRate = ratio1 + (1 - ratio2) + (1 - ratio3) + (1 - ratio4) + (1 - ratio5);
        stats.overallEcologyRate /= 5;
        // // positive scores
        // float netGreenSpacesScore = (float)statManager.objectsStats.greenSpaces / statManager.maxStats.monthlyEcologyStats.greenSpaces;
        //
        // // global pollutions scores
        // float netCo2EmittedScore = 1 - (statManager.globalStats.currentEmittedCo2 / statManager.maxStats.globalEcologyStats.co2);
        // float netWasteProducedScore = 1 - (statManager.globalStats.currentWasteProduced / statManager.maxStats.globalEcologyStats.waste);
        //
        // // monthly pollutions scores
        // float co2EmissionPerMonthScore = 1 - ((statManager.objectsStats.co2EmissionPerMonth - statManager.objectsStats.co2AbsorptionPerMonth) / statManager.maxStats.monthlyEcologyStats.co2);
        // float wasteProductionPerMonthScore = 1 - ((statManager.objectsStats.wasteProductionPerMonth - statManager.objectsStats.wasteDestructionPerMonth) / statManager.maxStats.monthlyEcologyStats.waste);


        // statManager.globalStats.overallEcologyRate = netCo2EmittedScore +
        //                                              netWasteProducedScore +
        //                                              netGreenSpacesScore +
        //                                              co2EmissionPerMonthScore +
        //                                              wasteProductionPerMonthScore;
        // statManager.globalStats.overallEcologyRate /= 5;
        
    }
    
    public static void UpdateGlobalStatsFromCard(ClassStats stats, Card card)
    {
        stats.currentGlobalStats.money -= card.cardStats.actionCost;
        stats.currentGlobalStats.energy -= card.cardStats.actionEnergyCost;
        stats.currentEcologyStats.co2 += card.cardStats.actionCo2Emission;
        stats.currentEcologyStats.waste += card.cardStats.actionWasteProduction;
        
        // globalStats.currentMoneyInBank -= card.cardStats.actionCost;
        // globalStats.currentEnergyInStock -= card.cardStats.actionEnergyCost;
        // globalStats.currentEmittedCo2 += card.cardStats.actionCo2Emission;
        // globalStats.currentWasteProduced += card.cardStats.actionWasteProduction;
    }
    
    public static void DailyUpdateGlobalStatsFromCitizens(ClassStats stats, CitizensGestion citizensGestion)
    {
        stats.currentGlobalStats.money -= (citizensGestion.dailyTransportsStats.lossesPerMonth / 30);
        stats.currentGlobalStats.energy -= (citizensGestion.dailyTransportsStats.energyConsumptionPerMonth / 30);
        stats.currentEcologyStats.co2 += (citizensGestion.dailyTransportsStats.co2EmissionPerMonth / 30);
        stats.currentEcologyStats.waste += (citizensGestion.dailyTransportsStats.wasteProductionPerMonth / 30);
        
        
        // globalStats.currentMoneyInBank -= (citizensGestion.dailyTransportsStats.lossesPerMonth / 30);
        // globalStats.currentEnergyInStock -= (citizensGestion.dailyTransportsStats.energyConsumptionPerMonth / 30);
        // globalStats.currentEmittedCo2 += (citizensGestion.dailyTransportsStats.co2EmissionPerMonth / 30);
        // globalStats.currentWasteProduced += (citizensGestion.dailyTransportsStats.wasteProductionPerMonth / 30);
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
