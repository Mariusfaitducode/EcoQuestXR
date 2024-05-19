using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectGestion
{
    public static void PlaceObjectOnMap(ObjectProperties objectProperties, List<Area> areas, float prefabScale, GameManager gameManager, float uniformScale)
    {
        // Get Prefab
        
        // TODO : Load prefabLow + prefabHigh
        
        GameObject objectPrefab = ObjectsInitialization.LoadPrefab(objectProperties);
        
        // Place objects on the map
        // GameObject cardObject = GameObject.Instantiate(objectPrefab, Vector3.zero, Quaternion.identity);
        
        
        
        AreaPrefab areaPrefab = new AreaPrefab();
        areaPrefab.prefabLow = objectPrefab;
        areaPrefab.prefabHigh = objectPrefab; // TODO : Load prefabHigh
        areaPrefab.size = new Vector2Int(objectProperties.sizeX, objectProperties.sizeY);
        areaPrefab.weight = objectProperties.weight;
        
        
        
        // objectsInstantiated.Add(cardObject);
        
        // TODO : Place object on the map in the good area
        
        // Place in good area object
        
        Area area = areas.Find(a => a.data.type == objectProperties.areaType);
        
        int size = area.areaGrid.GetLength(0);
        
        Vector2Int cellLocation = ObjectUtils.FindClosestAreaCell(area, size, objectProperties);
        
        Debug.Log("Cell location : " + cellLocation);
        
        
        if (cellLocation == Vector2Int.zero)
        {
            Debug.Log("No cell found");
        }
        else
        {
            Debug.Log("Cell found : Place Object on the map");
            
            GameObject placedObject = FillArea.PlaceBuilding(areaPrefab, area, cellLocation, false, prefabScale * 5f, uniformScale);
            
            
            // TODO : Initialize object script with objectProperties and gameManager
        
            // Initialize areaPrefab

            placedObject.AddComponent<ObjectScript>();
            
            ObjectScript objectScript = placedObject.GetComponent<ObjectScript>();
            
            objectScript.objectProperties = objectProperties;
            objectScript.gameManager = gameManager;
            objectScript.areaPrefab = areaPrefab;
        }
            
        
        
        // TODO : Update area properties
        
        // ? List objects instantiated
    }
    
    public static void RemoveObjectOnMap(ObjectProperties objectProperties)
    {
        // Remove objects from the map
    }
    
    
    
    
}
