using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActionsUtils
{
    public static void PlaceObjectsOnMap(Card card)
    {
        // Load object prefab
        GameObject objectPrefab = LoadObjectPrefab(card.objectProperties1.prefabName);
        
        AreaType areaType = card.objectProperties1.areaType;
        
        // Place objects on the map
        GameObject cardObject = GameObject.Instantiate(objectPrefab, cardLocationPanels[idx].transform);
    }
    
    public static GameObject LoadObjectPrefab(string objectName)
    {
        // Load object prefab
        return Resources.Load<GameObject>("Prefabs/Objects/" + objectName);
    }
}
