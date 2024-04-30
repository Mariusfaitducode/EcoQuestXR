using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class AreaData : UpdatableData
{
    public AreaType type;
    // internal Vector3 position;
    public int radius;
    
    public int startSize;

    // To change for gridSize
    public int areaGridSize;
    // public float gridCellSize;

    //City generation
    public int pavilionWidth = 2;
    public int pavilionHeightMean = 5;
    
    
        
    public GameObject prefabs;

    // internal List<Vector3> vertices;
    
}
