using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class AreaData : UpdatableData
{
    public AreaType type;
    // internal Vector3 position;
    public int size;
    
    public int startSize;

    // To change for gridSize
    public float gridCellSize;

    //City generation
    public int pavilionWidth = 2;
    public int pavillonHeightMean = 5;
    
    
        
    public GameObject prefabs;

    // internal List<Vector3> vertices;
    
}
