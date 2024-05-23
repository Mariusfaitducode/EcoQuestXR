using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Actions
{
    public static void ExecuteCardAction(Card card, ObjectManager objectManager)
    {
        if (card.cardType == CardType.Construction)
        {
            objectManager.PlaceObjects(card.objectProperties1, card.quantityObject1);
        }
        else if (card.cardType == CardType.Destruction)
        {
            objectManager.RemoveObjects(card.objectProperties1, card.quantityObject1);
        }
        // else if (card.cardType == CardType.Upgrade)
        // {
        //     objectManager.UpgradeObjectsOnMap(card.objectProperties1, card.objectProperties2);
        // }
        // else if (card.cardType == CardType.Rules)
        // {
        //     objectManager.ApplyRulesOnMap(card.objectProperties1);
        // }
        // else if (card.cardType == CardType.Event)
        // {
        //     objectManager.ApplyEventOnMap(card.objectProperties1);
        // }
    }
}
