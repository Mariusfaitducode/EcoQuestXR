using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SetAreaPosition
{
    
    public static bool FindAreaPosition(List<Area> areas, MeshData meshData, float minHeight, int triesLeft, int maxMapIteration)
    {
        if (triesLeft <= 0)
        {
            return false;
        }
        
        Debug.Log("Try number : " + triesLeft);
        
        List<FillMapManager.Sphere> placedSpheres = new List<FillMapManager.Sphere>();
        
        for(int i = 0; i < areas.Count; i++)
        {
            Area area = areas[i];
            
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
                foreach (FillMapManager.Sphere otherSphere in placedSpheres)
                {
                    if (FillMapUtils.AreSpheresColliding(newPosition, area.data.radius, otherSphere.position, otherSphere.size))
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
                    if (FillMapUtils.IsVertexInsideCircle(vertices[j], newPosition, area.data.radius))
                    {
                        verticesInsideCircle.Add(vertices[j]);
                    }
                }
                
                // Si la surface est plane, on place l'area
                if (FillMapUtils.IsSurfaceFlat(verticesInsideCircle, area.flatnessThreshold))
                {
                    area.SetPosition(newPosition);
                    PlaceSphere(area);
                    
                    placedSpheres.Add(new FillMapManager.Sphere { position = area.position, size = area.data.radius, type = area.data.type});
                    
                    validPosition = true;
                }
                attempts++;
            }

            if (!validPosition)
            {
                return FindAreaPosition(areas, meshData, minHeight, triesLeft - 1, maxMapIteration);
            }
            
            // Debug.Log("Attempts done : "+ area.type + " : "+ attempts);
        }
        return true;
    }
    
    static void PlaceSphere(Area area) {
        
        Vector3 newPosition = area.position;

        GameObject sphere = area.sphere;
        float radius = area.data.radius;
        
        sphere.transform.position = newPosition;
        
        sphere.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2); // Multiplier par 2 car le scale est en diamètre
        // sphere.GetComponent<Renderer>().material = area.material;
    }
}
