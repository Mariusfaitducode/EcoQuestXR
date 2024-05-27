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

    // To choose between gridSize or gridCellSize
    // public int areaGridSize;
    // public float gridCellSize;

    //City generation
    public int pavilionWidth = 2;
    public int pavilionHeightMean = 5;
    
    [Range(0,1)]
    public float fillPercent;
    
    public List<AreaPrefab> aloneNecessaryPrefabs;
    [FormerlySerializedAs("prefabs")] public List<AreaPrefab> randomPrefabs;

    // internal List<Vector3> vertices;
}

[Serializable]
public struct AreaPrefab
{
    public GameObject prefabLow;
    // public GameObject prefabHigh;
    
    public Vector2Int size;

    public float weight;

    [Range(0,1)]
    public float rotation;

    // public bool oneToPlace;

    // public Vector3 position;
    // public Vector3 scale;
    // public Quaternion rotation;
}
