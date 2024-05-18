using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectGestion
{
    public static void PlaceObjectsOnMap(ObjectProperties objectProperties )
    {
        // Get Prefab
        GameObject objectPrefab = ObjectsInitialization.LoadPrefab(objectProperties);
        
        // AreaType areaType = card.objectProperties1.areaType;
        
        // Place objects on the map
        GameObject cardObject = GameObject.Instantiate(objectPrefab, Vector3.zero, Quaternion.identity);
        
        
    }
}
