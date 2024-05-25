using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType { Empty, Road, Object };

public class AreaCell
{
    public GameObject cellPosition;
    
    public Vector2Int gridPosition;

    public float size;
    
    public CellType type;
    
    public GameObject objectPrefab;

    public bool inArea;
    public bool inStartArea;

    public bool hasObject;
}
