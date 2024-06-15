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
        else if (card.cardType == CardType.Replace)
        {
            objectManager.ReplaceObjects(card.objectProperties1, card.objectProperties2, card.quantityObject1, card.quantityObject2);
        }
        // TODO : more actions to come
        
        audioManager.PlaySound(card.cardType);
        
        // Update global stats
        statManager.UpdateStatsFromCardEvent(card);
    }
}
