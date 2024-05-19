using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectGestion
{
    public static void PlaceObjectOnMap(ObjectProperties objectProperties, int quantity, List<Area> areas, float prefabScale, GameManager gameManager, float uniformScale)
    {
        // Load Prefab
        
        GameObject objectPrefab = ObjectsInitialization.LoadPrefab(objectProperties);
        
        // TODO : Load prefabLow + prefabHigh
        
        AreaPrefab areaPrefab = new AreaPrefab();
        areaPrefab.prefabLow = objectPrefab;
        areaPrefab.prefabHigh = objectPrefab; // TODO : Load prefabHigh
        areaPrefab.size = new Vector2Int(objectProperties.sizeX, objectProperties.sizeY);
        areaPrefab.weight = objectProperties.weight;
        
        // TODO : Place object on the map in the good area
        
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
            
            
            // TODO : Initialize object script with objectProperties and gameManager
            // Initialize areaPrefab

            foreach (GameObject placedObject in placedObjects)
            {
                ObjectScript objectScript = placedObject.GetComponent<ObjectScript>();
                
                objectScript.InitObjectScript(objectProperties, gameManager, areaPrefab);
            }
        }
            
        
        
        // TODO : Update area properties
        
        // ? List objects instantiated
        
        // objectsInstantiated.Add(cardObject);
    }
    
    public static void RemoveObjectOnMap(ObjectProperties objectProperties)
    {
        // Remove objects from the map
    }
    
    
    
    
}
