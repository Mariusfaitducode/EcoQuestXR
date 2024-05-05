using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsGeneration
{
    // Get line ax + by +c = 0 from 2 2D points
    public static Vector3 GetLineFrom2Points(Vector2 point1, Vector2 point2)
    {
        // Get slope
        Vector2 u = new Vector2(point2.x - point1.x, point2.y - point1.y);
        
        // Convert to normal
        Vector2 n = new Vector2(u.y, -u.x);

        Vector3 factors = new Vector3(
            n.x, //a
            n.y, //b
            -(n.x * point1.x + n.y * point1.y)); //c

        return factors;
    }

    public static float DistancePointToLine(Vector2 point, Vector3 line)
    {
        float distance = Math.Abs(line.x * point.x + line.y * point.y + line.z) / (float)Math.Sqrt(line.x * line.x + line.y * line.y);
        return distance;
    }
}
