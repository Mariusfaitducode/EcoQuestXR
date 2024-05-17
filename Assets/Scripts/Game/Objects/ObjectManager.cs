using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameManager gameManager;
    
    internal List<GameObject> objectsPrefab;
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
}
