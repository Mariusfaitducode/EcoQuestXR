using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapMouvement
{
    
     public static void ScaleObjectAroundPoint(Transform mapTransform, Vector3 pivot, float scalingSpeed, float scalingMin)
    {
        float newScale = mapTransform.localScale.x + scalingSpeed * Time.deltaTime;
        
        if (newScale < scalingMin)
        {
            newScale = scalingMin;
        }

        float newRatio = newScale / mapTransform.localScale.x;
        
        Debug.Log(newScale);
        Vector3 scaleRatio = new Vector3(newRatio, newRatio, newRatio);
        
        
        // Mettre à jour la scale
        mapTransform.localScale = new Vector3(newScale, newScale, newScale);

        // Calculer la nouvelle position relative du pivot
        Vector3 pivotToPosition = mapTransform.position - pivot;
        Vector3 newPivotToPosition = new Vector3(pivotToPosition.x * scaleRatio.x, pivotToPosition.y * scaleRatio.y, pivotToPosition.z * scaleRatio.z);

        // Mise à jour de la position pour compenser le scaling autour du pivot
        mapTransform.position = pivot + newPivotToPosition;

    }
    
    public static void RotateObjectAroundPoint(Transform obj, Vector3 point, float angle)
    {
        
        Vector3 direction = obj.position - point;
        
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        direction = rotation * direction;
        
        obj.position = point + direction;
        obj.Rotate(0, angle, 0);
    }
    
    
    public static void Translate(Transform mapTransform, Vector3 initialPosition, Renderer renderer, Vector2 rightAxis, float translationSpeed, float mapSize)
    {
        //     vectorPlayer.y = 0;
        float limitTerrain = renderer.material.GetFloat("_Limit_Terrain");
        float mapRotation = renderer.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        
        Vector3 newCoordinates = mapTransform.position + new Vector3(rightAxis.x, 0, rightAxis.y).normalized * Time.deltaTime * translationSpeed;
        
        if (!Collide(mapSize * mapTransform.localScale.x, mapRotation,initialPosition, limitTerrain, newCoordinates, rightAxis))
        {
            mapTransform.position = newCoordinates;
        }
        else
        {
            Debug.Log("Out Of Bound : Left - Right");
        }
    }
    
    
    public static bool Collide(float mapSize, float mapRotation, Vector3 circlePosition, float circleRadius, Vector3 newPosition, Vector2 direction)
    {
        
        // Debug.Log("Map Size : " + mapSize);
        
        float delta = mapSize / 2 - circleRadius;

        Vector3 circleLocalPosition = circlePosition - newPosition;
        
        
        float x = circleLocalPosition.x * Mathf.Cos(mapRotation) - circleLocalPosition.z * Mathf.Sin(mapRotation);
        float z = circleLocalPosition.x * Mathf.Sin(mapRotation) + circleLocalPosition.z * Mathf.Cos(mapRotation);

        if (Mathf.Abs(x) > delta || Mathf.Abs(z) > delta)
        {

            float dotProduct = Vector2.Dot(new Vector2(circleLocalPosition.x, circleLocalPosition.z), direction);
            
            Debug.Log(dotProduct);
            
            if (dotProduct < 0)
            {
                return true;
            }
        }
        
        return false;
    }
}
