using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProperties
{
    public int id { get; set; }
    public string name { get; set; }
    public string prefabName { get; set; }
    
    public string prefabPath { get; set; }
    
    public AreaType areaType { get; set; }
    
    public int sizeX { get; set; }
    public int sizeY { get; set; }
    
    public int weight { get; set; }

    public Stat stats;
    
    public List<int> subObjectsIds = new List<int>();
    public List<SubObjects> subObjects = new List<SubObjects>();
}
