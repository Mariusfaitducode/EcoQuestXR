using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType { Empty, Road, Object };

public class AreaCell
{
    public Vector3 position;

    public float size;
    
    public CellType type;
    
    public GameObject objectPrefab;

    public bool inArea;
    public bool hasObject;
}
