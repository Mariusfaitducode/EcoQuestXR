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
            objectManager.UpgradeObjects(card.objectProperties1, card.objectProperties2, card.quantityObject1, card.quantityObject2);
        }
        
        // Update global stats
        statManager.UpdateStatsFromCardEvent(card);
    }
}
