using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FillMapArea : MonoBehaviour
{
    public enum AreaType { City, Industry, Energy, Agriculture };

    [System.Serializable]
    public struct Area
    {
        public AreaType type;
        internal Vector3 position;
        public int size;
        public GameObject sphere;
        // public Material material;
        public bool valid;
        
        public void SetPosition(Vector3 position)
        {
            this.position = position;
        }
    }

    public struct Sphere
    {
        public Vector3 position;
        public int size;
        public AreaType type;
    }
    
    public float minHeight = 0.1f;
    public float flatnessThreshold = 0.1f;
    
    public int maxTries = 5;
    public int maxMapIteration = 200;
    
    public List<Area> areas;
    
    
    public void PlaceAreaOnMap(MeshData meshData, float uniformScale)
    {
        // int maxTries = this.maxTries;
        bool valid = findPosition(meshData, uniformScale, this.maxTries);
        Debug.Log(this.maxTries + " tries");

        if (valid)
        {
            Debug.Log("All areas placed successfully");
        }
        else
        {
            Debug.Log("Areas placement failed, map not good");
        }
        for (int i = 0; i < areas.Count; i++)
        {
            areas[i].sphere.SetActive(valid);
        }
    }

    public bool findPosition(MeshData meshData, float uniformScale, int triesLeft)
    {
        if (triesLeft <= 0)
        {
            return false;
        }
        
        Debug.Log("Try number : " + (maxTries - triesLeft));
        
        List<Sphere> placedSpheres = new List<Sphere>();
        
        for(int i = 0; i < areas.Count; i++)
        {
            Area area = areas[i];
            
            bool validPosition = false;
            int attempts = 0;
            // int maxAttempts = 1000;
            Vector3[] vertices = meshData.vertices;

            while (!validPosition && attempts < this.maxMapIteration)
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
                foreach (Sphere otherSphere in placedSpheres)
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
                    
                    placedSpheres.Add(new Sphere { position = area.position, size = area.size, type = area.type});
                    
                    validPosition = true;
                }
                attempts++;
            }

            if (!validPosition)
            {
                return findPosition(meshData, uniformScale, triesLeft - 1);
            }
            
            // Debug.Log("Attempts done : "+ area.type + " : "+ attempts);
        }
        return true;
    }
    
    bool IsVertexInsideCircle(Vector3 vertex, Vector3 circleCenter, float radius) {
        
        float distanceXZ = Mathf.Sqrt((vertex.x - circleCenter.x) * (vertex.x - circleCenter.x) + 
                                      (vertex.z - circleCenter.z) * (vertex.z - circleCenter.z));
    
        return distanceXZ <= radius;
    }
    
    

    bool AreSpheresColliding(Vector3 position1, int size1, Vector3 position2, int size2)
    {
        float distance = Vector3.Distance(position1, position2);
        return distance < size1 + size2;
    }
    
    // Verif flat area
    float CalculateMean(List<Vector3> vertices) {
        float sum = 0.0f;
        foreach (Vector3 vertex in vertices) {
            sum += vertex.y; // Ajoute la coordonnée y de chaque vertex à la somme
        }
        return sum / vertices.Count; // Retourne la moyenne
    }
    
    float CalculateVariance(List<Vector3> vertices, float mean) {
        float sumOfSquares = 0.0f;
        foreach (Vector3 vertex in vertices) {
            sumOfSquares += (vertex.y - mean) * (vertex.y - mean); // Somme des carrés des écarts à la moyenne
        }
        return sumOfSquares / vertices.Count; // Retourne la variance
    }
    
    bool IsSurfaceFlat(List<Vector3> vertices, float flatnessThreshold) {
        float mean = CalculateMean(vertices); // Calcul de la moyenne
        float variance = CalculateVariance(vertices, mean); // Calcul de la variance

        if (variance <= flatnessThreshold)
        {
            // Debug.Log("mean : " + mean);
            // Debug.Log("variance : " + variance);
        }
        return variance <= flatnessThreshold; // Compare la variance au seuil de planéité
    }
    
    
    public void PlaceSphere(Area area, float scale) {
        
        Vector3 newPosition = area.position * scale;

        GameObject sphere = area.sphere;
        float radius = area.size;
        
        sphere.transform.position = newPosition;
        
        sphere.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2); // Multiplier par 2 car le scale est en diamètre
        // sphere.GetComponent<Renderer>().material = area.material;
    }
    
    
}
