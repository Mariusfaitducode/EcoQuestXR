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
        
        // ? First initialisation

        // Citizens stats
        
        CitizensStats citizensStats = statManager.citizensGestion.citizensStats;
        
        Debug.Log("Stat Manager Transport Generation");
        
        foreach (TransportMode transportMode in statManager.citizensGestion.transportModes)
        {
            transportMode.Display();
        }

        foreach (TransportMode transportMode in statManager.citizensGestion.availableTransportModes)
        {
            transportMode.Display();
        }

        int quantityPop = objectManager.GetMaxPopSize();

        // Transport

        float carPercent = (citizensStats.sensibilisation + citizensStats.health) / 2;
        
        Debug.Log("CAR PERCENT : "+carPercent);
        
        
        
        foreach (Area area in areas)
        {
            int carQuantity = (int) (quantityPop * carPercent);

            switch (area.data.type)
            {
                case AreaType.City:
                    carQuantity = (int) (carQuantity * 0.55f);
                    break;
                case AreaType.Energy:
                    carQuantity = (int) (carQuantity * 0.1f);
                    break;
                case AreaType.Agriculture:
                    carQuantity = (int) (carQuantity * 0.1f);
                    break;
                case AreaType.Industry:
                    carQuantity = (int) (carQuantity * 0.2f);
                    break;
                
            }
            
            for (int i = 0; i < carQuantity; i++)
            {
                GameObject car = carPrefabs[Random.Range(0, carPrefabs.Count)];
                
                car.GetComponent<DriveInArea>().areaType = area.data.type;
            
                
            
                Transform parent = area.hierarchyBuildingFolder.transform;
                Vector3 position = area.areaGrid[Random.Range(0, area.areaGrid.GetLength(0)), 
                    Random.Range(0, area.areaGrid.GetLength(1))].cellPosition.transform.position;
            
                float uniformScale = objectManager.meshTerrain.transform.localScale.x;
            
                GameObject newCar = FillMapUtils.InstantiateObjectWithScale(car, parent, position, Quaternion.identity, Vector3.one * uniformScale * objectManager.prefabScale);
            
                
                
                cars.Add(newCar);
            }
        }
        
        


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
