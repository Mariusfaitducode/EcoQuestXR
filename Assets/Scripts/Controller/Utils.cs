using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static bool Collide(GameObject map, Vector3 circleLocation, float circleRadius, Vector3 newLocation)
    {
        Renderer renderer = map.GetComponent<Renderer>();
        float size = renderer.bounds.size.x;
        float delta = size / 2 - circleRadius;
        
        float leftBound = newLocation.x - delta;
        float rightBound = newLocation.x + delta;
        float downBound = newLocation.z - delta;
        float upBound = newLocation.z + delta;

        if (circleLocation.x < leftBound |
            circleLocation.x > rightBound |
            circleLocation.z < downBound |
            circleLocation.z > upBound)
        {
            return true;
        }
        
        return false;
    }
}
