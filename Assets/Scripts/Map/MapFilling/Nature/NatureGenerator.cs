using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class NatureGenerator
{

    [Serializable]
    public class NatureData
    {
        public GameObject natureParent;
        public int treeQuantity = 300;
        public float minTreeDistance = 5f;
        
        public float prefabSizeMultiplier = 3f;

        public Renderer noiseRenderer;
        public NoiseData forestNoise;
        
        public List<ForestData> forestDatas;
        
        public List<NaturePrefab> naturePrefabs;

        public int maxTries = 100;
    }
    
    [Serializable]
    public struct NaturePrefab
    {
        public GameObject gameObject;
    }
    
    [Serializable]
    public struct ForestData
    {
        [Range(0,1)]
        public float forestDensity;
        public int radiusMin;
        public int radiusMax;
        public float minTreeDistance;
        public GameObject forestParent;
    }
    
    public static void GenerateNature(List<Area> areas, NatureData natureData, MapGenerator mapGenerator, MapDisplay mapDisplay, float minHeight, float prefabSize, List<Vector3> roadVertices)
    {
        MeshData meshData = mapGenerator.meshData;
        List<Vector3> meshVertices = new List<Vector3>(meshData.vertices);
        
        int placedTrees = 0;
        
        List<Vector3> exploredPositions = new List<Vector3>();

        // Generate global trees randomly on the map
        while (placedTrees < natureData.treeQuantity)
        {
            PlaceTree(meshVertices, minHeight, areas, exploredPositions, natureData.minTreeDistance, 
                natureData.naturePrefabs, natureData.natureParent, prefabSize, natureData.prefabSizeMultiplier, roadVertices, ref placedTrees);
        }

        float[,] mapNoise = mapGenerator.mapData.heightMap;
        
        mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(mapNoise), natureData.noiseRenderer);

        // Generate forests

        // Generate forests
        // int placedForests = 0;
        foreach(ForestData forestData in natureData.forestDatas)
        {
            int randomIndex = Random.Range(0, meshVertices.Count);
            Vector3 forestCenter = meshVertices[randomIndex];
            
            int forestRadius = Random.Range(forestData.radiusMin, forestData.radiusMax);
        
            bool validArea = IsValidForestPosition(forestCenter, forestRadius, areas);
        
            int i = 0;
            
            while (!validArea && i < natureData.maxTries)
            
            {
                randomIndex = Random.Range(0, meshVertices.Count);
                forestCenter = meshVertices[randomIndex];
                forestRadius = Random.Range(forestData.radiusMin, forestData.radiusMax);
                
                validArea = IsValidForestPosition(forestCenter, forestRadius, areas);
                i++;
            }
            
            if (validArea)
            {
                List<Vector3> forestVertices = new List<Vector3>();
                
                foreach (Vector3 vertex in meshVertices)
                {
                    if (FillMapUtils.IsVertexInsideCircle(vertex, forestCenter, forestRadius))
                    {
                        // Add vertex to list
                        forestVertices.Add(vertex);
                    }
                }
                
                foreach (Vector3 vertex in forestVertices)
                {
                    
                    float centerDistance = Vector3.Distance(vertex, forestCenter);
                    
                    if (((float)Random.Range(0, 100) / 100) < forestData.forestDensity * 10 / centerDistance)
                    {
                        bool valid = IsValidNaturePosition(vertex, minHeight, areas, exploredPositions, roadVertices, forestData.minTreeDistance);
                    
                        if (valid)
                        {
                            exploredPositions.Add(vertex);
                        
                            GameObject naturePrefab = natureData.naturePrefabs[Random.Range(0, natureData.naturePrefabs.Count)].gameObject;
                        
                            GameObject placedPrefab = FillMapUtils.InstantiateObjectWithScale(naturePrefab, forestData.forestParent.transform, vertex, Quaternion.identity, 
                                naturePrefab.transform.localScale * prefabSize * natureData.prefabSizeMultiplier);
                        
                            // placedTreesInForest++;
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Forest Area Not found");
            }
        }
    }


    public static void PlaceTree(List<Vector3> vertices, float minHeight, List<Area> areas, List<Vector3> exploredPositions, 
       float minTreeDistance, List<NaturePrefab> naturePrefabs, GameObject natureParent, float prefabSize, float prefabSizeMultiplier, List<Vector3> roadVertices, ref int placedTrees)
    {
        // Choisir un point aléatoire
        
        int randomIndex = Random.Range(0, vertices.Count);
        Vector3 newPosition = vertices[randomIndex];
            
        // Vérifier si il est valide
            
        bool valid = IsValidNaturePosition(newPosition, minHeight, areas, exploredPositions, roadVertices, minTreeDistance);
        
        // Placer la nature
            
        if (valid)
        {
            exploredPositions.Add(newPosition);
                
            GameObject naturePrefab = naturePrefabs[Random.Range(0, naturePrefabs.Count)].gameObject;
                
            GameObject placedPrefab = FillMapUtils.InstantiateObjectWithScale(naturePrefab, natureParent.transform, newPosition, Quaternion.identity, 
                naturePrefab.transform.localScale * prefabSize * prefabSizeMultiplier);
                
            placedTrees++;
        } 
    }
    
    public static bool IsValidNaturePosition(Vector3 position, float minHeight, List<Area> areas, List<Vector3> exploredPositions, List<Vector3> roadVertices, float minTreeDistance)
    {
        // Not in water
        if (position.y < minHeight)
        {
            return false;
        }

        // Not in area
        foreach (Area area in areas)
        {
            if (FillMapUtils.IsVertexInsideCircle(position, area.sphere.transform.position, area.data.radius))
            {
                return false;
            }
        }
        
        // Not in road
        foreach (Vector3 roadVertex in roadVertices)
        {
            if (Vector3.Distance(position, roadVertex) < minTreeDistance)
            {
                return false;
            }
        }
        
        // Not too close to other nature
        foreach (Vector3 exploredPosition in exploredPositions)
        {
            if (Vector3.Distance(position, exploredPosition) < minTreeDistance)
            {
                return false;
            }
        }

        return true;
    }
    
    
    public static bool IsValidForestPosition(Vector3 position, int forestRadius, List<Area> areas)
    {
        foreach (Area area in areas)
        {
            if (FillMapUtils.AreSpheresColliding(position, forestRadius, area.sphere.transform.position, area.data.radius))
            {
                return false;
            }
        }

        return true;
    }
}
