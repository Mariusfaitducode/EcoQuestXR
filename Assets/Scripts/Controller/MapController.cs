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
    //public GameObject mapGenerator;
    public float scalingSpeed = 0.001f;
    
    // Move
    private Vector3 _initialPosition;
    private Vector2 _tempPosition;
    private bool _initialPositionFound = false;
    private Vector2 _movingAxis = Vector2.zero;
    public float movingSpeed = 0.05f;

    // Update is called once per frame
    void Update()
    {
        /*Vector2 leftAxisTemp = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
        Vector2 rightAxisTemp = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);

        Debug.Log("leftinput : " + OVRInput.Get(OVRInput.RawAxis2D.LThumbstick)+ "rightinput : " + OVRInput.Get(OVRInput.RawAxis2D.RThumbstick)+ "varleft : " +leftAxisTemp+ "varright : "+rightAxisTemp);*/
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
            // Check iif rotating rather than scaling
            if (Math.Abs(leftAxisTemp.x) > Math.Abs(leftAxisTemp.y))
            {
                _rotationAxis = leftAxisTemp.x;
                float newY = leftAxisTemp.x * rotationSpeed * Time.deltaTime;
                transform.Rotate(0.0f, newY, 0.0f, Space.World);
                Debug.Log("Rotating : " + transform.rotation);
            }
            else
            {
                _scalingAxis = leftAxisTemp.y;
                float newS = transform.localScale.x + leftAxisTemp.y * scalingSpeed * Time.deltaTime;
                transform.localScale = new Vector3(newS, newS/10, newS);
                Debug.Log("Scaling : " + transform.localScale);
            }
        }
    }

    void Move()
    {
        // Get right controller thumbstick axis
        Vector2 rightAxisTemp = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        // Get right controller thumbstick axis
        bool rightButtonThumbstickTemp = OVRInput.Get(OVRInput.RawButton.RThumbstick);
        
        // moving
        // TODO : move dans la direction que pointe le joueur
        if (_movingAxis != rightAxisTemp)
        {
            _movingAxis = rightAxisTemp;
            _tempPosition = new Vector2(_tempPosition.x + rightAxisTemp.x * movingSpeed * Time.deltaTime,
                _tempPosition.y + rightAxisTemp.y * movingSpeed * Time.deltaTime);
            
            transform.position = new Vector3(_initialPosition.x + _tempPosition.x, _initialPosition.y, _initialPosition.z + _tempPosition.y);
            Debug.Log("Moving : " + transform.rotation);
        }
        
        // reset position
        if (_initialPosition != transform.position & rightButtonThumbstickTemp)
        {
            transform.position = _initialPosition;
            Debug.Log("Reset position");
        }
    }
}
