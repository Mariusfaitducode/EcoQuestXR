using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    internal GameManager gameManager;
    internal List<Area> areas;
    internal List<ObjectProperties> listObjectsProperties;
    internal List<ObjectProperties> subObjectsProperties;
    
    public string objectsPath = "Csv/objects";
    public string subObjectsPath = "Csv/subObjects";
    
    public float prefabScale = 1f;
    // public float mapScale = 1f;

    internal GameObject mesh;


    public void AreasSounds(Camera camera, bool timePaused)
    {
        // bool allAudioOff = true;
        
        foreach (Area area in areas)
        {
            // MapSoundsGestion.PlayAreaSounds(areas);

            if (area.sphere.GetComponent<AudioSource>())
            {
                MapSoundsGestion.AreaAudioSource(area, camera, mesh.transform.localScale.x, timePaused);
                // area.AreaAudioSource(camera, meshTerrain.transform.localScale.x);
            }
            else
            {
                Debug.LogError("No audio source on area sphere");
            }
            
        }
    }
    
    public void ObjectsStartInitialization()
    {
        listObjectsProperties = ObjectsInitialization.InitializeObjectsProperties(objectsPath);
        
        subObjectsProperties = ObjectsInitialization.InitializeObjectsProperties(subObjectsPath);
        
        List<SubObject> subObjects = ObjectsInitialization.ObjectsPropertiesToSubObjects(subObjectsProperties);
        
        ObjectsInitialization.LinkSubObjectsToObjects(listObjectsProperties, subObjects);

    }

    public void SetMapInformations(FillMapManager fillMapManager)
    {
        areas = fillMapManager.areas;
        prefabScale = fillMapManager.prefabScale;
        // mapScale = fillMapManager.mapGenerator.terrainData.uniformScale;

        foreach (Area area in areas)
        {
            ObjectsInitialization.InitializeObjectAlreadyOnArea(area, listObjectsProperties, gameManager);
        }
    }
    
    public void PlaceObjects(ObjectProperties objectProperties, int quantity = 1)
    {
        // Instantiate the object on the map
        List<GameObject> placedObjects = ObjectGestion.PlaceObjectsOnMap(objectProperties, quantity, areas, prefabScale, gameManager, mesh.transform.localScale.x);
        
        if (placedObjects.Count == 0)
        {
            Debug.LogWarning("No object found to place");
        }
        else
        {
            Debug.Log("Object found : Place Object on the map");
            // start animation
            StartCoroutine(gameManager.animationManager.AnimationPlaceObjects(placedObjects));
        }
    }
    
    public void RemoveObjects(ObjectProperties objectProperties, int quantity = 1)
    {
        // Get the objects to remove and delete in lists but not destroy
        List<GameObject> removedObjects = ObjectGestion.RemoveObjectOnMap(objectProperties, quantity, areas);
        
        if (removedObjects.Count == 0)
        {
            Debug.LogWarning("No object found to remove");
        }
        else
        {
            Debug.Log("Object found : Remove Object on the map");
            // start animation
            StartCoroutine(gameManager.animationManager.AnimationRemoveObjects(removedObjects));
        }
    }
    
    public void ReplaceObjects(ObjectProperties objectProperties1, ObjectProperties objectProperties2, int quantity1 = 1, int quantity2 = 1)
    {
        // Get the objects to remove and delete in lists but not destroy
        List<GameObject> removedObjects = ObjectGestion.RemoveObjectOnMap(objectProperties1, quantity1, areas);
        
        // Instantiate the object on the map
        List<GameObject> placedObjects = ObjectGestion.PlaceObjectsOnMap(objectProperties2, quantity2, areas, prefabScale, gameManager, mesh.transform.localScale.x);
        
        if (removedObjects.Count == 0)
        {
            Debug.LogWarning("No object found to remove");
        }
        else if (placedObjects.Count == 0)
        {
            Debug.LogWarning("No object found to place");
        }
        else
        {
            Debug.Log("Object found : Replace Object on the map");
            // start animation
            StartCoroutine(gameManager.animationManager.AnimationReplacementObjects(removedObjects, placedObjects));
        }
    }
    
    
    
    
    public void UpgradeObjects(ObjectProperties objectProperties1, int quantity1, ObjectProperties objectProperties2, bool upgrade)
    {
        // Get the objects to upgrade
        List<GameObject> upgradedObjects = ObjectGestion.UpgradeObjectOnMap(objectProperties1,  quantity1, objectProperties2, areas, upgrade);
        
        if (upgradedObjects.Count == 0)
        {
            Debug.LogWarning("No object found to upgrade");
        }
        else
        {
            Debug.Log("Object found : Upgrade Object on the map");

            if (upgrade)
            {
                StartCoroutine(gameManager.animationManager.AnimationPlaceObjects(upgradedObjects));
            }
            else
            {
                StartCoroutine(gameManager.animationManager.AnimationRemoveObjects(upgradedObjects));
            }
            // start animation
        }
    }
    
    
    
    
    public List<ObjectScript> GetAllObjectScripts()
    {
        List<ObjectScript> objectScripts = new List<ObjectScript>();
        foreach (Area area in areas)
        {
            foreach (GameObject gameObject in area.areaObjects)
            {
                ObjectScript objectScript = gameObject.GetComponent<ObjectScript>();
                objectScripts.Add(objectScript);
            }
        }
        Debug.Log("Number of objects found on the map : " + objectScripts.Count);
        return objectScripts;
    }

    public int GetMaxPopSize()
    {
        int maxPopSize = 0;

        foreach (ObjectScript objectScript in GetAllObjectScripts())
        {
            if (objectScript.objectProperties != null)
            {
                maxPopSize += objectScript.objectProperties.stats.populationSize;
            }
            else
            {
                Debug.LogWarning("ObjectScript without objectProperties");
            }
            
        }
        Debug.Log("Max population size : " + maxPopSize);
        return maxPopSize;
    }
    
    public int GetNumberOfObjectsById(int id)
    {
        int numberOfObjects = 0;
            
        foreach (ObjectScript objectScript in GetAllObjectScripts())
        {
            if (objectScript.objectProperties != null)
            {
                if (objectScript.objectProperties.id == id)
                {
                    numberOfObjects++;
                }
            }
            else
            {
                Debug.LogWarning("Object without objectScript");
            }
            
        }
        Debug.Log("Number of objects with id " + id + " : " + numberOfObjects);
        return numberOfObjects;
    }
}
