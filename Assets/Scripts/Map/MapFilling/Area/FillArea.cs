using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FillArea
{

    public static void GenerateAreaContent(Area area, float prefabScale, RoadGenerator.RoadData roadData, MapDisplay mapDisplay)
    {
        // Debug.Log("Generate Area Content : " + area.data.type);

        // int count = 0;
        
        int size = area.areaGrid.GetLength(0);

        float[,] areaNoiseMap = Noise.GenerateNoiseMap(size, size, area.areaNoise.seed, area.areaNoise.noiseScale, 
            area.areaNoise.octaves, area.areaNoise.persistance, area.areaNoise.lacunarity, Vector2.zero, area.areaNoise.normalizeMode);

        mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(areaNoiseMap), area.noiseRenderer);
        
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Vector3 newPosition = area.areaGrid[x, y].position;

                // Generate roads 
                if (area.areaGrid[x, y].type == CellType.Road)
                {
                    if (FillMapUtils.IsVertexInsideCircle(newPosition, area.sphere.transform.position,
                            area.data.radius))
                    {
                        bool up = (y < size - 1) && area.areaGrid[x, y + 1].type == CellType.Road;
                        bool right = (x < size - 1) && area.areaGrid[x + 1, y].type == CellType.Road;
                        bool down = (y > 0) && area.areaGrid[x, y - 1].type == CellType.Road;
                        bool left = (x > 0) && area.areaGrid[x - 1, y].type == CellType.Road;
                        
                        RoadGenerator.RoadTile result = RoadGenerator.FindGoodRoadTile(up, right, down, left, roadData.roadTiles)!;
                        
                        
                        FillMapUtils.InstantiateObjectWithScale(result.tile, area.sphere.transform, newPosition, result.rotation, 
                            Vector3.one * (area.areaGrid[x, y].size * prefabScale));
                        
                    }
                    else
                    {
                        area.areaGrid[x, y].type = CellType.Empty;
                    }
                }
                // Generate buildings 
                else if (area.data.prefabs.Count > 0 && area.areaGrid[x, y].type == CellType.Empty)
                {
                    if (FillMapUtils.IsVertexInsideCircle(newPosition, area.sphere.transform.position,
                        area.data.startRadius))
                    {
                        // if (Random.Range(0, 100) < area.data.fillPercent * 100)
                        // {
                        //     continue;
                        // }
                        
                        AreaPrefab areaPrefab = ChooseRandomPrefab(area.data.prefabs, areaNoiseMap[x, y]);
                        
                        if (areaPrefab.prefabLow == null)
                        {
                            continue;
                        }
                        
                        bool rotate = Random.Range(0, 100) < 30;
                        
                        // Rotate randomly the prefab
                        if (rotate)
                        {
                            areaPrefab.size = new Vector2Int(areaPrefab.size.y, areaPrefab.size.x);
                            // areaPrefab.prefabLow.transform.Rotate(Vector3.up, 90);
                        }
                        
                        if (CanPlaceBuilding(new Vector2Int(x, y), areaPrefab.size, area.areaGrid))
                        {
                            PlaceBuilding(areaPrefab, area, new Vector2Int(x, y), rotate, prefabScale);
                        }

                        if (rotate) // Reset size
                        {
                            areaPrefab.size = new Vector2Int(areaPrefab.size.y, areaPrefab.size.x);
                        }
                    }
                }
            }
        }
    }


    public static AreaPrefab ChooseRandomPrefab(List<AreaPrefab> areaPrefabs, float noiseWeight)
    {
        
        float totalWeight = 0;
        foreach (AreaPrefab areaPrefab in areaPrefabs)
        {
            totalWeight += areaPrefab.weight;
        }
        
        // float random = Random.Range(0, totalWeight);

        noiseWeight *= totalWeight;
        float currentPoint = 0;
        
        foreach (AreaPrefab areaPrefab in areaPrefabs)
        {
            currentPoint += areaPrefab.weight;
            if (currentPoint >= noiseWeight)
            {
                return areaPrefab;
            }
        }

        return new AreaPrefab();
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
        
        position *= scale;
        
        // Debug.Log("Final position"position);
        
        GameObject placedPrefab = FillMapUtils.InstantiateObjectWithScale(areaPrefab.prefabLow, area.sphere.transform, position, Quaternion.identity, 
            Vector3.one * (area.areaGrid[gridLocation.x, gridLocation.y].size * prefabSize * scale));
        
        Debug.DrawLine(position, position + Vector3.up * 10, Color.red, 20);

        if (rotate)
        {
            placedPrefab.transform.Rotate(Vector3.up, 90);
        }
        
        return placedPrefab;
    }


    public static void SetAreaShader(Area area, GameObject meshTerrain, float uniformScale)
    {
        Material material = meshTerrain.GetComponent<Renderer>().material;
        
        Vector2 position = new Vector2(area.sphere.transform.position.x, area.sphere.transform.position.z);

        if (area.data.type == AreaType.City)
        {
            material.SetFloat("_City_Radius", area.data.radius * uniformScale);
            material.SetFloat("_City_Start_Radius", area.data.startRadius * uniformScale);
            material.SetVector("_City_Center", new Vector2(position.x, position.y));
        }
        
        if (area.data.type == AreaType.Industry)
        {
            material.SetFloat("_Industry_Radius", area.data.radius * uniformScale);
            material.SetFloat("_Industry_Start_Radius", area.data.startRadius * uniformScale);
            material.SetVector("_Industry_Center", new Vector2(position.x, position.y));
        }
        
        if (area.data.type == AreaType.Energy)
        {
            material.SetFloat("_Energy_Radius", area.data.radius * uniformScale);
            material.SetFloat("_Energy_Start_Radius", area.data.startRadius * uniformScale);
            material.SetVector("_Energy_Center", new Vector2(position.x, position.y));
        }
        
        if (area.data.type == AreaType.Agriculture)
        {
            material.SetFloat("_Agriculture_Radius", area.data.radius * uniformScale);
            material.SetFloat("_Agriculture_Start_Radius", area.data.startRadius * uniformScale);
            material.SetVector("_Agriculture_Center", new Vector2(position.x, position.y));
        }
    }

    public static void SetAreaVerticesInformation(Area area, MeshData meshData, float uniformScale)
    {
        
        List<int> listAreaIndex = new List<int>(0);
        List<Vector3> listAreaVertices = new List<Vector3>(0);
        
        // List<Vector2> listAreaUV = new List<Vector2>(0);
        
        for (int i = 0; i < meshData.vertices.Length; i++)
        {
            Vector3 vertex = meshData.vertices[i] * uniformScale;
            if (FillMapUtils.IsVertexInsideCircle(vertex, area.sphere.transform.position, area.data.radius * uniformScale))
            {
                
                listAreaIndex.Add(i);
                listAreaVertices.Add(meshData.vertices[i]);
            }
        }

        float mean = FillMapUtils.CalculateMean(listAreaVertices);
        
        foreach(int index in listAreaIndex)
        {
            // Debug.DrawLine(meshData.vertices[index] *uniformScale, new Vector3(meshData.vertices[index].x, mean, meshData.vertices[index].z) * uniformScale, Color.blue, 60);
            meshData.vertices[index] = new Vector3(meshData.vertices[index].x, mean, meshData.vertices[index].z);
        }
        
        

    }
}
