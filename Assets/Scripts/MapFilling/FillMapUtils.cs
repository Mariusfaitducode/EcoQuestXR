using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    
}