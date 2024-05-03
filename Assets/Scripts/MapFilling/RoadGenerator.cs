using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public static class RoadGenerator
{
    
    // private Random random = new Random();
    
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
    
    public static int[,] GenerateRoadArea(Area area)
    {
        int width = area.areaGrid.GetLength(0);
        int height = area.areaGrid.GetLength(1);
        
        
        int[,] newGrid = new int[width, height];
        int[,] grid = new int[width, height];
        Array.Copy(grid, newGrid, grid.Length);


        // Initialise avec des routes horizontales
        for (int i = 0; i < width; i += area.data.pavilionWidth + 1)
        {
            for (int j = 0; j < height; j++)
            {
                grid[i, j] = 1;
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
                        grid[ j + roadCount, nextRoadCount] = 1;
                        
                    }
                }
                nextRoadCount += GenerateGaussian((double)area.data.pavilionHeightMean);
                
            }
        }
        
        return  grid;
    }



    public static List<Vector3> FindNeighbours(Vector3 position, float angle, float radius, float scale)
    {
        List<Vector3> neighbours = new List<Vector3>();

        float angleRad = 0;
        
        while (angleRad < 2 * Mathf.PI)
        {
            float x = position.x + radius * Mathf.Cos(angleRad);
            float z = position.z + radius * Mathf.Sin(angleRad);
            
            float y = FillMapUtils.GetHeightFromRaycast(new Vector3(x, position.y, z));
            
            neighbours.Add(new Vector3(x, y, z));
            
            angleRad += Mathf.Deg2Rad * angle;
        }
        
        
        return neighbours;
    }

    public struct  PathPoint
    {
        public Vector3 position;
        // float weight;
        public int count;
        
        public PathPoint(Vector3 position,  int count)
        {
            this.position = position;
            // this.weight = weight;
            this.count = count;
        }
        
        public float GetWeight(Vector3 end)
        {
            return Vector3.Distance(position, end) + count;
        }
    }

    public static void FindPathWithAStar(Vector3 start, Vector3 end, float angle, float radius, GameObject testCube,  GameObject roadParent, float scale, float roadScale)
    {
        int count = 0;
        
        List<PathPoint> exploredVertices = new List<PathPoint>();

        // Vector3 newPosition = start;
        
        PathPoint newPoint = new PathPoint(start, 0);
        exploredVertices.Add(newPoint);
        
        int i = 0;
        
        while (!Arrive(newPoint.position, end, 20 * scale) && i < 200 )
        {
            List<Vector3> neighbours = FindNeighbours(newPoint.position, angle, radius, scale);
            
            PathPoint nextPoint = ChooseNextPosition(neighbours, end, newPoint);
            
            exploredVertices.Add(nextPoint);
            newPoint = nextPoint;
            
            FillMapUtils.InstantiateObjectWithScale(testCube, roadParent.transform, newPoint.position, Vector3.one * scale * roadScale);
            
            // GameObject cube = GameObject.Instantiate(testCube, newPoint.position * scale, Quaternion.identity);
            // cube.transform.parent = roadParent.transform;
            i++;
        }
    }
    
    public static PathPoint ChooseNextPosition(List<Vector3> neighbours, Vector3 end, PathPoint lastPoint)
    {
        PathPoint nextPoint = new PathPoint();
        
        Vector3 nextPosition = Vector3.zero;
        
        float minWeight = float.MaxValue;
        
        foreach (Vector3 neighbour in neighbours)
        {
            PathPoint point = new PathPoint(neighbour, lastPoint.count + 1);
            
            float weight = point.GetWeight(end);
            
            if (weight < minWeight)
            {
                minWeight = weight;
                nextPoint = point;
                nextPosition = neighbour;
            }
        }
        neighbours.Remove(nextPosition);
        
        return nextPoint;
    }
    
    public static bool Arrive(Vector3 position, Vector3 end, float radius)
    {
        return Vector3.Distance(position, end) < radius;
    }
    
}
