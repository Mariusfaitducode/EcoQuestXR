using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUtils
{
    public static void GetCardStat(Card card)
    {
        // Check if statObject2 is null for card type Upgrade
        if (card.cardType == CardType.Replace && card.objectProperties2 == null)
        {
            Debug.LogError("StatObject2 is null for card type Upgrade");
        }

        if (card.cardType == CardType.Construction)
        {
            card.cardStats.actionCost = card.objectProperties1.stats.constructionCost * card.quantityObject1;
            card.cardStats.actionEnergyCost = card.objectProperties1.stats.energyConstructionCost * card.quantityObject1;
            card.cardStats.actionCo2Emission = card.objectProperties1.stats.constructionCo2Emission * card.quantityObject1;
            card.cardStats.actionWasteProduction = card.objectProperties1.stats.constructionWasteProduction * card.quantityObject1;
            
            card.cardStats.ecologyRate = -1;
            card.cardStats.acceptationRate = -1;
        }
        else if (card.cardType == CardType.Destruction)
        {
            card.cardStats.actionCost = card.objectProperties1.stats.destructionCost * card.quantityObject1;
            card.cardStats.actionEnergyCost = card.objectProperties1.stats.energyDestructionCost * card.quantityObject1;
            card.cardStats.actionCo2Emission = card.objectProperties1.stats.destructionCo2Emission * card.quantityObject1;
            card.cardStats.actionWasteProduction = card.objectProperties1.stats.destructionWasteProduction * card.quantityObject1;
            
            card.cardStats.ecologyRate = -1;
            card.cardStats.acceptationRate = -1;
        }
        else if (card.cardType == CardType.Replace)
        {
            card.cardStats.actionCost = card.objectProperties1.stats.destructionCost * card.quantityObject1 + card.objectProperties2.stats.constructionCost * card.quantityObject2;
            card.cardStats.actionEnergyCost = card.objectProperties1.stats.energyDestructionCost * card.quantityObject1 + card.objectProperties2.stats.energyConstructionCost * card.quantityObject2;
            card.cardStats.actionCo2Emission = card.objectProperties1.stats.destructionCo2Emission * card.quantityObject1 + card.objectProperties2.stats.constructionCo2Emission * card.quantityObject2;
            card.cardStats.actionWasteProduction = card.objectProperties1.stats.destructionWasteProduction * card.quantityObject1 + card.objectProperties2.stats.constructionWasteProduction * card.quantityObject2;
            
            card.cardStats.ecologyRate = -1;
            card.cardStats.acceptationRate = -1;
        }
        else
        {
            Debug.LogError("Card type not implemented : " + card.cardType);
        }
    }
    
    public static void GetObjectStat(Card card)
    {
        card.stats.Reset();
        
        // Check if statObject2 is null for card type Upgrade
        if (card.cardType == CardType.Replace && card.objectProperties2 == null)
        {
            Debug.LogError("StatObject2 is null for card type Upgrade");
        }
        
        if (card.cardType == CardType.Construction)
        {
            card.stats = card.objectProperties1.stats.Multiply(card.quantityObject1);
        }
        else if (card.cardType == CardType.Destruction)
        {
            card.stats = card.objectProperties1.stats.Multiply(-card.quantityObject1);
        }
        else if (card.cardType == CardType.Replace)
        {
            Stat stat1 = card.objectProperties1.stats.Multiply(-card.quantityObject1);
            Stat stat2 = card.objectProperties2.stats.Multiply(card.quantityObject2);
            card.stats.Add(stat1);
            card.stats.Add(stat2);
            card.stats.ResetConstructionDestructionStats();
        }
        else
        {
            Debug.LogError("Card type " + card.cardType + " not implemented");
        }
    }
}
