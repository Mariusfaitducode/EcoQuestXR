using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameManager gameManager;
    internal List<Area> areas;
    internal List<ObjectProperties> listObjectsProperties;
    
    public float prefabScale = 1f;
    public float mapScale = 1f;
    
    public void ObjectsStartInitialization()
    {
        listObjectsProperties = ObjectsInitialization.InitializeObjectsProperties("Csv/objects");
        
        // TODO : initialize objects already on map
        
    }

    public void SetMapInformations(FillMapManager fillMapManager)
    {
        areas = fillMapManager.areas;
        prefabScale = fillMapManager.prefabScale;
        mapScale = fillMapManager.mapGenerator.terrainData.uniformScale;

        foreach (Area area in areas)
        {
            ObjectsInitialization.InitializeObjectAlreadyOnArea(area, listObjectsProperties, gameManager);
        }
    }
    
    public void PlaceObjects(ObjectProperties objectProperties, int quantity = 1)
    {
        ObjectGestion.PlaceObjectsOnMap(objectProperties, quantity, areas, prefabScale, gameManager, mapScale);
    }
    
    public void RemoveObjects(ObjectProperties objectProperties, int quantity = 1)
    {
        ObjectGestion.RemoveObjectOnMap(objectProperties, quantity, areas);
    }
    
    public void UpgradeObjects(ObjectProperties objectProperties1, ObjectProperties objectProperties2, int quantity1 = 1, int quantity2 = 1)
    {
        RemoveObjects(objectProperties1, quantity1);
        PlaceObjects(objectProperties2, quantity2);
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
        Debug.Log("Lenght22 : " + objectScripts.Count);
        return objectScripts;
    }
}
