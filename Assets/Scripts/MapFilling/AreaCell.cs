using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType { Empty, Road, Object, Area };

public class AreaCell
{
    public Vector3 position;
    
    public CellType type;
    

    public bool inArea;
    public bool hasObject;
}
