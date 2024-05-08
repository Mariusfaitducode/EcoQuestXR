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
        
        public int forestQuantity = 3;
        
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
    
    public static void GenerateNature(List<Area> areas, NatureData natureData, MeshData meshData, float minHeight, float prefabSize)
    {
        List<Vector3> meshVertices = new List<Vector3>(meshData.vertices);
        
        int placedTrees = 0;
        
        List<Vector3> exploredPositions = new List<Vector3>();

        while (placedTrees < natureData.treeQuantity)
        {
            PlaceTree(meshVertices, minHeight, areas, exploredPositions, natureData.minTreeDistance, 
                natureData.naturePrefabs, natureData.natureParent, prefabSize, natureData.prefabSizeMultiplier, ref placedTrees);
        }
        
        // Generate forests
        // int placedForests = 0;
        foreach(ForestData forestData in natureData.forestDatas)
        
        {
            int randomIndex = Random.Range(0, meshVertices.Count);
            Vector3 forestCenter = meshVertices[randomIndex];
            
            int forestRadius = Random.Range(10, 20);

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
                // float areaSurface = Mathf.PI * forestRadius * forestRadius;
                // int treeQuantity = (int) (areaSurface * natureData.forestDensity);
                
                // Get area vertices

                List<Vector3> forestVertices = new List<Vector3>();
                
                foreach (Vector3 vertex in meshVertices)
                {
                    if (FillMapUtils.IsVertexInsideCircle(vertex, forestCenter, forestRadius))
                    {
                        // Add vertex to list
                        forestVertices.Add(vertex);
                    }
                }
                
                // Generate trees
                
                // int placedTreesInForest = 0;
                
                foreach (Vector3 vertex in forestVertices)
                {
                    
                    float centerDistance = Vector3.Distance(vertex, forestCenter);
                    
                    if (((float)Random.Range(0, 100) / 100) < forestData.forestDensity * 10 / centerDistance)
                    {
                        bool valid = IsValidNaturePosition(vertex, minHeight, areas, exploredPositions, forestData.minTreeDistance);
                    
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
                
                // while (placedTreesInForest < forestVertices.Count * forestData.forestDensity / forestData.minTreeDistance)
                // {
                //     PlaceTree(forestVertices, minHeight, areas, exploredPositions, 
                //         forestData.minTreeDistance, natureData.naturePrefabs, forestData.forestParent, prefabSize, natureData.prefabSizeMultiplier, ref placedTreesInForest);
                // }
            }
            else
            {
                Debug.Log("Forest Area Not found");
            }
        }
    }


    public static void PlaceTree(List<Vector3> vertices, float minHeight, List<Area> areas, List<Vector3> exploredPositions, 
       float minTreeDistance, List<NaturePrefab> naturePrefabs, GameObject natureParent, float prefabSize, float prefabSizeMultiplier, ref int placedTrees)
    {
        // Choisir un point aléatoire
        
        int randomIndex = Random.Range(0, vertices.Count);
        Vector3 newPosition = vertices[randomIndex];
            
        // Vérifier si il est valide
            
        bool valid = IsValidNaturePosition(newPosition, minHeight, areas, exploredPositions, minTreeDistance);
        
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
    
    public static bool IsValidNaturePosition(Vector3 position, float minHeight, List<Area> areas, List<Vector3> exploredPositions, float minTreeDistance)
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
