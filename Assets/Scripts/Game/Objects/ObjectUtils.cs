using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectUtils
{

    public static List<GameObject> PlaceNeighbourhood(List<AreaCell> areaCells, Area area, int quantity, AreaPrefab areaPrefab, float prefabScale, float uniformScale)
    {
        float minDistance = float.MaxValue;
        Vector2Int firstLocation = Vector2Int.zero;
        AreaCell firstCell = null;
        
        List<GameObject> placedObjects = new List<GameObject>();

        foreach (AreaCell areaCell in areaCells)
        {
            float distance = Vector3.Distance(areaCell.position, area.sphere.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                firstCell = areaCell;
                firstLocation = areaCell.gridPosition;
            }
        }
        areaCells.Remove(firstCell);
        

        
        GameObject placedObject = PlaceBuilding(areaPrefab, area, firstLocation, false, prefabScale, uniformScale);
        placedObjects.Add(placedObject);
        quantity--;

        
        while (quantity > 0)
        {
            minDistance = float.MaxValue;
            Vector2Int newLocation = Vector2Int.zero;
            
            foreach (AreaCell areaCell in areaCells)
            {
                float distance = Vector3.Distance(areaCell.position, area.areaGrid[firstLocation.x, firstLocation.y].position);

                if (CanPlaceBuilding(areaCell.gridPosition, areaPrefab.size, area.areaGrid) && distance < minDistance)
                {
                    minDistance = distance;
                    newLocation = areaCell.gridPosition;
                }
            }
            GameObject placedObject2 = PlaceBuilding(areaPrefab, area, newLocation, false, prefabScale, uniformScale);
            placedObjects.Add(placedObject2);

            quantity--;
        }

        return placedObjects;
    }
    

    public static List<AreaCell> FindEmptyAreaCellsInPeriphery(Area area, float size, ObjectProperties objectProperties)
    {
        
        // AreaCell closestCell = null;
        
        List<AreaCell> areaCells = new List<AreaCell>();

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Vector3 newPosition = area.areaGrid[x, y].position;

                if (area.areaGrid[x, y].inArea 
                    && !area.areaGrid[x, y].inStartArea)
                {
                    if (area.areaGrid[x, y].type == CellType.Empty)
                    {

                        if (CanPlaceBuilding(new Vector2Int(x, y),
                                new Vector2Int(objectProperties.sizeX, objectProperties.sizeY), area.areaGrid))
                        {
                            areaCells.Add(area.areaGrid[x, y]);
                            
                            Debug.DrawLine(newPosition, newPosition + Vector3.up * 1000, Color.green, 60);
                        }
                    }
                }
            }
        }

        return areaCells;
    }
    
    
    public static bool CanPlaceBuilding(Vector2Int position, Vector2Int size, AreaCell[,] areaGrid)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if (position.x + x >= areaGrid.GetLength(0) || position.y + y >= areaGrid.GetLength(1))
                {
                    return false;
                }
                if (areaGrid[position.x + x, position.y + y].type != CellType.Empty)
                {
                    return false;
                }
            }
        }
        return true;
    }
    
    
    public static GameObject PlaceBuilding(AreaPrefab areaPrefab, Area area, Vector2Int gridLocation, bool rotate, float prefabSize, float scale = 1)
    {
        Vector3 position = Vector3.zero;
        
        List<AreaCell> areaCells = new List<AreaCell>();
        
        // Calculate position
        for (int x = 0; x < areaPrefab.size.x; x++)
        {
            for (int y = 0; y < areaPrefab.size.y; y++)
            {
                AreaCell areaCell = area.areaGrid[gridLocation.x + x, gridLocation.y + y];
                
                position += areaCell.position;
                
                areaCells.Add(areaCell);
            }
        }
        
        position /= areaPrefab.size.x * areaPrefab.size.y;
        
        position *= scale;
        
        // Debug.Log("Final position"position);
        
        GameObject placedObject = FillMapUtils.InstantiateObjectWithScale(areaPrefab.prefabLow, area.hierarchyBuildingFolder.transform, position, Quaternion.identity, 
            Vector3.one * (area.areaGrid[gridLocation.x, gridLocation.y].size * prefabSize * scale));
        
        placedObject.AddComponent<ObjectScript>();
        
        ObjectScript objectScript = placedObject.GetComponent<ObjectScript>();
        objectScript.areaCells = areaCells;
        objectScript.areaPrefab = areaPrefab;

        // Update area informations
        area.areaObjects.Add(placedObject);
        
        foreach (AreaCell areaCell in areaCells)
        {
            areaCell.hasObject = true;
            areaCell.type = CellType.Object;
            areaCell.objectPrefab = placedObject;
        }
        
        // Debug.DrawLine(position, position + Vector3.up * 10, Color.red, 20);

        if (rotate)
        {
            placedObject.transform.Rotate(Vector3.up, 90);
        }
        
        return placedObject;
    }
    
    
    
    public static List<GameObject> FindAreaObjectsWithPrefabName(Area area, string prefabName)
    {
        List<GameObject> objects = new List<GameObject>();

        foreach (GameObject obj in area.areaObjects)
        {
            ObjectScript objectScript = obj.GetComponent<ObjectScript>();
            
            if (objectScript != null && objectScript.objectProperties != null && objectScript.objectProperties.prefabName == prefabName)
            {
                objects.Add(obj);
            }
        }
        
        return objects;
    }


    public static void RemoveNeighbourhood(List<GameObject> validObjectsToRemove, int quantity, Area area)
    {
        List<GameObject> objectsToRemove = new List<GameObject>();
        
        GameObject firstObject = validObjectsToRemove[Random.Range(0, validObjectsToRemove.Count)];

        validObjectsToRemove.Remove(firstObject);
        objectsToRemove.Add(firstObject);
        
        quantity--;
        
        while (quantity > 0)
        {
            float minDistance = float.MaxValue;
            GameObject newObject = null;
            
            foreach (GameObject obj in validObjectsToRemove)
            {
                float distance = Vector3.Distance(firstObject.transform.position, obj.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    newObject = obj;
                }
            }
            validObjectsToRemove.Remove(newObject);
            objectsToRemove.Add(newObject);
            quantity--;
        }
        
        // Delete and update area informations
        foreach (GameObject objectToRemove in objectsToRemove)
        {
            ObjectScript objectScript = objectToRemove.GetComponent<ObjectScript>();
            
            foreach (AreaCell areaCell in objectScript.areaCells)
            {
                areaCell.hasObject = false;
                areaCell.type = CellType.Empty;
                areaCell.objectPrefab = null;
            }
            
            area.areaObjects.Remove(objectToRemove);
            Object.Destroy(objectToRemove);
        }
    }
    
}
