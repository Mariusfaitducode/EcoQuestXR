using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameManager gameManager;

    internal List<GameObject> objectsInstantiated;
    internal List<Area> areas;
    internal List<ObjectProperties> objectsProperties;
    
    
    public void ObjectsStartInitialization()
    {
        objectsProperties = ObjectsInitialization.InitializeObjectsProperties("Csv/objects");
        
    }

    public void SetAreas(List<Area> areas)
    {
        this.areas = areas;
    }
    
    public void PlaceObjects(ObjectProperties objectProperties, int quantity = 1)
    {
        for (int i = 0; i < quantity; i++)
        {
            ObjectGestion.PlaceObjectOnMap(objectProperties);
        }
    }
    
    public void RemoveObjects(ObjectProperties objectProperties, int quantity = 1)
    {
        for (int i = 0; i < quantity; i++)
        {
            ObjectGestion.RemoveObjectOnMap(objectProperties);
        }
    }
    
    public void UpgradeObjects(ObjectProperties objectProperties1, ObjectProperties objectProperties2, int quantity1 = 1, int quantity2 = 1)
    {
        RemoveObjects(objectProperties1, quantity1);
        PlaceObjects(objectProperties2, quantity2);
    }
}
