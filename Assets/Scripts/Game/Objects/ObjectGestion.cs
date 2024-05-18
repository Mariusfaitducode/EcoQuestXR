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
        
        // TODO : Initialize object script with objectProperties and gameManager

        // cardObject.AddComponent<Objects>();
        // cardObject.GetComponent<Objects>().objectProperties = objectProperties;
        // cardObject.GetComponent<Objects>().gameManager = gameManager;
        //
        //
        // objectsInstantiated.Add(cardObject);
        
        // TODO : Place object on the map in the good area
        
        // Place in good area object
        
        
        // TODO : Update area properties
        
        // ? List objects instantiated
    }
    
    public static void RemoveObjectOnMap(ObjectProperties objectProperties)
    {
        // Remove objects from the map
    }
}
