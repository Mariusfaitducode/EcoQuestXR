using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FillArea
{


    public static void GenerateAreaContent(Area area, int[,] roads, float mapSize,float scale)
    {
        Debug.Log("Generate Area Content : " + area.data.type);

        // int count = 0;

        // Generate roads area
        for (int i = 0; i < area.areaGrid.GetLength(0); i++)
        {
            for (int j = 0; j < area.areaGrid.GetLength(0); j++)
            {
                Vector3 newPosition = area.areaGrid[i, j].position;


                if (roads[i, j] == 1)
                {
                    if (FillMapUtils.IsVertexInsideCircle(newPosition, area.sphere.transform.position,
                            area.uniformStartRadius))
                    {
                        area.areaGrid[i, j].type = CellType.Road;
                        
                        FillMapUtils.InstantiateObjectWithScale(area.testCube, area.sphere.transform, newPosition,
                            Vector3.one * area.areaGrid[i, j].size);
                        
                    }
                // }
                // else
                // {
                //     Debug.Log("NO ROAD");
                //     
                //     
                //     if (FillMapUtils.IsVertexInsideCircle(newPosition, area.sphere.transform.position,
                //         area.uniformStartRadius))
                //     {
                //
                //         if (Random.Range(0, 100) < 50)
                //         {
                //             area.areaGrid[i, j].type = CellType.Object;
                //             
                //             GameObject prefab = area.data.prefabs[Random.Range(0, area.data.prefabs.Count)].prefabLow;
                //             
                //             
                //             // GameObject placedPrefab = FillMapUtils.InstantiateObjectWithScale(prefab, area.sphere.transform, newPosition,
                //             //     Vector3.one * area.areaGrid[i, j].size);
                //
                //
                //             // placedPrefab.transform.localScale = area.areaGrid[i, j].size * Vector3.one;
                //
                //             // count++;
                //         }
                //     }
                //     
                }
            }
        }
    }
    
    
    public static void PlacePrefabOnArea(Area area)
    {
        
    }


    public static void SetAreaVerticesInformation(Area area, MeshData meshData)
    {
        
        Debug.Log(meshData);
        Debug.Log(area);

        List<int> listAreaIndex = new List<int>(0);
        List<Vector3> listAreaVertices = new List<Vector3>(0);
        List<Vector2> listAreaUV = new List<Vector2>(0);
        
        for (int i = 0; i < meshData.vertices.Length; i++)
        {
            Vector3 vertex = meshData.vertices[i];
            if (FillMapUtils.IsVertexInsideCircle(vertex, area.sphere.transform.position, area.uniformRadius))
            {
                // Can modify uv information here
                
                listAreaIndex.Add(i);
                listAreaVertices.Add(meshData.vertices[i]);
                listAreaUV.Add(meshData.uvs[i]);
                if (area.data.type == AreaType.City)
                {
                    meshData.vertices[i] = new Vector3(vertex.x, 20, vertex.z);
                    meshData.uvs[i] = new Vector2(area.data.areaId, 0);
                }
            }
        }

        float mean = FillMapUtils.CalculateMean(listAreaVertices);
        
        foreach(int index in listAreaIndex)
        {
            meshData.vertices[index] = new Vector3(meshData.vertices[index].x, mean, meshData.vertices[index].z);
        }

    }
}
