using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AreaType { City, Industry, Energy, Agriculture };

[System.Serializable]
public class Area
{
    
    public AreaData data;
    
    // public AreaType type;
    internal Vector3 position;
    // public int size;
    public GameObject sphere;
    // public Material material;
    public bool valid;

        
    // public GameObject prefabs;

    internal List<Vector3> vertices;
        
    public void SetPosition(Vector3 position)
    {
        this.position = position;
    }
}
