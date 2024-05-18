using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectGestion
{
    public static void PlaceObjectOnMap(ObjectProperties objectProperties )
    {
        // Get Prefab
        GameObject objectPrefab = ObjectsInitialization.LoadPrefab(objectProperties);
        
        // Place objects on the map
        GameObject cardObject = GameObject.Instantiate(objectPrefab, Vector3.zero, Quaternion.identity);
        // cardObject.AddComponent<Objects>();
        // cardObject.GetComponent<Objects>().objectProperties = objectProperties;
        // cardObject.GetComponent<Objects>().gameManager = gameManager;
        //
        //
        // objectsInstantiated.Add(cardObject);
    }
    
    public static void RemoveObjectOnMap(ObjectProperties objectProperties)
    {
        // Remove objects from the map
    }
}
