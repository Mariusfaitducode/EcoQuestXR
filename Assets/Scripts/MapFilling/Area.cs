using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AreaType { City, Industry, Energy, Agriculture };

[System.Serializable]
public class Area
{
    
    public AreaData data;
    
    
    public float flatnessThreshold = 0.1f;
    
    
    internal Vector3 position;
    
    
    
    // public bool valid;

        
    
    public GameObject sphere;
    
    public GameObject testCube;

    internal float uniformRadius;
    internal float uniformStartRadius;
    
    internal float gridCellSize;
    
    internal AreaCell[,] areaGrid;
        
    public void SetPosition(Vector3 position)
    {
        this.position = position;
    }
    
    // Go To Area script ?
    public void CreateGrid()
    {

        int tabSize = data.areaGridSize;
        gridCellSize = (uniformRadius * 2 / tabSize);
        AreaCell[,] gridPoints = new AreaCell[tabSize, tabSize];

        // int i = 0;
        
        Vector3 center = sphere.transform.position;
        
        for (int i = 0; i < tabSize; i++) {
            
            float x = center.x - uniformRadius + i * gridCellSize;
            
            
            for (int j = 0; j < tabSize; j++) {
                
                float z = center.z - uniformRadius + j * gridCellSize;
                
                AreaCell cell = new AreaCell();
                
                Vector3 pos = new Vector3(x, center.y, z);
                pos.y = FillMapUtils.GetHeightFromRaycast(pos);
                
                cell.position = pos;
                
                if (Vector3.Distance(pos, center) <= uniformRadius) {
                    cell.inArea = true;
                }
                
                gridPoints[i, j] = cell;
            }
        }
        areaGrid = gridPoints;
    }
}
