using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TransportModeType
{
    Car,
    Taxi,
    Bus,
    Bike,
    Walk
}


public static class TransportGestion
{
    
    
    public static void InitTransport(AgentManager agentManager, FillMapManager fillMapManager)
    {
        StatManager statManager = agentManager.statManager;
        ObjectManager objectManager = agentManager.objectManager;
        
        // Initialisation

        // Get stats from Citizen Stats
        CitizensGestion citizensGestion = statManager.citizensGestion;
        
        List<TransportMode> availableTransportModes = citizensGestion.transportModes.FindAll(tr => tr.isAvailable);
        int totalCitizens = citizensGestion.totalCitizens;
        List<VehiclePrefab> carPrefabs = agentManager.carPrefabs;
        AgentRepartition agentRepartition = agentManager.agentRepartition;
        List<VehicleStat> vehicleStats = agentManager.vehicleStats;
      
        
        // Quantity of daily users for available and rendered vehicles
        int dailyUsersForRenderedVehicle = 0;
        
        foreach (VehicleStat vehicleStat in vehicleStats)
        {
            TransportMode transportMode = availableTransportModes.Find(transportMode =>
                transportMode.transportModeType == vehicleStat.transportModeType);
            
            if (transportMode == null)
            {
                Debug.LogError("Could not find transport mode " + vehicleStat.transportModeType +
                               " in available transport modes.");
            }

            dailyUsersForRenderedVehicle += transportMode.dailyUsers;
        }
        Debug.Log("dailyUsersForRenderedVehicle: " + dailyUsersForRenderedVehicle + " / " + totalCitizens);
        
        
        // Set ratio for each transport mode from stat
        foreach (VehicleStat vehicleStat in vehicleStats)
        {
            TransportMode transportMode = availableTransportModes.Find(transportMode =>
                transportMode.transportModeType == vehicleStat.transportModeType);
            if (transportMode == null)
            {
                Debug.LogError("Could not find transport mode " + vehicleStat.transportModeType +
                               " in available transport modes.");
            }

            vehicleStat.percentPerType = (float)transportMode.dailyUsers / dailyUsersForRenderedVehicle;
        }

        // Loop over all values in the TransportModeType enum
        List<RepartitionPerArea> repartitionPerAreas = agentRepartition.repartitionPerAreas;
        foreach (RepartitionPerArea repartitionPerArea in repartitionPerAreas)
        {
            // Get objects and the total percent for the area
            float totalPercent = 0;
            foreach (VehiclePrefab carPrefab in carPrefabs)
            {
                if (carPrefab.canSpawnOnAreas.Contains(repartitionPerArea.areaType))
                {
                    totalPercent += vehicleStats.Find(vehicleStat => vehicleStat.transportModeType == carPrefab.transportModeType).percentPerType;
                }
            }

            float coef = 1 / totalPercent;
            
            // Set the percent for each car prefab
            foreach (VehicleStat vehicleStat2 in vehicleStats)
            {
                // if it can spawn on the area
                if (carPrefabs.Find(carPrefab => carPrefab.transportModeType == vehicleStat2.transportModeType).canSpawnOnAreas.Contains(repartitionPerArea.areaType))
                {
                    float percentInAreaDepedantOnAvailableVehicle = coef * vehicleStat2.percentPerType;
                    float ratio = agentRepartition.percentInAreas * repartitionPerArea.percentInArea *
                                 percentInAreaDepedantOnAvailableVehicle;
                    RepartitionPerArea repartitionPerArea2 = new RepartitionPerArea();
                    repartitionPerArea2.areaType = repartitionPerArea.areaType;
                    repartitionPerArea2.percentInArea = ratio;
                    vehicleStat2.repartitionPerAreas.Add(repartitionPerArea2);
                }
            }
        }

        // Generation per areas
        
        
        
        foreach (VehicleStat vehicleStat in vehicleStats)
        {
            // vehicleStat.Display();

            foreach (RepartitionPerArea repartitionPerArea in vehicleStat.repartitionPerAreas)
            {
                Area area = agentManager.areas.Find(area => area.data.type == repartitionPerArea.areaType);
                
                int maxEntities = agentRepartition.maxEntities;
                int generatedEntities = (int)(maxEntities * repartitionPerArea.percentInArea);
                
                for (int i = 0; i < generatedEntities; i++)
                {

                    VehiclePrefab listVehicle= agentManager.carPrefabs.Find(car => car.transportModeType == vehicleStat.transportModeType);
            
                    GameObject vehicle = listVehicle.prefab[Random.Range(0, listVehicle.prefab.Count)];
                
                
                    if (!vehicle.GetComponent<DriveInArea>())
                    {
                        vehicle.AddComponent<DriveInArea>();
                    }
            
                    if (vehicle.GetComponent<DriveOnRoad>())
                    {
                        vehicle.GetComponent<DriveOnRoad>().enabled = false;
                    }
                
                    DriveInArea carDrive = vehicle.GetComponent<DriveInArea>();

                    if (vehicleStat.transportModeType == TransportModeType.Bike)
                    {
                        carDrive.driveSettings.speedFactor = 0.3f;
                        carDrive.driveSettings.roadStepFactor = 1.8f;
                    }
                    else
                    {
                        carDrive.driveSettings.speedFactor = 1f;
                        carDrive.driveSettings.roadStepFactor = 1f;
                    }

                    carDrive.enabled = true;
                    carDrive.areaType = area.data.type;
            
                    // TODO : Set vehicle drive settings by vehicle type

                    // Bike settings
                    
            
                    Transform parent = area.hierarchyBuildingFolder.transform;
                    Vector3 position = area.areaGrid[Random.Range(0, area.areaGrid.GetLength(0)), 
                        Random.Range(0, area.areaGrid.GetLength(1))].cellPosition.transform.position;
            
                    float uniformScale = objectManager.mesh.transform.localScale.x;
            
                    GameObject newCar = FillMapUtils.InstantiateObjectWithScale(vehicle, parent, position, Quaternion.identity, Vector3.one * uniformScale * objectManager.prefabScale);
            
                
                
                    agentManager.cars.Add(newCar);
                } 
            }
        }
        
        // Generation in big road
        
        fillMapManager.roadCarsFolder = new GameObject("RoadsCars");
        fillMapManager.roadCarsFolder.transform.parent = agentManager.roadParent.transform;
        
        int generatedCars = (int)(agentRepartition.maxEntities * (1 - agentRepartition.percentInAreas));
        
        for (int i = 0; i < generatedCars; i++)
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
            
            Transform parent = fillMapManager.roadCarsFolder.transform;
            
            float uniformScale = objectManager.mesh.transform.localScale.x;
            
            GameObject newCar = FillMapUtils.InstantiateObjectWithScale(car, parent, Vector3.zero, Quaternion.identity, Vector3.one * uniformScale * objectManager.prefabScale);
            
            agentManager.cars.Add(newCar);
        }
    }
    
    
    
}
