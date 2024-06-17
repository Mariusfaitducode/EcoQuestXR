using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BusGestion : MonoBehaviour
{
    
    public int busQuantity = 0;

    public int busStartHour = 6;
    public int busEndHour = 22;
    
    public List<GameObject> busPrefabs = new List<GameObject>();
    internal List<GameObject> buses = new List<GameObject>();
    
    
    internal float prefabScale = 1f;
    internal float uniformScale = 1f;
    
    // public float floorHeight = 0.05f;
    
    public bool busOut = false;
    
    
    internal GameManager gameManager;
    internal DateTime currentTime;

    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
        if (gameManager != null)
        {
            gameManager.statManager.citizensGestion.transportModes.Find(tr => tr.transportModeType == TransportModeType.Bus).isAvailable = true;
        }
        
    }

    void Update()
    {
        // Instantiate / Destroy buses with GameManager informations
        
        // Debug.Log("CurrentTime :"+currentTime);
        
        if (gameManager != null)
        {
            currentTime = gameManager.timer.currentTime;
            prefabScale = gameManager.objectManager.prefabScale;
            uniformScale = gameManager.objectManager.mesh.transform.localScale.x;
            
            
        }
        if ((currentTime.Hour >= busStartHour && currentTime.Hour < busEndHour) && !busOut)
        {
            busOut = true;
            
            // Debug.Log("BusGestion Instantiate buses");
            
            // Instantiate buses
            for (int i = 0; i< busQuantity; i++)
            {
                // Instantiate bus
                
                GameObject bus = busPrefabs[Random.Range(0, busPrefabs.Count)];
                
                // Debug.Log("INSTANTIATE BUSES ");
                // Debug.Log("PrefabScale : " + prefabScale);
                // Debug.Log("UniformScale : " + uniformScale);
                
                Vector3 busPlace = this.transform.position;
                
                // busPlace.y = busPlace.y + floorHeight * uniformScale;
                
                GameObject newBus = FillMapUtils.InstantiateObjectWithScale(bus, this.transform, busPlace, Quaternion.identity, Vector3.one * uniformScale);
                
                DriveInArea carDrive = newBus.GetComponent<DriveInArea>();
                
                carDrive.enabled = true;
                carDrive.areaType = AreaType.City;
            
                // TODO : Set vehicle drive settings by vehicle type

                // Bike settings
                carDrive.driveSettings.speedFactor = 1f;
                carDrive.driveSettings.roadStepFactor = 1f;
                
                // Init bus script
                buses.Add(newBus);
            }
        }
        // else if ((currentTime.Hour < busStartHour || currentTime.Hour >= busEndHour) && busOut)
        // {
        //     Debug.Log("BusGestion Destroy buses");
        //     
        //     
        //     // Destroy buses
        //     foreach (GameObject bus in buses)
        //     {
        //         Destroy(bus);
        //     }
        //     busOut = false;
        // }
    }
}
