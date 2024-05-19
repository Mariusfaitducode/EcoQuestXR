using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum AreaType { City, Industry, Energy, Agriculture };

[System.Serializable]
public class Area
{
    public AreaData data;
    public float flatnessThreshold = 0.1f;
    internal Vector3 position;

    internal GameObject hierarchyRoadFolder;
    internal GameObject hierarchyBuildingFolder;
    

    public GameObject sphere;

    public GameObject roadParent;

    public NoiseData areaNoise;
    
    public Renderer noiseRenderer;
    
    // internal float uniformRadius;
    // internal float uniformStartRadius;
    
    internal float gridCellSize;
    
    internal AreaCell[,] areaGrid;
        
    public void SetPosition(Vector3 position)
    {
        this.position = position;
    }
    
    // Go To Area script ?
    public void CreateGrid(float cellSize)
    {

        // int tabSize = data.areaGridSize;
        int tabSize = (int)(data.radius * 2 / cellSize);
        
        // gridCellSize = ((float)data.radius * 2 / tabSize);
        gridCellSize = cellSize;
        
        AreaCell[,] gridPoints = new AreaCell[tabSize, tabSize];

        // int i = 0;
        
        Vector3 center = sphere.transform.position;
        
        for (int i = 0; i < tabSize; i++) {
            
            float x = center.x - data.radius + i * gridCellSize;
            
            
            for (int j = 0; j < tabSize; j++) {
                
                float z = center.z - data.radius + j * gridCellSize;
                
                AreaCell cell = new AreaCell();
                
                Vector3 pos = new Vector3(x, center.y, z);
                pos.y = FillMapUtils.GetHeightFromRaycast(pos);
                
                cell.position = pos;
                cell.size = gridCellSize;
                
                cell.gridPosition = new Vector2Int(i, j);
                
                if (Vector3.Distance(pos, center) <= data.radius) {
                    cell.inArea = true;
                }
                if (Vector3.Distance(pos, center) <= data.startRadius) {
                    cell.inStartArea = true;
                }
                
                gridPoints[i, j] = cell;
            }
        }
        areaGrid = gridPoints;
    }
}
