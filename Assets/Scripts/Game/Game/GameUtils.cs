using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    public static void ExecuteCardAction(Card card, ObjectManager objectManager, StatManager statManager)
    {
        if (card.cardType == CardType.Construction)
        {
            objectManager.PlaceObjects(card.objectProperties1, card.quantityObject1);
        }
        else if (card.cardType == CardType.Destruction)
        {
            objectManager.RemoveObjects(card.objectProperties1, card.quantityObject1);
        }
        else if (card.cardType == CardType.Upgrade)
        {
            objectManager.RemoveObjects(card.objectProperties1, card.quantityObject1);
            objectManager.PlaceObjects(card.objectProperties2, card.quantityObject2);
        }
        
        // Update global stats
        statManager.UpdateGlobalStatsFromCardEvent(card);
        
        // Update populations stats
        statManager.citizensGestion.ImpactPopulationStats(card.stats);
        
        // Update objects stats
        statManager.UpdateObjectStatsFromObjectsAndCitizensEvent(objectManager);
    }
}