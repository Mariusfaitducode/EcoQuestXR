using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInitialisation
{
    public static void TransfersObjectsPropertiesToCards(CardManager cardManager, ObjectManager objectManager, bool hasTransferredObjectsPropertiesFromOMToCM)
    {
        if (cardManager.areCardsInitialized && objectManager.areObjectsPropertiesInitialized && !hasTransferredObjectsPropertiesFromOMToCM)
        {
            cardManager.SetCardsProperties(objectManager.objectsProperties);
            hasTransferredObjectsPropertiesFromOMToCM = true;
        }
    }
}
