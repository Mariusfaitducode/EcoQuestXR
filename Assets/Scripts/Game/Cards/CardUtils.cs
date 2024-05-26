using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUtils
{
    public static void GetCardStat(Card card)
    {
        // Check if statObject2 is null for card type Upgrade
        if (card.cardType == CardType.Upgrade && card.objectProperties2 == null)
        {
            Debug.LogError("StatObject2 is null for card type Upgrade");
        }

        if (card.cardType == CardType.Construction)
        {
            card.actionCost = card.objectProperties1.stats.constructionCost * card.quantityObject1;
            card.actionEnergyCost = card.objectProperties1.stats.energyConstructionCost * card.quantityObject1;
            card.ecologyRate = -1;
            card.acceptationRate = -1;
        }
        else if (card.cardType == CardType.Destruction)
        {
            card.actionCost = card.objectProperties1.stats.destructionCost * card.quantityObject1;
            card.actionEnergyCost = card.objectProperties1.stats.energyDestructionCost * card.quantityObject1;
            card.ecologyRate = -1;
            card.acceptationRate = -1;
        }
        else if (card.cardType == CardType.Upgrade)
        {
            card.actionCost = card.objectProperties1.stats.destructionCost * card.quantityObject1 + card.objectProperties2.stats.constructionCost * card.quantityObject2;
            card.actionEnergyCost = card.objectProperties1.stats.energyDestructionCost * card.quantityObject1 +
                                    card.objectProperties2.stats.energyConstructionCost * card.quantityObject2;
            card.ecologyRate = -1;
            card.acceptationRate = -1;
        }
        else if (card.cardType == CardType.Rules)
        {
            card.actionCost = 0;
            card.actionEnergyCost = -1;
            card.ecologyRate = -1;
            card.acceptationRate = -1;
        }
        else if (card.cardType == CardType.Event)
        {
            card.actionCost = 0;
            card.actionEnergyCost = -1;
            card.ecologyRate = 0;
            card.acceptationRate = 0;
        }
    }
}
