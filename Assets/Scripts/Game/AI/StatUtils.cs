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
}
