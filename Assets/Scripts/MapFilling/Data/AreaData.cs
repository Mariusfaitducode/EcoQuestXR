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

    public float gridCellSize;

        
    public GameObject prefabs;

    // internal List<Vector3> vertices;
    
}
