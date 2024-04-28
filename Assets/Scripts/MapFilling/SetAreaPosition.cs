using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SetAreaPosition
{
    
    public static bool findPosition(List<FillMapArea.Area> areas, MeshData meshData, float uniformScale, float minHeight, float flatnessThreshold, int triesLeft, int maxMapIteration)
    {
        if (triesLeft <= 0)
        {
            return false;
        }
        
        Debug.Log("Try number : " + triesLeft);
        
        List<FillMapArea.Sphere> placedSpheres = new List<FillMapArea.Sphere>();
        
        for(int i = 0; i < areas.Count; i++)
        {
            FillMapArea.Area area = areas[i];
            
            bool validPosition = false;
            int attempts = 0;
            // int maxAttempts = 1000;
            Vector3[] vertices = meshData.vertices;

            while (!validPosition && attempts < maxMapIteration)
            {
                int randomIndex = Random.Range(0, vertices.Length);
                Vector3 newPosition = vertices[randomIndex];

                // Si la position est trop basse, on recommence (éviter la mer)
                bool validHeight = !(newPosition.y < minHeight);
                
                if (!validHeight)
                {
                    attempts++;
                    continue;
                }
                
                // Si la position est déjà occupée, on recommence
                bool collision = false;
                foreach (FillMapArea.Sphere otherSphere in placedSpheres)
                {
                    if (AreSpheresColliding(newPosition, area.size, otherSphere.position, otherSphere.size))
                    {
                        collision = true;
                        break;
                    }
                }

                if (collision)
                {
                    attempts++;
                    continue;
                }
                
                // Position valide, on vérifie la planéité
                List<Vector3> verticesInsideCircle = new List<Vector3>();
        
                // On récupère les vertices à l'intérieur du cercle
                for (int j = 0; j < vertices.Length; j++)
                {
                    if (IsVertexInsideCircle(vertices[j], newPosition, area.size))
                    {
                        verticesInsideCircle.Add(vertices[j]);
                    }
                }
                
                // Si la surface est plane, on place l'area
                if (IsSurfaceFlat(verticesInsideCircle, flatnessThreshold))
                {
                    area.SetPosition(newPosition);
                    PlaceSphere(area, uniformScale);
                    
                    placedSpheres.Add(new FillMapArea.Sphere { position = area.position, size = area.size, type = area.type});
                    
                    validPosition = true;
                }
                attempts++;
            }

            if (!validPosition)
            {
                return findPosition(areas, meshData, uniformScale,  minHeight, flatnessThreshold, triesLeft, maxMapIteration);
            }
            
            // Debug.Log("Attempts done : "+ area.type + " : "+ attempts);
        }
        return true;
    }
    
    static bool AreSpheresColliding(Vector3 position1, int size1, Vector3 position2, int size2)
    {
        float distance = Vector3.Distance(position1, position2);
        return distance < size1 + size2;
    }
    
    static bool IsVertexInsideCircle(Vector3 vertex, Vector3 circleCenter, float radius) {
        
        float distanceXZ = Mathf.Sqrt((vertex.x - circleCenter.x) * (vertex.x - circleCenter.x) + 
                                      (vertex.z - circleCenter.z) * (vertex.z - circleCenter.z));
    
        return distanceXZ <= radius;
    }
    
    // Verif flat area
    static float CalculateMean(List<Vector3> vertices) {
        float sum = 0.0f;
        foreach (Vector3 vertex in vertices) {
            sum += vertex.y; // Ajoute la coordonnée y de chaque vertex à la somme
        }
        return sum / vertices.Count; // Retourne la moyenne
    }
    
    static float CalculateVariance(List<Vector3> vertices, float mean) {
        float sumOfSquares = 0.0f;
        foreach (Vector3 vertex in vertices) {
            sumOfSquares += (vertex.y - mean) * (vertex.y - mean); // Somme des carrés des écarts à la moyenne
        }
        return sumOfSquares / vertices.Count; // Retourne la variance
    }
    
    static bool IsSurfaceFlat(List<Vector3> vertices, float flatnessThreshold) {
        float mean = CalculateMean(vertices); // Calcul de la moyenne
        float variance = CalculateVariance(vertices, mean); // Calcul de la variance

        if (variance <= flatnessThreshold)
        {
            // Debug.Log("mean : " + mean);
            // Debug.Log("variance : " + variance);
        }
        return variance <= flatnessThreshold; // Compare la variance au seuil de planéité
    }
    
    static void PlaceSphere(FillMapArea.Area area, float scale) {
        
        Vector3 newPosition = area.position * scale;

        GameObject sphere = area.sphere;
        float radius = area.size;
        
        sphere.transform.position = newPosition;
        
        sphere.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2); // Multiplier par 2 car le scale est en diamètre
        // sphere.GetComponent<Renderer>().material = area.material;
    }
}
