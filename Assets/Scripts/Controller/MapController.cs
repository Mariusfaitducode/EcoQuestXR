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
    public float scalingSpeed = 1f;
    
    // Move
    private Vector3 _initialPosition;
    private Vector2 _tempPosition;
    private bool _initialPositionFound = false;
    private Vector2 _movingAxis = Vector2.zero;
    public float movingSpeed = 1f;

    // Update is called once per frame
    void Update()
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
        
        // Get right controller thumbstick axis
        Vector2 rightAxisTemp = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        // Get right controller thumbstick axis
        bool rightButtonThumbstickTemp = OVRInput.Get(OVRInput.Button.SecondaryThumbstick);
        // Get left controller thumbstick axis
        Vector2 leftAxisTemp = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        // Rotate
        // TODO : Lock an axis 
        if (_rotationAxis != leftAxisTemp.x)
        {
            _rotationAxis = leftAxisTemp.x;
            float newY = leftAxisTemp.x * rotationSpeed;
            transform.Rotate(0.0f, newY, 0.0f, Space.World);
            Debug.Log("Rotating : " + transform.rotation);
        }
        
        // Scale
        if (_scalingAxis != leftAxisTemp.y)
        {
            _scalingAxis = leftAxisTemp.y;
            float newS = mapGenerator.transform.localScale.x + leftAxisTemp.y * rotationSpeed;
            mapGenerator.transform.localScale = new Vector3(newS, newS, newS);
            Debug.Log("Scaling : " + transform.rotation);
        }
        
        // moving
        // TODO : move dans la direction que pointe le joueur
        if (_movingAxis != rightAxisTemp)
        {
            _movingAxis = rightAxisTemp;
            _tempPosition = new Vector2(_tempPosition.x + rightAxisTemp.x * movingSpeed,
                _tempPosition.y + rightAxisTemp.y * movingSpeed);
            
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
