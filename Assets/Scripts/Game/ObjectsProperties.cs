using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProperties
{
    public int id { get; set; }
    public string name { get; set; }
    public string prefabName { get; set; }
    
    public AreaType areaType { get; set; }
    
    public int sizeX { get; set; }
    public int sizeY { get; set; }
    
    public int weight { get; set; }
    
    public int price { get; set; }
    public int ecology { get; set; }
    public int population { get; set; }
    public int energy { get; set; }
    public int pollution { get; set; }
}
