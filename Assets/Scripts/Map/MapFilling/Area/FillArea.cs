using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FillArea
{

    public static void InstantiateRoadsAndBuildingsOnArea(Area area, float prefabScale, RoadGenerator.RoadData roadData, MapDisplay mapDisplay)
    {
        int size = area.areaGrid.GetLength(0);

        float[,] areaNoiseMap = Noise.GenerateNoiseMap(size, size, area.areaNoise.seed, area.areaNoise.noiseScale, 
            area.areaNoise.octaves, area.areaNoise.persistance, area.areaNoise.lacunarity, Vector2.zero, area.areaNoise.normalizeMode);

        mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(areaNoiseMap), area.noiseRenderer);
        
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Vector3 newPosition = area.areaGrid[x, y].cellPosition.transform.position;

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
                        
                        
                        FillMapUtils.InstantiateObjectWithScale(result.tile, area.hierarchyRoadFolder.transform, newPosition, result.rotation, 
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
                        
                        if (ObjectUtils.CanPlaceBuilding(new Vector2Int(x, y), areaPrefab.size, area.areaGrid))
                        {
                            ObjectUtils.PlaceBuilding(areaPrefab, area, new Vector2Int(x, y), rotate, prefabScale);
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
