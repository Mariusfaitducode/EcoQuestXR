using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    internal List<Area> areas;
    internal float mapScale;
    
    
    public void SetMapInformations(FillMapManager fillMapManager)
    {
        areas = fillMapManager.areas;
        mapScale = fillMapManager.mapGenerator.terrainData.uniformScale;

        
        // Road Informations
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
