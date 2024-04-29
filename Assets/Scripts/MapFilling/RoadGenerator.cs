using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class RoadGenerator
{
    
    public static int[,] GenerateRoadContent(Area area)
    {
        int width = area.areaGrid.GetLength(0);
        int height = area.areaGrid.GetLength(1);
        
        
        int[,] newGrid = new int[width, height];
        int[,] grid = new int[width, height];
        Array.Copy(grid, newGrid, grid.Length);
        
        
        
        // Initialise avec une route centrale
        for (int i = 0; i < width; i++) {
            area.areaGrid[i, height / 2].type = CellType.Road;
            grid[i, height / 2] = 1;
        }
        for (int i = 0; i < height; i++) {
            area.areaGrid[width / 2, i].type = CellType.Road;
            grid[width / 2, i] = 1;
        }

        for (int i = 0; i < 1; i++)
        {
            for (int x = 1; x < width - 1; x++) {
                for (int y = 1; y < height - 1; y++) {
                    int neighbors = CountRoadNeighbors(grid, x, y);
                
                    Debug.Log("Neighbors : " + neighbors);

                    newGrid[x, y] = 0;
                    
                    // Règle d'expansion et de branchement
                    // Règle d'expansion avec probabilité ajustée
                    // Ajout de probabilité et condition supplémentaire pour devenir route
                    if (grid[x, y] == 0 && neighbors >= 3 && Random.value < 0.3) {
                        newGrid[x, y] = 1;
                    } else if (grid[x, y] == 1 && (neighbors < 2 || neighbors > 4)) {
                        newGrid[x, y] = 0; // Routes trop isolées ou trop encombrées disparaissent
                    }
                }
            }
            grid = newGrid;
        }
        
        return  newGrid;

        
        
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
