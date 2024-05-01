using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class MapController : MonoBehaviour
{
    // Speed
    public float rotationSpeed = 5f;
    public float scalingSpeed = 0.001f;
    public float movingSpeed = 0.05f;

    // Move
    private GameObject _table;
    private Vector2 _tempPosition;

    private bool playerHasMoved = false;
    private bool tableFound = false;
    
    private Renderer _renderer;
    
    public GameObject player;
    
    public UpdateTerrainRenderer updateTerrainRenderer;

    private float originalSize;
    private float originalScale;
    
    private float epsilon = 0.0001f;
    
    //Shader
    // private Material _material;
    
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        // _material = _renderer.material;
        
        originalSize = _renderer.bounds.size.x;
        originalScale = transform.localScale.x;
    }
        
        

    // Update is called once per frame
    void Update()
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
            return;
        }
        // Left axis
        
        
        
        Vector2 leftAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        
        // Rotate or Scale
        if (Math.Abs(leftAxis.x) > 0.5 | Math.Abs(leftAxis.y) > 0.5)
        {
            // Check if rotating rather than scaling
            if (Math.Abs(leftAxis.x) > Math.Abs(leftAxis.y))
            {
                MapInteraction.Rotate(this.transform, _table.transform.position, leftAxis, rotationSpeed);
            }
            else
            {
                MapInteraction.Scale(this.transform, leftAxis, scalingSpeed);
                
                Debug.Log("Original Scale : " + originalScale.ToString("F8"));
                Debug.Log("Local scale" + transform.localScale.ToString("F8"));
                Debug.Log("Lossy scale" + transform.lossyScale.ToString("F8"));
                originalSize = originalSize * transform.localScale.x / originalScale + epsilon;
            }
        }
        
        // Debug.Log("Local scale" + transform.localScale);
        Debug.Log("Original Size : " + originalSize);
        // Debug.Log("Mesh bounds" + _renderer.bounds.size);

        // Right axis
        
        Vector2 rightAxis = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);

        Vector3 vectorPlayer = _table.transform.position - player.transform.position;
        
        if (Math.Abs(rightAxis.x) > 0.5 | Math.Abs(rightAxis.y) > 0.5)
        {
            // Move horizontal
            MapInteraction.Translate(this.transform, _table.transform, _renderer, rightAxis, movingSpeed, originalSize);
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
        _table = GameObject.FindGameObjectWithTag("Table");
        if (_table != null)
        {
            tableFound = true;
            Debug.Log("Table found");
        }
    }
    
    // Get the location of table (useful because on the headset, it's not very precise)
    // void GetTableLocation()
    // {
    //     if (transform.position != _table.transform.position)
    //     {
    //         _initialPosition = _table.transform.position;
    //     }
    // }

    // While the player has not interacted with map, we continue checking for the table position.
    void SetMapPosition()
    {
        Vector3 initialPosition = _table.transform.position;
        transform.position = initialPosition;
        
        updateTerrainRenderer.UpdateCenter();
        
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick) != Vector2.zero || OVRInput.Get(OVRInput.RawAxis2D.LThumbstick) != Vector2.zero)
        {
            playerHasMoved = true;
        }
    }
    
    

    

    
}
