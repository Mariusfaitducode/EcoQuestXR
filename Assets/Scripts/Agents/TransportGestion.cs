using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TransportModeType
{
    Car,
    Taxi,
    Bus,
    Bike,
}


public static class TransportGestion
{
    
    
    public static void InitTransport(AgentManager agentManager)
    {
        StatManager statManager = agentManager.statManager;
        ObjectManager objectManager = agentManager.objectManager;
        
        // ? First initialisation

        // Citizens stats
        
        CitizensStats citizensStats = statManager.citizensGestion.citizensStats;
        
        // Debug.Log("Stat Manager Transport Generation");
        //
        // foreach (TransportMode transportMode in statManager.citizensGestion.transportModes)
        // {
        //     transportMode.Display();
        // }
        //
        // foreach (TransportMode transportMode in statManager.citizensGestion.availableTransportModes)
        // {
        //     transportMode.Display();
        // }
        //
        
        
        // TODO : good stats
        
        
        // Percent car
        
        
        
        
        
        
        
        
        
        
        int quantityPop = objectManager.GetMaxPopSize();

        // Transport

        float carPercent = (citizensStats.sensibilisation + citizensStats.health) / 2;
        
        // Debug.Log("CAR PERCENT : "+carPercent);
        
        
        int carQuantity = (int) (quantityPop * carPercent);
        
        
        GameObject roadsCarsFolder = new GameObject("RoadsCars");
        roadsCarsFolder.transform.parent = agentManager.roadParent.transform;
        
        // Drive on road
        
        for (int i = 0; i < carQuantity; i++)
        {

            VehiclePrefab listCars= agentManager.carPrefabs.Find(car => car.transportModeType == TransportModeType.Car);
            
            GameObject car = listCars.prefab[Random.Range(0, listCars.prefab.Count)];

            if (!car.GetComponent<DriveOnRoad>())
            {
                car.AddComponent<DriveOnRoad>();
            }
            
            if (car.GetComponent<DriveInArea>())
            {
                car.GetComponent<DriveInArea>().enabled = false;
            }
            
            DriveOnRoad carDrive = car.GetComponent<DriveOnRoad>();
            
            carDrive.enabled = true;
            carDrive.listRoads = agentManager.listRoads;

            // Set car settings
            carDrive.driveSettings.speedFactor = 1;
            carDrive.driveSettings.roadStepFactor = 1;
            
            Transform parent = roadsCarsFolder.transform;
            
            float uniformScale = objectManager.mesh.transform.localScale.x;
            
            GameObject newCar = FillMapUtils.InstantiateObjectWithScale(car, parent, Vector3.zero, Quaternion.identity, Vector3.one * uniformScale * objectManager.prefabScale);
            
            agentManager.cars.Add(newCar);
        }
        
        // Drive In Area
        foreach (Area area in agentManager.areas)
        {
            

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
                // GameObject car = agentManager.carPrefabs[Random.Range(0, agentManager.carPrefabs.Count)];

                VehiclePrefab listCars= agentManager.carPrefabs.Find(car => car.transportModeType == TransportModeType.Bike);
            
                GameObject car = listCars.prefab[Random.Range(0, listCars.prefab.Count)];
                
                
                if (!car.GetComponent<DriveInArea>())
                {
                    car.AddComponent<DriveInArea>();
                }
            
                if (car.GetComponent<DriveOnRoad>())
                {
                    car.GetComponent<DriveOnRoad>().enabled = false;
                }
                
                
                DriveInArea carDrive = car.GetComponent<DriveInArea>();

                carDrive.enabled = true;
                carDrive.areaType = area.data.type;
            
                // TODO : Set vehicle drive settings by vehicle type

                // Bike settings
                carDrive.driveSettings.speedFactor = 0.3f;
                carDrive.driveSettings.roadStepFactor = 1.8f;
            
                Transform parent = area.hierarchyBuildingFolder.transform;
                Vector3 position = area.areaGrid[Random.Range(0, area.areaGrid.GetLength(0)), 
                    Random.Range(0, area.areaGrid.GetLength(1))].cellPosition.transform.position;
            
                float uniformScale = objectManager.mesh.transform.localScale.x;
            
                GameObject newCar = FillMapUtils.InstantiateObjectWithScale(car, parent, position, Quaternion.identity, Vector3.one * uniformScale * objectManager.prefabScale);
            
                
                
                agentManager.cars.Add(newCar);
            }
        }
        
        


    }
}
