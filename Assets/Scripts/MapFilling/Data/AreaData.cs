using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu()]
[System.Serializable]
public class AreaData : UpdatableData
{
    public AreaType type;
    public int areaId;
    
    // internal Vector3 position;
    public int radius;
    
    [FormerlySerializedAs("startSize")] public int startRadius;

    // To change for gridSize
    public int areaGridSize;
    // public float gridCellSize;

    //City generation
    public int pavilionWidth = 2;
    public int pavilionHeightMean = 5;
    
    
        
    public List<AreaPrefab> prefabs;

    // internal List<Vector3> vertices;
}

[Serializable]
public struct AreaPrefab
{
    public GameObject prefabLow;
    public GameObject prefabHigh;
    
    public Vector2Int size;

    // public Vector3 position;
    // public Vector3 scale;
    // public Quaternion rotation;
}
