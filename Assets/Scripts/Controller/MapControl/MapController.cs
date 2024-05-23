using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class MapController : MonoBehaviour
{
    [System.Serializable]

    public struct MouvementSettings
    {
        public float rotationSpeed;
        public float movingSpeed;
        public float scalingSpeed;
        public float scalingMin;

    }
    
    public MouvementSettings mouvementSettings;

    // Move
    private GameObject table;

    private bool playerHasMoved = false;
    private bool tableFound = false;
    
    private new Renderer renderer;
    
    public GameObject ovrPlayer;
    
    public UpdateTerrainRenderer updateTerrainRenderer;
    
    public GameManager gameManager;

    public bool useKeyboard = true;
    
    
    private float originalSize;
    // private float originalScale;
    
    // private float epsilon = 0.0001f;
    
    
    
    
    
    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalSize = GetComponent<MeshFilter>().mesh.bounds.size.x;
    }
    
    void Update()
    {

        if (useKeyboard)
        {
            bool moved = KeyBoardMapInteraction.Controller(this.transform, Vector3.zero, renderer, mouvementSettings, originalSize);


            if (moved)
            {
                updateTerrainRenderer.UpdateMapInformations(false);
                updateTerrainRenderer.SetObjectsVisibility(gameManager);
            }
        }
        else
        {
            
            if (!tableFound)
            {
                FindTable();
                return;
            }
            if (!playerHasMoved)
            {
                // GetTableLocation();
                SetMapPosition();
            }
            else
            {
                OvrMapInteraction.Controller(this.transform, table.transform.position, renderer, mouvementSettings, originalSize);
            }
            // Use OVR controller
        }
        
    }
    
    

    void FindTable()
    {
        table = GameObject.FindGameObjectWithTag("Table");
        if (table != null)
        {
            tableFound = true;
            Debug.Log("Table found");
        }
    }

    // While the player has not interacted with map, we continue checking for the table position.
    void SetMapPosition()
    {
        Vector3 initialPosition = table.transform.position;
        transform.position = initialPosition;
        
        updateTerrainRenderer.UpdateLimitTerrainCenter();
        
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick) != Vector2.zero || OVRInput.Get(OVRInput.RawAxis2D.LThumbstick) != Vector2.zero)
        {
            playerHasMoved = true;
        }
    }
    
    

    

    
}
