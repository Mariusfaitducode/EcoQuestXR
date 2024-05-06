using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public static class RoadGenerator
{
    [Serializable]
    public class RoadData
    {
        public float edgeLength = 10;
        public float angle = 45;
        public float roadScale = 3f;
        public float mountainGap = 0.2f;
        public float roadMinHeight = 0.1f;
        public float targetDistance = 20f;
        
        public Material roadMaterial = null;
        public Material testMaterial = null;
    }
    
    // private Random random = new Random();


    public static Vector3[] FindRoadExtremity(MeshData meshData, MapGenerator mapGenerator, GameObject meshTerrain, GameObject testCube, GameObject roadParent, float uniformScale, RoadData roadData)
    {
        bool valid = false;

        Vector3 randVertex = meshData.borderVertices[Random.Range(0, mapGenerator.meshData.borderVertices.Length)];
        Vector3 randVertex2 = Vector3.zero;
        
        Vector3[] listPoints = new Vector3[2];
        listPoints[0] = randVertex * uniformScale;

        int i = 0;

        while (!valid && i < 500)
        {
            randVertex2 = mapGenerator.meshData.borderVertices[Random.Range(0, mapGenerator.meshData.borderVertices.Length)];
            
            float distance = Vector3.Distance(randVertex, randVertex2);

            // Renderer meshRenderer = meshTerrain.GetComponent<MeshFilter>();
            
            Vector3 size = meshTerrain.GetComponent<MeshFilter>().sharedMesh.bounds.size * uniformScale;
            
            Debug.Log(size);
            
            

            if (distance > size.x)
            {
                FillMapUtils.InstantiateObjectWithScale(testCube, roadParent.transform, randVertex * uniformScale, Vector3.one * uniformScale * roadData.roadScale);
                GameObject cube = FillMapUtils.InstantiateObjectWithScale(testCube, roadParent.transform, randVertex2 * uniformScale, Vector3.one * uniformScale * roadData.roadScale);
                
                cube.GetComponent<Renderer>().material = roadData.testMaterial;
                
                listPoints[1] = randVertex2 * uniformScale;
                valid = true;
            }
            i++;
        }
        
        

        // Debug.Log(i);
        return listPoints;
    }


    public static Vector3[] ExtremityOnTerrain(Vector3[] listPoints, List<Area> areas, RoadData roadData, GameObject testCube, GameObject roadParent, float uniformScale)
    {
        List<FindPath.PathPoint> startList = new List<FindPath.PathPoint>();
        List<FindPath.PathPoint> endList = new List<FindPath.PathPoint>();
        
        startList.Add(new FindPath.PathPoint(listPoints[0], 0, null));
        endList.Add(new FindPath.PathPoint(listPoints[1], 0, null));

        for (int i = 0; i < 3; i++)
        {
            List<FindPath.PathPoint> copyStartList = new List<FindPath.PathPoint>(startList);
            foreach (FindPath.PathPoint point in copyStartList)
            {
                FindPath.FindNeighbours(startList, point,  areas, roadData, copyStartList);

            }

            List<FindPath.PathPoint> copyEndList = new List<FindPath.PathPoint>(endList);

            foreach (FindPath.PathPoint point in copyEndList)
            {
                FindPath.FindNeighbours(endList, point,  areas, roadData, copyEndList);
            }
        }
        Debug.Log(startList.Count);
        
        foreach (FindPath.PathPoint point in startList)
        {
            if (point.position.y > roadData.roadMinHeight)
            {
                listPoints[0] = point.position;
                break;
            }
        }
        foreach (FindPath.PathPoint point in endList)
        {
            if (point.position.y > roadData.roadMinHeight)
            {
                listPoints[1] = point.position;
                break;
            }
        }
        
        FillMapUtils.InstantiateObjectWithScale(testCube, roadParent.transform, listPoints[0] * uniformScale, Vector3.one * uniformScale * roadData.roadScale);
        GameObject cube = FillMapUtils.InstantiateObjectWithScale(testCube, roadParent.transform, listPoints[1] * uniformScale, Vector3.one * uniformScale * roadData.roadScale);
                
        cube.GetComponent<Renderer>().material = roadData.testMaterial;

        return listPoints;
    }
    
    
    public static void GenerateRoadArea(Area area)
    {
        int width = area.areaGrid.GetLength(0);
        int height = area.areaGrid.GetLength(1);
        
        
        // int[,] grid = new int[width, height];
        // Array.Copy(grid, newGrid, grid.Length);


        // Initialise avec des routes horizontales
        for (int i = 0; i < width; i += area.data.pavilionWidth + 1)
        {
            for (int j = 0; j < height; j++)
            {
                area.areaGrid[i, j].type = CellType.Road;
            }
        }
        
        // Génère des intersections verticales aléatoires
        for (int j = 1; j < height; j += area.data.pavilionWidth + 1)
        {

            int nextRoadCount = 0;

            int randomPavilionLength = GenerateGaussian((double)area.data.pavilionHeightMean);
            
            Debug.Log("Gaussian value : " + randomPavilionLength);

            nextRoadCount += randomPavilionLength;

            while (nextRoadCount < width)
            {
                for (int roadCount = 0; roadCount < area.data.pavilionWidth; roadCount++)
                {
                    if (j + roadCount < height && nextRoadCount < width)
                    {
                        area.areaGrid[ j + roadCount, nextRoadCount].type = CellType.Road;
                        
                    }
                }
                nextRoadCount += GenerateGaussian((double)area.data.pavilionHeightMean);
                
            }
        }
    }

    
    
    
    
    
    public static int GenerateGaussian(double mean = 5, double stdDev = 1)
    {
        // Random random = new Random();

        double u1 = 1.0 - Random.value; // uniform(0,1] random doubles
        double u2 = 1.0 - Random.value;
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                               Math.Sin(2.0 * Math.PI * u2); // random normal(0,1)
        double gaussianValue = mean + stdDev * randStdNormal; // random normal(mean,stdDev^2)
        
        return (int)Mathf.Round((float)gaussianValue);
    }
    
    public static void TestGaussian()
    {
        for (int i = 0; i < 1000; i++)
        {
            double gaussianValue = GenerateGaussian(5.0, 1.0); // Moyenne = 5, Écart-type = 1
            Debug.Log(gaussianValue);
        }
    }


    

    

    
    
}
