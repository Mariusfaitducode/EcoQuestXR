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



    

    

    
    
}
