using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    internal List<Area> areas;
    internal float mapScale;
    
    internal Timer timer;

    // Stat Manager
    internal StatManager statManager;
    internal ObjectManager objectManager;
    
    // Transport objects
    
    public List<GameObject> carPrefabs = new List<GameObject>();
    internal List<GameObject> cars = new List<GameObject>();
    
    
    
    
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


        // Road Informations
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
