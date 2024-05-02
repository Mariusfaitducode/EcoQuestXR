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
    
    private Renderer renderer;
    
    public GameObject ovrPlayer;
    
    public UpdateTerrainRenderer updateTerrainRenderer;

    public bool useKeyboard = true;
    
    
    private float originalSize;
    // private float originalScale;
    
    private float epsilon = 0.0001f;
    
    
    
    
    
    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalSize = GetComponent<MeshFilter>().mesh.bounds.size.x;
    }
    
    void Update()
    {
        // if (!tableFound)
        // {
        //     FindTable();
        //     return;
        // }
        // if (!playerHasMoved)
        // {
        //     // GetTableLocation();
        //     SetMapPosition();
        //     return;
        // }
        // Left axis
        
        
        // LEFT AXIS
        
        // Vector2 leftAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);



        if (useKeyboard)
        {
            KeyBoardMapInteraction.Controller(this.transform, Vector3.zero, renderer, mouvementSettings, originalSize);
        }
        else
        {
            // Use OVR controller
        }
        
        


        // Vector2 leftAxis = ;
        
        // Rotate or Scale
        // if (Math.Abs(leftAxis.x) > 0.5 | Math.Abs(leftAxis.y) > 0.5)
        // {
        //     // Check if rotating rather than scaling
        //     if (Math.Abs(leftAxis.x) > Math.Abs(leftAxis.y))
        //     {
        //         MapInteraction.Rotate(this.transform, _table.transform.position, leftAxis, rotationSpeed);
        //     }
        //     else
        //     {
        //         MapInteraction.Scale(this.transform, leftAxis, scalingSpeed);
        //         
        //         Debug.Log("Original Scale : " + originalScale.ToString("F8"));
        //         Debug.Log("Local scale" + transform.localScale.ToString("F8"));
        //         Debug.Log("Lossy scale" + transform.lossyScale.ToString("F8"));
        //         originalSize = originalSize * transform.localScale.x / originalScale + epsilon;
        //     }
        // }
        
        // Debug.Log("Local scale" + transform.localScale);
        // Debug.Log("Original Size : " + originalSize);
        // Debug.Log("Mesh bounds" + _renderer.bounds.size);

        // RIGHT AXIS
        
        Vector2 rightAxis = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);

        // Vector3 vectorPlayer = _table.transform.position - player.transform.position;
        
        if (Math.Abs(rightAxis.x) > 0.5 | Math.Abs(rightAxis.y) > 0.5)
        {
            // Move horizontal
            // MapInteraction.Translate(this.transform, table.transform, _renderer, rightAxis, movingSpeed, originalSize);
        }

        // if (Math.Abs(rightAxis.y) > 0.5)
        // {
        //     // Move vertical
        //     MapInteraction.Translate(this.transform, _table.transform, _material.GetFloat("_Limit_Terrain"), rightAxis, movingSpeed);
        //
        // }
        
        bool rightButtonThumbstickTemp = OVRInput.Get(OVRInput.RawButton.RThumbstick);

        
        
        // Click left axis
        
        
        
        
        
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
        
        updateTerrainRenderer.UpdateCenter();
        
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick) != Vector2.zero || OVRInput.Get(OVRInput.RawAxis2D.LThumbstick) != Vector2.zero)
        {
            playerHasMoved = true;
        }
    }
    
    

    

    
}
