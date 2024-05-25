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
    
    // Fonction de transformation de la distance
    public static float GetFloatWeight(float number, float mean, float stddev) {
        return Mathf.Exp(-Mathf.Pow((number - mean) / stddev, 2));
    }

    public static void GetObjectStat(Card card)
    {
        card.stats.Reset();
        
        // Check if statObject2 is null for card type Upgrade
        if (card.cardType == CardType.Upgrade && card.objectProperties2 == null)
        {
            Debug.LogError("StatObject2 is null for card type Upgrade");
        }
        
        if (card.cardType == CardType.Construction)
        {
            card.stats.profits = card.objectProperties1.stats.profits * card.quantityObject1;
            card.stats.losses = card.objectProperties1.stats.losses * card.quantityObject1;
            
            card.stats.airQuality = card.objectProperties1.stats.airQuality * card.quantityObject1;
            card.stats.groundQuality = card.objectProperties1.stats.groundQuality * card.quantityObject1;
            card.stats.biodiversity = card.objectProperties1.stats.biodiversity * card.quantityObject1;
            
            card.stats.size = card.objectProperties1.stats.size * card.quantityObject1;
            
            card.stats.health = card.objectProperties1.stats.health * card.quantityObject1;
            card.stats.happiness = card.objectProperties1.stats.happiness * card.quantityObject1;
            card.stats.sensibilisation = card.objectProperties1.stats.sensibilisation * card.quantityObject1;
            
            card.stats.energyProduction = card.objectProperties1.stats.energyProduction * card.quantityObject1;
            card.stats.energyConsumption = card.objectProperties1.stats.energyConsumption * card.quantityObject1;
        }
        else if (card.cardType == CardType.Destruction)
        {
            card.stats.profits = -card.objectProperties1.stats.profits * card.quantityObject1;
            card.stats.losses = -card.objectProperties1.stats.losses * card.quantityObject1;
            
            card.stats.airQuality = -card.objectProperties1.stats.airQuality * card.quantityObject1;
            card.stats.groundQuality = -card.objectProperties1.stats.groundQuality * card.quantityObject1;
            card.stats.biodiversity = -card.objectProperties1.stats.biodiversity * card.quantityObject1;
            
            card.stats.size = -card.objectProperties1.stats.size * card.quantityObject1;

            card.stats.health = -card.objectProperties1.stats.health * card.quantityObject1;
            card.stats.happiness = -card.objectProperties1.stats.happiness * card.quantityObject1;
            card.stats.sensibilisation = -card.objectProperties1.stats.sensibilisation * card.quantityObject1;
            
            card.stats.energyProduction = -card.objectProperties1.stats.energyProduction * card.quantityObject1;
            card.stats.energyConsumption = -card.objectProperties1.stats.energyConsumption * card.quantityObject1;
        }
        else if (card.cardType == CardType.Upgrade)
        {
            card.stats.profits = -card.objectProperties1.stats.profits * card.quantityObject1 + card.objectProperties2.stats.profits * card.quantityObject2;
            card.stats.losses = -card.objectProperties1.stats.losses * card.quantityObject1 + card.objectProperties2.stats.losses * card.quantityObject2;
            
            card.stats.airQuality = -card.objectProperties1.stats.airQuality * card.quantityObject1 + card.objectProperties2.stats.airQuality * card.quantityObject2;
            card.stats.groundQuality = -card.objectProperties1.stats.groundQuality * card.quantityObject1 + card.objectProperties2.stats.groundQuality * card.quantityObject2;
            card.stats.biodiversity = -card.objectProperties1.stats.biodiversity * card.quantityObject1 + card.objectProperties2.stats.biodiversity * card.quantityObject2;
            
            card.stats.size = -card.objectProperties1.stats.size * card.quantityObject1 + card.objectProperties2.stats.size * card.quantityObject2;
            
            card.stats.health = -card.objectProperties1.stats.health * card.quantityObject1 + card.objectProperties2.stats.health * card.quantityObject2;
            card.stats.happiness = -card.objectProperties1.stats.happiness * card.quantityObject1 + card.objectProperties2.stats.happiness * card.quantityObject2;
            card.stats.sensibilisation = -card.objectProperties1.stats.sensibilisation * card.quantityObject1 + card.objectProperties2.stats.sensibilisation * card.quantityObject2;
            
            card.stats.energyProduction = -card.objectProperties1.stats.energyProduction * card.quantityObject1 + card.objectProperties2.stats.energyProduction * card.quantityObject2;
            card.stats.energyConsumption = -card.objectProperties1.stats.energyConsumption * card.quantityObject1 + card.objectProperties2.stats.energyConsumption * card.quantityObject2;
        }
        else
        {
            Debug.LogError("Card type " + card.cardType + " not implemented");
        }
    }
    
    public static void UpdateGlobalStatsFromObjects(GlobalStats globalStats, List<ObjectScript> objects)
    {
        foreach (ObjectScript objScript in objects)
        {
            if (objScript.objectProperties != null && objScript.objectProperties.stats != null)
            {
                globalStats.currentMoneyInBank += objScript.objectProperties.stats.profits;
                globalStats.currentMoneyInBank -= objScript.objectProperties.stats.losses;
                
                globalStats.currentEnergyInStock += objScript.objectProperties.stats.energyProduction;
                globalStats.currentEnergyInStock -= objScript.objectProperties.stats.energyConsumption;
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
    
    public static void UpdateGlobalStatsFromCard(GlobalStats globalStats, Card card)
    {
        globalStats.currentMoneyInBank -= card.actionCost;
        globalStats.currentEnergyInStock -= card.actionEnergyCost;
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
