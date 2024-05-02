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
    
    public static int[,] GenerateRoadContent(Area area)
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
        
        // Initialise avec une route centrale
        //for (int i = 0; i < width; i++) {
        //    area.areaGrid[i, height / 2].type = CellType.Road;
        //    grid[i, height / 2] = 1;
        //}
        //for (int i = 0; i < height; i++) {
        //    area.areaGrid[width / 2, i].type = CellType.Road;
        //    grid[width / 2, i] = 1;
        //}

        //for (int i = 0; i < 1; i++)
        //{
        //    for (int x = 1; x < width - 1; x++) {
        //        for (int y = 1; y < height - 1; y++) {
        //            int neighbors = CountRoadNeighbors(grid, x, y);
                
        //            Debug.Log("Neighbors : " + neighbors);

        //            newGrid[x, y] = 0;
                    
        //            // Règle d'expansion et de branchement
        //            // Règle d'expansion avec probabilité ajustée
        //            // Ajout de probabilité et condition supplémentaire pour devenir route
        //            if (grid[x, y] == 0 && neighbors >= 3 && Random.value < 0.3) {
        //                newGrid[x, y] = 1;
        //            } else if (grid[x, y] == 1 && (neighbors < 2 || neighbors > 4)) {
        //                newGrid[x, y] = 0; // Routes trop isolées ou trop encombrées disparaissent
        //            }
        //        }
        //    }
        //    grid = newGrid;
        //}
        
        return  grid;

        
        
    }
    
    static int CountRoadNeighbors(int[,] grid, int x, int y) {
        int count = 0;
        for (int dx = -1; dx <= 1; dx++) {
            for (int dy = -1; dy <= 1; dy++) {
                if ((dx != 0 || dy != 0) && grid[x + dx, y + dy] == 1) {
                    count++;
                }
            }
        }
        return count;
    }
}
