using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct VehiclePrefab
{
    
    public TransportModeType transportModeType;
    
    public List<GameObject> prefab;
    public List<AreaType> canSpawnOnAreas;
}

public class VehicleStat
{
    public TransportModeType transportModeType;
    internal float percentPerType;
    internal List<RepartitionPerArea> repartitionPerAreas = new List<RepartitionPerArea>();
    
    public void Display()
    {
        Debug.Log("===========================");
        Debug.Log("TransportModeType: " + transportModeType);
        Debug.Log("percentPerType: " + percentPerType);
        foreach (RepartitionPerArea repartitionPerArea in repartitionPerAreas)
        {
            Debug.Log("AreaType: " + repartitionPerArea.areaType+ ", percentInArea: " + repartitionPerArea.percentInArea);
        }
    }
}

[Serializable]
public struct AgentRepartition
{
    public int maxEntities;
    public float percentInAreas;
    public List<RepartitionPerArea> repartitionPerAreas;
}

[Serializable]
public struct RepartitionPerArea
{
    public AreaType areaType;
    public float percentInArea;

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
    public AgentRepartition agentRepartition;
    
    public List<VehiclePrefab> carPrefabs = new List<VehiclePrefab>();
    public List<VehicleStat> vehicleStats = new List<VehicleStat>();
    
    internal List<GameObject> cars = new List<GameObject>();

    internal GameObject roadParent;
    
    internal List<List<FindPath.PathPoint>> listRoads;
    
    
    
    
    public void InitAgentManager(StatManager statManager, ObjectManager objectManager)
    {
        this.statManager = statManager;
        this.objectManager = objectManager;
        
        // Init vehicleStats
        foreach (VehiclePrefab carPrefab in carPrefabs)
        {
            VehicleStat vehicleStat = new VehicleStat();
            vehicleStat.transportModeType = carPrefab.transportModeType;
            vehicleStats.Add(vehicleStat);
        }
        
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
}
