using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectGestion
{
    public static void PlaceObjectsOnMap(ObjectProperties objectProperties, int quantity, List<Area> areas, float prefabScale, GameManager gameManager, float uniformScale)
    {
        // Load Prefab
        
        GameObject objectPrefab = ObjectsInitialization.LoadPrefab(objectProperties);
        
        // TODO : Load prefabLow + prefabHigh
        
        AreaPrefab areaPrefab = new AreaPrefab();
        areaPrefab.prefabLow = objectPrefab;
        // areaPrefab.prefabHigh = objectPrefab; // TODO : Load prefabHigh
        areaPrefab.size = new Vector2Int(objectProperties.sizeX, objectProperties.sizeY);
        areaPrefab.weight = objectProperties.weight;
        
        // Place in good area object
        
        Area area = areas.Find(a => a.data.type == objectProperties.areaType);
        int size = area.areaGrid.GetLength(0);
        
        List<AreaCell> areaCells = ObjectUtils.FindEmptyAreaCellsInPeriphery(area, size, objectProperties);
        
        if (areaCells.Count == 0)
        {
            Debug.LogWarning("No cell found");
        }
        else
        {
            Debug.Log("Cell found : Place Object on the map");
            
            List<GameObject> placedObjects = ObjectUtils.PlaceNeighbourhood(areaCells, area, quantity, areaPrefab, prefabScale, uniformScale);
            
            // Initialize object script with objectProperties gameManager and areaPrefab
            
            foreach (GameObject placedObject in placedObjects)
            {
                ObjectScript objectScript = placedObject.GetComponent<ObjectScript>();
                
                objectScript.InitObjectScript(objectProperties, gameManager);
            }
        }
    }
    
    public static void RemoveObjectOnMap(ObjectProperties objectProperties, int quantity, List<Area> areas)
    {
        // Find objects with prefabName
        
        Area area = areas.Find(a => a.data.type == objectProperties.areaType);
        
        List<GameObject> objectsToRemove = ObjectUtils.FindAreaObjectsWithPrefabName(area, objectProperties.prefabName);
        
        // Choose one object to remove
        
        if (objectsToRemove.Count == 0)
        {
            Debug.LogWarning("No object found to remove");
        }
        else
        {
            ObjectUtils.RemoveNeighbourhood(objectsToRemove, quantity, area);
        }
    }
    
    
    
    
}