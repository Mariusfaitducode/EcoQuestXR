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

                        if (FillArea.CanPlaceBuilding(new Vector2Int(x, y),
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
    
    
    public static void PlaceObject(ObjectProperties objectProperties, GameObject cardObject, Area area, Vector2Int gridLocation, bool rotate, float prefabSize)
    {
        Vector3 position = Vector3.zero;
        
        // Mettre à jour la grille pour indiquer que les cases sont maintenant occupées
        for (int x = 0; x < objectProperties.sizeX; x++)
        {
            for (int y = 0; y < objectProperties.sizeY; y++)
            {
                area.areaGrid[gridLocation.x + x, gridLocation.y + y].type = CellType.Object;
                
                position += area.areaGrid[gridLocation.x + x, gridLocation.y + y].position;
            }
        }
        
        position /= objectProperties.sizeX * objectProperties.sizeY;
        
        GameObject placedPrefab = FillMapUtils.InstantiateObjectWithScale(cardObject, area.sphere.transform, position, Quaternion.identity, 
            Vector3.one * (area.areaGrid[gridLocation.x, gridLocation.y].size * prefabSize));

        if (rotate)
        {
            placedPrefab.transform.Rotate(Vector3.up, 90);
        }
    }
}
