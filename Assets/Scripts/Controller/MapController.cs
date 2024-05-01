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
    private Vector3 _initialPosition;
    private Vector2 _tempPosition;

    private bool playerHasMoved = false;
    private bool tableFound = false;
    
    public GameObject player;

    
    //Shader
    private Material _material;
    
    void Start()
    {
        _material = GetComponent<Renderer>().material;
    }
        
        

    // Update is called once per frame
    void Update()
    {
        if (!tableFound)
        {
            findTable();
            
        }
        else if (!playerHasMoved)
        {
            GetTableLocation();
            SetMapPosition();
        }
        else
        {
            GetTableLocation();
            ScaleRotate();
            Move();
        }
        
    }

    void findTable()
    {
        _table = GameObject.FindGameObjectWithTag("Table");
        if (_table != null)
        {
            tableFound = true;
            Debug.Log("Table found");
        }
    }
    
    // Get the location of table (useful because on the headset, it's not very precise)
    void GetTableLocation()
    {
        if (transform.position != _table.transform.position)
        {
            _initialPosition = _table.transform.position;
        }
    }

    // While the player has not interacted with map, we continue checking for the table position.
    void SetMapPosition()
    {
        transform.position = _initialPosition;
        _material.SetVector("_Map_Center", new Vector2(_initialPosition.x, _initialPosition.z));
        
        if (OVRInput.Get(OVRInput.RawAxis2D.RThumbstick) != Vector2.zero | OVRInput.Get(OVRInput.RawAxis2D.LThumbstick) != Vector2.zero)
        {
            playerHasMoved = true;
        }
    }

    void ScaleRotate()
    {
        // Get left controller thumbstick axis
        Vector2 leftAxisTemp = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        
        // Rotate or Scale
        if (Math.Abs(leftAxisTemp.x) > 0.5 | Math.Abs(leftAxisTemp.y) > 0.5)
        {
            // Check if rotating rather than scaling
            if (Math.Abs(leftAxisTemp.x) > Math.Abs(leftAxisTemp.y))
            {
                float newY = leftAxisTemp.x * rotationSpeed * Time.deltaTime;
                transform.Rotate(0.0f, newY, 0.0f, Space.World);
                //Debug.Log("Rotating : " + transform.rotation);
            }
            else
            {
                float newS = transform.localScale.x + leftAxisTemp.y * scalingSpeed * Time.deltaTime;
                transform.localScale = new Vector3(newS, newS, newS);
                //Debug.Log("Scaling : " + transform.localScale);
            }
        }
    }

    void Move()
    {
        // Get right controller thumbstick axis
        Vector2 rightAxisTemp = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        // Get right controller thumbstick axis
        bool rightButtonThumbstickTemp = OVRInput.Get(OVRInput.RawButton.RThumbstick);
        
        Vector3 vectorPlayer = _initialPosition - player.transform.position;
        vectorPlayer.y = 0;
        
        float circleRadius = _material.GetFloat("_Limit_Terrain");
        
        if (Math.Abs(rightAxisTemp.x) > 0.5)
        {
            Vector3 newHorizontalCoordinates = transform.position + new Vector3(vectorPlayer.z, vectorPlayer.y, -vectorPlayer.x).normalized * Time.deltaTime * movingSpeed * rightAxisTemp.x;
            if (!Utils.Collide(this.gameObject, _initialPosition, circleRadius, newHorizontalCoordinates))
            {
                transform.position = newHorizontalCoordinates;
            }
            else
            {
                Debug.Log("Out Of Bound : Left - Right");
            }
            

        }
        
        if (Math.Abs(rightAxisTemp.y) > 0.5)
        {
            Vector3 newVerticalCoordinates = transform.position + vectorPlayer.normalized * Time.deltaTime * movingSpeed * rightAxisTemp.y;
            if (!Utils.Collide(this.gameObject, _initialPosition, circleRadius, newVerticalCoordinates))
            {
                transform.position = newVerticalCoordinates;
            }
            else
            {
                Debug.Log("Out Of Bound : Up - Down");
            }
        }

        // reset position
        if (_initialPosition != transform.position & rightButtonThumbstickTemp)
        {
            transform.position = _initialPosition;
            //Debug.Log("Reset position");
        }
    }

    
}
