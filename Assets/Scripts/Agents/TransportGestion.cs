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
    
    
    public static void InitTransport(AgentManager agentManager)
    {
        StatManager statManager = agentManager.statManager;
        ObjectManager objectManager = agentManager.objectManager;
        
        // Initialisation

        // Get stats from Citizen Stats
        CitizensGestion citizensGestion = statManager.citizensGestion;
        
        List<TransportMode> availableTransportModes = citizensGestion.availableTransportModes;
        int totalCitizens = citizensGestion.totalCitizens;
        List<VehiclePrefab> carPrefabs = agentManager.carPrefabs;
        AgentRepartition agentRepartition = agentManager.agentRepartition;
        List<VehicleStat> vehicleStats = agentManager.vehicleStats;
        
        // Get percent of rendered entities
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

        foreach (VehicleStat vehicleStat in vehicleStats)
        {
            vehicleStat.Display();
        }
        
        
        
        
        
        
        
    


        int carQuantity = 0;
        
        
        GameObject roadsCarsFolder = new GameObject("RoadsCars");
        roadsCarsFolder.transform.parent = agentManager.roadParent.transform;
        
        // Drive on road
        
        for (int i = 0; i < carQuantity; i++)
        {



            VehiclePrefab listCars= agentManager.carPrefabs.Find(car => car.transportModeType == TransportModeType.Car);
            
            GameObject car = listCars.prefab[Random.Range(0, listCars.prefab.Count)];

            if (car.GetComponent<DriveOnRoad>())
            {
                car.GetComponent<DriveInArea>().enabled = false;
                car.GetComponent<DriveOnRoad>().enabled = true;
            
                // car.GetComponent<DriveOnRoad>().listRoads = agentManager.listRoads;
                
            
                Transform parent = roadsCarsFolder.transform;
                // Vector3 position = area.areaGrid[Random.Range(0, area.areaGrid.GetLength(0)), 
                //     Random.Range(0, area.areaGrid.GetLength(1))].cellPosition.transform.position;
            
                float uniformScale = objectManager.mesh.transform.localScale.x;
            
                GameObject newCar = FillMapUtils.InstantiateObjectWithScale(car, parent, Vector3.zero, Quaternion.identity, Vector3.one * uniformScale * objectManager.prefabScale);
            
                
                
                agentManager.cars.Add(newCar);
            }
            
            
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

                VehiclePrefab listCars= agentManager.carPrefabs.Find(car => car.transportModeType == TransportModeType.Car);
            
                GameObject car = listCars.prefab[Random.Range(0, listCars.prefab.Count)];
                
                if (car.GetComponent<DriveOnRoad>())
                {
                    car.GetComponent<DriveOnRoad>().enabled = false;
                }

                car.GetComponent<DriveInArea>().enabled = true;

                car.GetComponent<DriveInArea>().areaType = area.data.type;
            
                
            
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
