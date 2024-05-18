using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameManager gameManager;

    internal List<GameObject> objectsInstantiated;
    internal List<Area> areas;
    internal List<ObjectProperties> objectsProperties;
    internal bool areObjectsPropertiesInitialized = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get all the objects properties from the CSV file
        objectsProperties = ObjectsInitialization.InitializeObjectsProperties("Csv/objects");
        areObjectsPropertiesInitialized = true;
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
        // Get Prefab
        GameObject objectPrefab = ObjectsInitialization.LoadPrefab(objectProperties);
        
        // TODO : Initialize object script with objectProperties and gameManager
        
        
        // Place objects on the map
        GameObject cardObject = GameObject.Instantiate(objectPrefab, Vector3.zero, Quaternion.identity);
        // cardObject.AddComponent<Objects>();
        // cardObject.GetComponent<Objects>().objectProperties = objectProperties;
        // cardObject.GetComponent<Objects>().gameManager = gameManager;
        //
        //
        // objectsInstantiated.Add(cardObject);
        
        // TODO : Place object on the map in the good area
        
        // Place in good area object
        
        
        // TODO : Update area properties
        
        
        
        // ? List objects instantiated
    }
}
