using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectGestion
{
    public static List<GameObject> PlaceObjectsOnMap(ObjectProperties objectProperties, int quantity, List<Area> areas, float prefabScale, GameManager gameManager, float uniformScale)
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
        List<GameObject> placedObjects = new List<GameObject>();;
        
        if (areaCells.Count != 0)
        {
            placedObjects = ObjectUtils.PlaceNeighbourhood(areaCells, area, quantity, areaPrefab, prefabScale, uniformScale);
            
            // Initialize object script with objectProperties gameManager and areaPrefab
            foreach (GameObject placedObject in placedObjects)
            {
                ObjectScript objectScript = placedObject.GetComponent<ObjectScript>();
                
                objectScript.InitObjectScript(objectProperties, gameManager);
            }
        }

        return placedObjects;
    }
    
    public static List<GameObject> RemoveObjectOnMap(ObjectProperties objectProperties, int quantity, List<Area> areas)
    {
        // Find objects with prefabName
        
        Area area = areas.Find(a => a.data.type == objectProperties.areaType);
        
        List<GameObject> objectsToRemove = ObjectUtils.FindAreaObjectsWithPrefabName(area, objectProperties.prefabName);
        
        // Choose objects to remove
        if (objectsToRemove.Count != 0)
        {
            objectsToRemove = ObjectUtils.RemoveNeighbourhood(objectsToRemove, quantity, area);
        }
        
        
        return objectsToRemove;
    }


    public static List<GameObject> UpgradeObjectOnMap(ObjectProperties objectProperties, int quantity1, ObjectProperties objectProperties2, List<Area> areas, bool upgrade)
    {
        
        List<GameObject> upgradedModels = new List<GameObject>();
        
        Area area = areas.Find(a => a.data.type == objectProperties.areaType);
        
        List<GameObject> objectsToUpgrade = ObjectUtils.FindAreaObjectsWithPrefabName(area, objectProperties.prefabName);

        int i = 0;

        foreach (GameObject objectToUpgrade in objectsToUpgrade)
        {
            ObjectScript objectScript = objectToUpgrade.GetComponent<ObjectScript>();
            
            if (i < quantity1 && objectScript.canUpgrade(upgrade, objectProperties2))
            {
                GameObject obj = objectScript.UpgradeObject(objectProperties2, upgrade);
                upgradedModels.Add(obj);
                
                i++;
            }
        }
        return upgradedModels;
    }
    
    
}
