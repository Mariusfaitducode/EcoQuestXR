using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class StatUtils
{
    public static string ConvertNumberToText(int number)
    {
        string sign = number < 0 ? "-" : "";
        number = Mathf.Abs(number);
        
        if (number < 1000)
        {
            return sign + number.ToString();
        }
        else if (number < 1000000)
        {
            return sign + (number / 1000).ToString() + "K";
        }
        else if (number < 1000000000)
        {
            return sign + (number / 1000000).ToString() + "M";
        }
        else
        {
            return sign + (number / 1000000000).ToString() + "B";
        }
    }
    
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
    
    public static void UpdateObjectStatsFromObjectsAndCitizens(Stat objectsStats, List<ObjectScript> objects, CitizensGestion citizensGestion)
    {
        // Set all object stats to 0
        objectsStats.Reset();
        
        // Add stats from all the objects on the map
        foreach (ObjectScript objScript in objects)
        {
            if (objScript.objectProperties != null && objScript.objectProperties.stats != null)
            {
                objectsStats.Add(objScript.objectProperties.stats);
            }
            else
            {
                Debug.LogWarning("Object " + objScript.gameObject.name + " doesn't have properties");
            }
            
        }
        
        // Add to global stats the consequence of human activities (influence of transport use -> pollution, biodiversity)
        objectsStats.Add(citizensGestion.ComputeInfluenceOnGlobalStats());

        // Get the stats from the citizens (health, happiness, sensibilisation)
        objectsStats.Overwrite(citizensGestion.GetCitizensStats());
    }

    public static void ComputeRates(GlobalStats globalStats, Stat stats, float maxGreenSpaces)
    {
        globalStats.overallSocietyRate = (stats.health + stats.happiness + stats.sensibilisation) / 3;
        
        float netCo2Emitted = (stats.co2EmissionPerMonth - stats.co2AbsorptionPerMonth) / globalStats.currentEmittedCo2;
        float netWasteProduced = (stats.wasteProductionPerMonth - stats.wasteDestructionPerMonth) / globalStats.currentWasteProduced;
        float netGreenSpaces = stats.greenSpaces / maxGreenSpaces;
        
        globalStats.overallEcologyRate = 1 - (netCo2Emitted + netWasteProduced) / 2 + netGreenSpaces;
    }
    
    public static void UpdateGlobalStatsFromCard(GlobalStats globalStats, Card card)
    {
        globalStats.currentMoneyInBank -= card.cardStats.actionCost;
        globalStats.currentEnergyInStock -= card.cardStats.actionEnergyCost;
        globalStats.currentEmittedCo2 += card.cardStats.actionCo2Emission;
        globalStats.currentWasteProduced += card.cardStats.actionWasteProduction;
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
