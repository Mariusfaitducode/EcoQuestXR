using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FillMapUtils : MonoBehaviour
{
    public static bool AreSpheresColliding(Vector3 position1, int size1, Vector3 position2, int size2)
    {
        float distance = Vector3.Distance(position1, position2);
        return distance < size1 + size2;
    }
    
    public static bool IsVertexInsideCircle(Vector3 vertex, Vector3 circleCenter, float radius) {
        
        float distanceXZ = Mathf.Sqrt((vertex.x - circleCenter.x) * (vertex.x - circleCenter.x) + 
                                      (vertex.z - circleCenter.z) * (vertex.z - circleCenter.z));
    
        return distanceXZ <= radius;
    }
    
    // Verif flat area
    public static float CalculateMean(List<Vector3> vertices) {
        float sum = 0.0f;
        foreach (Vector3 vertex in vertices) {
            sum += vertex.y; // Ajoute la coordonnée y de chaque vertex à la somme
        }
        return sum / vertices.Count; // Retourne la moyenne
    }
    
    public static float CalculateVariance(List<Vector3> vertices, float mean) {
        float sumOfSquares = 0.0f;
        foreach (Vector3 vertex in vertices) {
            sumOfSquares += (vertex.y - mean) * (vertex.y - mean); // Somme des carrés des écarts à la moyenne
        }
        return sumOfSquares / vertices.Count; // Retourne la variance
    }
    
    public static bool IsSurfaceFlat(List<Vector3> vertices, float flatnessThreshold) {
        float mean = CalculateMean(vertices); // Calcul de la moyenne
        float variance = CalculateVariance(vertices, mean); // Calcul de la variance

        if (variance <= flatnessThreshold)
        {
            // Debug.Log("mean : " + mean);
            // Debug.Log("variance : " + variance);
        }
        return variance <= flatnessThreshold; // Compare la variance au seuil de planéité
    }


    public static bool IsHitFromRayCast(Vector3 worldPosition, float maxRaycastDistance = 500f)
    {
        Vector3 rayStart = new Vector3(worldPosition.x, worldPosition.y + maxRaycastDistance, worldPosition.z);
        Vector3 rayDirection = Vector3.down;

        RaycastHit[] hits = Physics.RaycastAll(rayStart, rayDirection, maxRaycastDistance * 2);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Terrain"))
            {
                return true;
            }
        }
        Debug.LogWarning("Raycast cannot hit the terrain mesh.");
        return false;
    }
    
    
    public static float GetHeightFromRaycast(Vector3 worldPosition, float maxRaycastDistance = 500f)
    {
        Vector3 rayStart = new Vector3(worldPosition.x, worldPosition.y + maxRaycastDistance, worldPosition.z);
        Vector3 rayDirection = Vector3.down;
        

        RaycastHit[] hits = Physics.RaycastAll(rayStart, rayDirection, maxRaycastDistance * 2);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Terrain"))
            {
                return hit.point.y;
            }
        }
        Debug.LogWarning("Raycast did not hit the terrain mesh.");
        return worldPosition.y; // Retourner une valeur par défaut ou générer une erreur selon votre gestion d'erreurs
        
    }


    public static void SetChildHeight(GameObject parent)
    {
        if (parent.transform.childCount > 0)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                GameObject child = parent.transform.GetChild(i).gameObject;
                var position = child.transform.position;
                position = new Vector3(position.x, GetHeightFromRaycast(position), position.z);
                child.transform.position = position;
            }
        }
    }


    public static GameObject InstantiateObjectWithScale(GameObject prefab, Transform parent, Vector3 position, Vector3 scale)
    {
        GameObject obj = GameObject.Instantiate(prefab, position, Quaternion.identity);
        
        Vector3 originalScale = obj.transform.localScale;
        
        Vector3 finalScale = new Vector3(
            scale.x / originalScale.x, 
            scale.y / originalScale.y, 
            scale.z / originalScale.z);
        
        obj.transform.localScale = finalScale;
        obj.transform.parent = parent;
        return obj;
    }
    
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
        float distance = Mathf.Abs(line.x * point.x + line.y * point.y + line.z) / Mathf.Sqrt(line.x * line.x + line.y * line.y);
        return distance;
    }
    
    
    public static int GenerateGaussian(double mean = 5, double stdDev = 1)
    {
        // Random random = new Random();

        double u1 = 1.0 - Random.value; // uniform(0,1] random doubles
        double u2 = 1.0 - Random.value;
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                               Math.Sin(2.0 * Math.PI * u2); // random normal(0,1)
        double gaussianValue = mean + stdDev * randStdNormal; // random normal(mean,stdDev^2)
        
        return (int)Mathf.Round((float)gaussianValue);
    }
    
    public static Vector3 GetPerpendicularDirection(Vector3 currentDirection, Vector3 nextDirection)
    {
        Vector3 averageDirection = (currentDirection + nextDirection).normalized;
        return Vector3.Cross(averageDirection, Vector3.up);
    }
    
    
}
