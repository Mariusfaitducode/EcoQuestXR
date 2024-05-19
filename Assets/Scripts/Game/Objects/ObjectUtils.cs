using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectUtils
{



    public static Vector2Int FindClosestAreaCell(Area area, float size, ObjectProperties objectProperties)
    {
        float minDistance = float.MaxValue;
        // AreaCell closestCell = null;
        Vector2Int gridLocation = Vector2Int.zero;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Vector3 newPosition = area.areaGrid[x, y].position;

                if (FillMapUtils.IsVertexInsideCircle(newPosition, area.sphere.transform.position,
                        area.data.radius))
                {
                    if (area.areaGrid[x, y].type == CellType.Empty)
                    {

                        if (CanPlaceBuilding(new Vector2Int(x, y),
                                new Vector2Int(objectProperties.sizeX, objectProperties.sizeY), area.areaGrid))
                        {
                            float distance = Vector3.Distance(newPosition, area.sphere.transform.position);
                        
                            if (distance < minDistance)
                            {
                                minDistance = distance;
                                // closestCell = area.areaGrid[x, y];
                                gridLocation = new Vector2Int(x, y);
                            }
                        }
                    }
                }
            }
        }

        return gridLocation;
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
        
        GameObject placedObject = FillMapUtils.InstantiateObjectWithScale(areaPrefab.prefabLow, area.sphere.transform, position, Quaternion.identity, 
            Vector3.one * (area.areaGrid[gridLocation.x, gridLocation.y].size * prefabSize * scale));
        
        placedObject.AddComponent<ObjectScript>();
        
        ObjectScript objectScript = placedObject.GetComponent<ObjectScript>();
        objectScript.areaCells = areaCells;

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
}
