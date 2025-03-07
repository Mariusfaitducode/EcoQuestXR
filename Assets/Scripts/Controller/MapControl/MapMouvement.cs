using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapMouvement
{
    
     public static void ScaleObjectAroundPoint(Transform mapTransform, Vector3 pivot, float scalingSpeed, float scalingMin)
    {
        float oldScale = mapTransform.localScale.x;
        float newScale = mapTransform.localScale.x + scalingSpeed * Time.deltaTime;

        if (newScale < scalingMin)
        {
            newScale = scalingMin;

        }

        float newRatio = newScale / oldScale;

        Vector3 scaleRatio = new Vector3(newRatio, newRatio, newRatio);
        
        float y1 = 0;
        bool hit1 = FillMapUtils.IsHitFromRayCast(mapTransform.position);
        if (hit1)
        {
            y1 = FillMapUtils.GetHeightFromRaycast(mapTransform.position);
        }
        float y2 = (y1 - mapTransform.position.y) / oldScale * newScale;
        float delta = y2 - (y1 - mapTransform.position.y);

        // Mettre à jour la scale
        mapTransform.localScale = new Vector3(newScale, newScale, newScale);
        

        // Calculer la nouvelle position relative du pivot
        Vector3 pivotToPosition = mapTransform.position - pivot;
        Vector3 newPivotToPosition = new Vector3(pivotToPosition.x * scaleRatio.x, pivotToPosition.y * scaleRatio.y, pivotToPosition.z * scaleRatio.z);
        
        mapTransform.position = new Vector3(pivot.x + newPivotToPosition.x, mapTransform.position.y - delta, pivot.z + newPivotToPosition.z);
     }
     
     public static Vector3 GetPositionFromScaleObjectAroundPoint(Vector3 mapPosition, Vector3 pivot, float mapScale, float scale)
     {
         float newScale = scale;
         
         float newRatio = newScale / mapScale;

         Vector3 scaleRatio = new Vector3(newRatio, newRatio, newRatio);
         
         float y1 = 0;
         bool hit1 = FillMapUtils.IsHitFromRayCast(mapPosition);
         if (hit1)
         {
             y1 = FillMapUtils.GetHeightFromRaycast(mapPosition);
         }
         else
         {
             Debug.Log("did not hit");
         }
         float y2 = (y1 - mapPosition.y) / mapScale * newScale;
         float delta = y2 - (y1 - mapPosition.y);

         // Calculer la nouvelle position relative du pivot
         Vector3 pivotToPosition = mapPosition - pivot;
         Vector3 newPivotToPosition = new Vector3(pivotToPosition.x * scaleRatio.x, pivotToPosition.y * scaleRatio.y, pivotToPosition.z * scaleRatio.z);

         // Mise à jour de la position pour compenser le scaling autour du pivot
         return new Vector3(pivot.x + newPivotToPosition.x, mapPosition.y - delta, pivot.z + newPivotToPosition.z);
         //mapTransform.position = new Vector3(pivot.x + newPivotToPosition.x, mapTransform.hierarchyCapacity, pivot.y + newPivotToPosition.y);

     }

    public static void RotateObjectAroundPoint(Transform obj, Vector3 point, float angle)
    {
        //Debug.Log(point);
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
    
    
    public static Vector2 RotateReference(Vector2 vector, float angle)
    {
        float x = vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle);
        float y = vector.x * Mathf.Sin(angle) + vector.y * Mathf.Cos(angle);
        
        return new Vector2(x, y);
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
            
            // Debug.Log(dotProduct);
            
            if (dotProduct < 0)
            {
                return true;
            }
        }
        
        return false;
    }
}
