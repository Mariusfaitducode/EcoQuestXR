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
        else if (card.cardType == CardType.Replacement)
        {
            gameManager.objectManager.ReplaceObjects(card.objectProperties1, card.objectProperties2, card.quantityObject1, card.quantityObject2);
        }
        else if (card.cardType == CardType.Upgrade)
        {
            gameManager.objectManager.UpgradeObjects(card.objectProperties1, card.quantityObject1, card.objectProperties2, true);
        }
        else if (card.cardType == CardType.Downgrade)
        {
            gameManager.objectManager.UpgradeObjects(card.objectProperties1, card.quantityObject1, card.objectProperties2, false);

        }
        
        
        // Update global stats
        gameManager.statManager.UpdateStatsFromCardEvent(card);
    }
}
