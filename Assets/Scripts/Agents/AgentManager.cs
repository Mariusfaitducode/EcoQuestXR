using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct VehiclePrefab
{
    
    public TransportModeType transportModeType;
    
    public List<GameObject> prefab;
    
}

public class AgentManager : MonoBehaviour
{
    internal List<Area> areas;
    internal float mapScale;
    
    internal Timer timer;

    // Stat Manager
    internal StatManager statManager;
    internal ObjectManager objectManager;
    
    // Transport objects
    
    public List<VehiclePrefab> carPrefabs = new List<VehiclePrefab>();
    internal List<GameObject> cars = new List<GameObject>();

    internal GameObject roadParent;
    
    internal List<List<FindPath.PathPoint>> listRoads;
    
    
    
    
    public void InitAgentManager(StatManager statManager, ObjectManager objectManager)
    {
        this.statManager = statManager;
        this.objectManager = objectManager;
        
        // transportGestion = new TransportGestion();
        TransportGestion.InitTransport(this);
    }
    

    public void SetTimerInformations(Timer timer)
    {
        this.timer = timer;
    }
    
    public void SetMapInformations(FillMapManager fillMapManager)
    {
        areas = fillMapManager.areas;
        mapScale = fillMapManager.mapGenerator.terrainData.uniformScale;
        
        listRoads = fillMapManager.listRoads;
        
        roadParent = fillMapManager.roadRootParentFolder;


        // Road Informations
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
