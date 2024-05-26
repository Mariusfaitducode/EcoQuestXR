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

        // public Renderer noiseRenderer;
        public NoiseData forestNoise;
        
        // public List<ForestData> forestDatas;
        
        public List<NaturePrefab> naturePrefabs;

        public int maxTries = 100;
    }
    
    [Serializable]
    public struct NaturePrefab
    {
        public GameObject gameObject;
        [Range(0,1)]
        public float preferredHeight;
        
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
        
        // int placedTrees = 0;
        
        List<Vector3> exploredPositions = new List<Vector3>();

        // Generate global trees randomly on the map
        // while (placedTrees < natureData.treeQuantity)
        // {
        //     PlaceTree(meshVertices, minHeight, areas, exploredPositions, natureData.minTreeDistance, 
        //         natureData.naturePrefabs, natureData.natureParent, prefabSize, natureData.prefabSizeMultiplier, roadVertices, ref placedTrees);
        // }

        int size = mapGenerator.mapData.heightMap.GetLength(0);
        
        float[,] forestNoiseMap = Noise.GenerateNoiseMap(size, size, natureData.forestNoise.seed, natureData.forestNoise.noiseScale, 
            natureData.forestNoise.octaves, natureData.forestNoise.persistance, natureData.forestNoise.lacunarity, Vector2.zero, natureData.forestNoise.normalizeMode);
        
        float[,] mapNoise = mapGenerator.mapData.heightMap;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                
                int index = meshData.vertexIndicesMap[x, y];
                
                if (index >= 0)
                {
                    if (forestNoiseMap[x, y]  > 0.6f || mapNoise[x, y] > 0.8f)
                    {

                        Vector3 newPosition = meshData.vertices[index];
                    
                        bool valid = IsValidNaturePosition(newPosition, minHeight, areas, exploredPositions, roadVertices, natureData.minTreeDistance);
                    
                        if (valid)
                        {
                            exploredPositions.Add(newPosition);

                            GameObject naturePrefab = ChooseRandomTree(natureData.naturePrefabs, mapNoise[x, y]);
                            
                            if (naturePrefab != null)
                            {
                                Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                                
                                GameObject placedPrefab = FillMapUtils.InstantiateObjectWithScale(naturePrefab, natureData.natureParent.transform, newPosition, rotation, 
                                    naturePrefab.transform.localScale * prefabSize * natureData.prefabSizeMultiplier * Random.Range(0.8f, 1.2f));

                                MeshRenderer renderer = placedPrefab.GetComponent<MeshRenderer>();

                                Material[] newMaterials = new Material[renderer.sharedMaterials.Length];
                                for (int i = 0; i < newMaterials.Length; i++)
                                {
                                    // Clone chaque matériau
                                    newMaterials[i] = new Material(renderer.sharedMaterials[i]);
                                    // Appliquer une variation de couleur
                                    newMaterials[i].color = FillMapUtils.GetRandomColorVariation(newMaterials[i].color, Random.Range(0, 0.1f));
                                }
                                renderer.materials = newMaterials;
                            }
                            // placedTreesInForest++;
                        }
                    }
                    
                }
            }
        }
        
        
        // mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(mapNoise), natureData.noiseRenderer);

        // Generate forests

        // Generate forests
        // int placedForests = 0;
        
        // foreach(ForestData forestData in natureData.forestDatas)
        // {
        //     int randomIndex = Random.Range(0, meshVertices.Count);
        //     Vector3 forestCenter = meshVertices[randomIndex];
        //     
        //     int forestRadius = Random.Range(forestData.radiusMin, forestData.radiusMax);
        //
        //     bool validArea = IsValidForestPosition(forestCenter, forestRadius, areas);
        //
        //     int i = 0;
        //     
        //     while (!validArea && i < natureData.maxTries)
        //     
        //     {
        //         randomIndex = Random.Range(0, meshVertices.Count);
        //         forestCenter = meshVertices[randomIndex];
        //         forestRadius = Random.Range(forestData.radiusMin, forestData.radiusMax);
        //         
        //         validArea = IsValidForestPosition(forestCenter, forestRadius, areas);
        //         i++;
        //     }
        //     
        //     if (validArea)
        //     {
        //         List<Vector3> forestVertices = new List<Vector3>();
        //         
        //         foreach (Vector3 vertex in meshVertices)
        //         {
        //             if (FillMapUtils.IsVertexInsideCircle(vertex, forestCenter, forestRadius))
        //             {
        //                 // Add vertex to list
        //                 forestVertices.Add(vertex);
        //             }
        //         }
        //         
        //         foreach (Vector3 vertex in forestVertices)
        //         {
        //             
        //             float centerDistance = Vector3.Distance(vertex, forestCenter);
        //             
        //             if (((float)Random.Range(0, 100) / 100) < forestData.forestDensity * 10 / centerDistance)
        //             {
        //                 bool valid = IsValidNaturePosition(vertex, minHeight, areas, exploredPositions, roadVertices, forestData.minTreeDistance);
        //             
        //                 if (valid)
        //                 {
        //                     exploredPositions.Add(vertex);
        //                 
        //                     GameObject naturePrefab = natureData.naturePrefabs[Random.Range(0, natureData.naturePrefabs.Count)].gameObject;
        //                 
        //                     GameObject placedPrefab = FillMapUtils.InstantiateObjectWithScale(naturePrefab, natureData.natureParent.transform, vertex, Quaternion.identity, 
        //                         naturePrefab.transform.localScale * prefabSize * natureData.prefabSizeMultiplier);
        //                 
        //                     // placedTreesInForest++;
        //                 }
        //             }
        //         }
        //     }
        //     else
        //     {
        //         Debug.Log("Forest Area Not found");
        //     }
        // }
    }


    public static GameObject ChooseRandomTree(List<NaturePrefab> naturePrefabs, float noiseHeight)
    {
        float randomValue = Random.Range(0f, 1f);

        List<GameObject> possiblePrefabs = new List<GameObject>();
        List<float> probabilities = new List<float>();

        foreach (NaturePrefab naturePrefab in naturePrefabs)
        {
            float probability = FillMapUtils.GaussianProbability(noiseHeight, naturePrefab.preferredHeight, 0.2f);
            
            if (probability > 0)
            {
                possiblePrefabs.Add(naturePrefab.gameObject);
                probabilities.Add(probability);
            }
        }
        
        if (possiblePrefabs.Count > 0)
        {
            float totalWeight = 0;
            foreach (float prob in probabilities)
            {
                totalWeight += prob;
            }
            
            randomValue *= totalWeight;
            
            float currentPoint = 0;
            
            for (int i = 0; i < possiblePrefabs.Count; i++)
            {
                currentPoint += probabilities[i];
                if (currentPoint >= randomValue)
                {
                    return possiblePrefabs[i];
                }
            }
        }

        return null;
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
        // foreach (Vector3 exploredPosition in exploredPositions)
        // {
        //     if (Vector3.Distance(position, exploredPosition) < minTreeDistance)
        //     {
        //         return false;
        //     }
        // }

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
