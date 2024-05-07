using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FillArea
{


    public static void GenerateAreaContent(Area area, float prefabScale)
    {
        Debug.Log("Generate Area Content : " + area.data.type);

        // int count = 0;

        // Generate roads area
        for (int i = 0; i < area.areaGrid.GetLength(0); i++)
        {
            for (int j = 0; j < area.areaGrid.GetLength(0); j++)
            {
                Vector3 newPosition = area.areaGrid[i, j].position;


                if (area.areaGrid[i, j].type == CellType.Road)
                {
                    if (FillMapUtils.IsVertexInsideCircle(newPosition, area.sphere.transform.position,
                            area.data.startRadius))
                    {
                        area.areaGrid[i, j].type = CellType.Road;
                        
                        FillMapUtils.InstantiateObjectWithScale(area.testCube, area.sphere.transform, newPosition,
                            Vector3.one * area.areaGrid[i, j].size);
                        
                    }
                    else
                    {
                        area.areaGrid[i, j].type = CellType.Empty;
                    }
                }
                else if (area.data.type == AreaType.City)
                {
                    if (FillMapUtils.IsVertexInsideCircle(newPosition, area.sphere.transform.position,
                        area.data.startRadius))
                    {
                        AreaPrefab areaPrefab = area.data.prefabs[Random.Range(0, area.data.prefabs.Count)];
                        
                        bool rotate = Random.Range(0, 100) < 30;
                        
                        // Rotate randomly the prefab
                        if (rotate)
                        {
                            areaPrefab.size = new Vector2Int(areaPrefab.size.y, areaPrefab.size.x);
                            // areaPrefab.prefabLow.transform.Rotate(Vector3.up, 90);
                        }
                        
                        if (CanPlaceBuilding(new Vector2Int(i, j), areaPrefab.size, area.areaGrid))
                        {
                            area.areaGrid[i, j].type = CellType.Object;
                            GameObject prefab = areaPrefab.prefabLow;
                            // if (Random.Range(0, 100) < 50)
                            // {
                            //     prefab = areaPrefab.prefabLow;
                            // }
                            PlaceBuilding(areaPrefab, area, new Vector2Int(i, j), rotate, prefabScale);
                        }

                        if (rotate)
                        {
                            areaPrefab.size = new Vector2Int(areaPrefab.size.y, areaPrefab.size.x);
                            // areaPrefab.prefabLow.transform.Rotate(Vector3.up, -90);

                        }
                    }
                }
            }
        }
    }
    
    
    static bool CanPlaceBuilding(Vector2Int position, Vector2Int size, AreaCell[,] areaGrid)
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
    
    static void PlaceBuilding(AreaPrefab areaPrefab, Area area, Vector2Int gridLocation, bool rotate, float prefabSize)
    {
        Vector3 position = Vector3.zero;
        
        // Mettre à jour la grille pour indiquer que les cases sont maintenant occupées
        for (int x = 0; x < areaPrefab.size.x; x++)
        {
            for (int y = 0; y < areaPrefab.size.y; y++)
            {
                area.areaGrid[gridLocation.x + x, gridLocation.y + y].type = CellType.Object;
                
                position += area.areaGrid[gridLocation.x + x, gridLocation.y + y].position;
            }
        }
        
        position /= areaPrefab.size.x * areaPrefab.size.y;
        
        GameObject placedPrefab = FillMapUtils.InstantiateObjectWithScale(areaPrefab.prefabLow, area.sphere.transform, position,
            Vector3.one * (area.areaGrid[gridLocation.x, gridLocation.y].size * prefabSize));

        if (rotate)
        {
            placedPrefab.transform.Rotate(Vector3.up, 90);
        }
    }
    


    public static void SetAreaVerticesInformation(Area area, GameObject meshTerrain, float uniformScale)
    {
        Material material = meshTerrain.GetComponent<Renderer>().material;
        
        Vector2 position = new Vector2(area.sphere.transform.position.x, area.sphere.transform.position.z);

        if (area.data.type == AreaType.City)
        {
            material.SetFloat("_City_Radius", area.data.startRadius * uniformScale);
            material.SetVector("_City_Center", new Vector2(position.x, position.y));
        }
        
        // Debug.Log(meshData);
        // Debug.Log(area);
        //
        // List<int> listAreaIndex = new List<int>(0);
        // List<Vector3> listAreaVertices = new List<Vector3>(0);
        // List<Vector2> listAreaUV = new List<Vector2>(0);
        //
        // for (int i = 0; i < meshData.vertices.Length; i++)
        // {
        //     Vector3 vertex = meshData.vertices[i];
        //     if (FillMapUtils.IsVertexInsideCircle(vertex, area.sphere.transform.position, area.uniformRadius))
        //     {
        //         // Can modify uv information here
        //         
        //         // listAreaIndex.Add(i);
        //         // listAreaVertices.Add(meshData.vertices[i]);
        //         // listAreaUV.Add(meshData.uvs[i]);
        //         //
        //         // meshData.uvs[i] = new Vector2(area.data.areaId, 0);
        //     }
        // }

        // float mean = FillMapUtils.CalculateMean(listAreaVertices);
        //
        // foreach(int index in listAreaIndex)
        // {
        //     meshData.vertices[index] = new Vector3(meshData.vertices[index].x, mean, meshData.vertices[index].z);
        // }

    }
}
