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
    
    public void PlaceObjectsOnMap(ObjectProperties objectProperties)
    {
        // Get Prefab
        GameObject objectPrefab = ObjectsInitialization.LoadPrefab(objectProperties);
        
        // Place objects on the map
        GameObject cardObject = GameObject.Instantiate(objectPrefab, Vector3.zero, Quaternion.identity);
        // cardObject.AddComponent<Objects>();
        // cardObject.GetComponent<Objects>().objectProperties = objectProperties;
        // cardObject.GetComponent<Objects>().gameManager = gameManager;
        //
        //
        // objectsInstantiated.Add(cardObject);
        
        
    }
}
