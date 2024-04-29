using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class MapController : MonoBehaviour
{
    // Rotation
    private float _rotationAxis = 0.0f;
    public float rotationSpeed = 5f;
    
    // Scale
    private float _scalingAxis = 0.0f;
    public GameObject mapGenerator;
    public float scalingSpeed = 0.001f;
    
    // Move
    private Vector3 _initialPosition;
    private Vector2 _tempPosition;
    private bool _initialPositionFound = false;
    private Vector2 _movingAxis = Vector2.zero;
    public float movingSpeed = 0.05f;
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        GetInitialPosition();

        ScaleRotate();
        
        Move();
    }

    void GetInitialPosition()
    {
        if (!_initialPositionFound)
        {
            _initialPosition = transform.position;
            if (_initialPosition != Vector3.zero)
            {
                _initialPositionFound = true;
                Debug.Log("Initial position found : " + _initialPosition);
            }
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
                Debug.Log("Rotating : " + transform.rotation);
            }
            else
            {
                // Cube
                float newS = transform.localScale.x + leftAxisTemp.y * scalingSpeed * Time.deltaTime;
                transform.localScale = new Vector3(newS, newS, newS);
                Debug.Log("Scaling : " + transform.localScale);
                // Map
                /*float newS = mapGenerator.transform.localScale.x + leftAxisTemp.y * scalingSpeed * Time.deltaTime;
                mapGenerator.transform.localScale = new Vector3(newS, newS/10, newS);
                Debug.Log("Scaling : " + mapGenerator.transform.localScale);*/
            }
        }
    }

    void Move()
    {
        // Get right controller thumbstick axis
        Vector2 rightAxisTemp = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        // Get right controller thumbstick axis
        bool rightButtonThumbstickTemp = OVRInput.Get(OVRInput.RawButton.RThumbstick);

        
        Vector3 playerPos = player.transform.position;
        Vector3 mapPos = _initialPosition;
        
        //Vector3 vectorMap = new Vector3(1, 0, 0);
        Vector3 vectorPlayer = mapPos - playerPos;
        vectorPlayer.y = 0;
        Debug.Log(vectorPlayer);

        if (Math.Abs(rightAxisTemp.x) > 0.5)
        {
            this.transform.Translate(new Vector3(vectorPlayer.z, vectorPlayer.y, -vectorPlayer.x).normalized * Time.deltaTime * movingSpeed * rightAxisTemp.x);
        }
        
        if (Math.Abs(rightAxisTemp.y) > 0.5)
        {
            this.transform.Translate(vectorPlayer.normalized * Time.deltaTime * movingSpeed * rightAxisTemp.y);
        }

        // reset position
        if (_initialPosition != transform.position & rightButtonThumbstickTemp)
        {
            transform.position = _initialPosition;
            Debug.Log("Reset position");
        }
    }
}
