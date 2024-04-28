using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class requestSceneCapture : MonoBehaviour
{
    // A reference to the OVRSceneManager prefab.
    public OVRSceneManager ovrSceneManager;
    
    // Start is called before the first frame update
    void Start()
    {
        ovrSceneManager.RequestSceneCapture();
        Debug.Log("done");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
