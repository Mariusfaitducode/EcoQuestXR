using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool Collide(float mapSize, float mapRotation, Vector3 circleLocation, float circleRadius, Vector3 newLocation)
    {
        
        Debug.Log("Map Size : " + mapSize);
        
        float delta = mapSize / 2 - circleRadius;
        
        // Debug.Log("delta : " + delta);

        Vector3 circlePosition = circleLocation - newLocation;
        
        // float theta = 0.785398f; 
        
        float x = circlePosition.x * Mathf.Cos(mapRotation) - circlePosition.z * Mathf.Sin(mapRotation);
        float z = circlePosition.x * Mathf.Sin(mapRotation) + circlePosition.z * Mathf.Cos(mapRotation);

        Vector3 rotatedPosition =  new Vector3(x, circlePosition.y, z);

        // Debug.Log("Map Rotation : " + mapRotation);
        // Debug.Log("Circle Position : " + circlePosition);
        Debug.Log("Rotated Position : " + rotatedPosition);

        
        if (Mathf.Abs(x) > delta || Mathf.Abs(z) > delta)
        {
            return true;
        }
        
        return false;
    }
}
