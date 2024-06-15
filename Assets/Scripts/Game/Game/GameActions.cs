using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameActions
{
    public static void ExecuteCardAction(Card card, ObjectManager objectManager, StatManager statManager, AudioManager audioManager)
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
        // TODO : more actions to come
        
        audioManager.PlaySound(card.cardType);
        
        // Update global stats
        statManager.UpdateStatsFromCardEvent(card);
    }
}
