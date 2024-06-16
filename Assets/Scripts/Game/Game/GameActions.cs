using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameActions
{
    public static void ExecuteCardAction(Card card, GameManager gameManager)
    {
        if (card.cardType == CardType.Construction)
        {
            gameManager.objectManager.PlaceObjects(card.objectProperties1, card.quantityObject1);
        }
        else if (card.cardType == CardType.Destruction)
        {
            gameManager.objectManager.RemoveObjects(card.objectProperties1, card.quantityObject1);
        }
        else if (card.cardType == CardType.Upgrade)
        {
            gameManager.objectManager.ReplaceObjects(card.objectProperties1, card.objectProperties2, card.quantityObject1, card.quantityObject2);
        }
        // TODO : more actions to come
        
        // Update global stats
        gameManager.statManager.UpdateStatsFromCardEvent(card);
    }
}
