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
    public static float DistanceWeight(float distance, float mean, float stddev) {
        return Mathf.Exp(-Mathf.Pow((distance - mean) / stddev, 2));
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
    
    public static void InitializeTransportsStats(Stat walk, Stat car, Stat bike, Stat bus)
    {
        walk.Reset();
        car.Reset();
        bike.Reset();
        bus.Reset();
        
        walk.profits = 1000;
        walk.losses = 100;
        walk.airQuality = 100;
        walk.groundQuality = 0;
        walk.biodiversity = 0;
        walk.size = 0;
        walk.health = 100;
        walk.happiness = 100;
        walk.sensibilisation = 100;
        walk.energyProduction = 0;
        walk.energyConsumption = 0;
        
        car.profits = 0;
        car.losses = 0;
        car.airQuality = -100;
        car.groundQuality = -100;
        car.biodiversity = -100;
        car.size = 0;
        car.health = -10;
        car.happiness = 0;
        car.sensibilisation = 0;
        car.energyProduction = 0;
        car.energyConsumption = 0;
        
        bike.profits = 0;
        bike.losses = 0;
        bike.airQuality = 0;
        bike.groundQuality = 0;
        bike.biodiversity = 0;
        bike.size = 0;
        bike.health = 10;
        bike.happiness = 10;
        bike.sensibilisation = 0;
        bike.energyProduction = 0;
        bike.energyConsumption = 0;
        
        bus.profits = 0;
        bus.losses = 0;
        bus.airQuality = 0;
        bus.groundQuality = 0;
        bus.biodiversity = 0;
        bus.size = 0;
        bus.health = 0;
        bus.happiness = 0;
        bus.sensibilisation = 0;
        bus.energyProduction = 0;
        bus.energyConsumption = 0;
    }
}
