using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FillMapArea : MonoBehaviour
{
    

    public GameObject cubePrefab;

    [System.Serializable]
    

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
    
    public MapGenerator mapGenerator;
    
    public bool autoUpdate;
    
    
    internal bool validPosition = false;
    
    
    public void SetAreaInEditor()
    {
        mapGenerator.DrawMapInEditor();
        if (mapGenerator.meshData != null)
        {
            PlaceAreaOnMap(mapGenerator.meshData, mapGenerator.terrainData.uniformScale);
        }
    }
    
    public void FillAreaInEditor()
    {
        mapGenerator.DrawMapInEditor();

        if (validPosition)
        {
            FillAreaOnMap(mapGenerator.meshData, mapGenerator.terrainData.uniformScale);
        }
        
    }
    
    public void PlaceAreaOnMap(MeshData meshData, float uniformScale)
    {
        // int maxTries = this.maxTries;
        this.validPosition = SetAreaPosition.FindAreaPosition(areas, meshData, uniformScale,  minHeight, flatnessThreshold, this.maxTries, maxMapIteration);
        Debug.Log(this.maxTries + " tries");

        if (validPosition)
        {
            Debug.Log("All areas placed successfully");
        }
        else
        {
            Debug.Log("Areas placement failed, map not good");
        }
        for (int i = 0; i < areas.Count; i++)
        {
            areas[i].sphere.SetActive(validPosition);
        }
    }
    
    public void FillAreaOnMap(MeshData meshData, float uniformScale)
    {
        foreach (Area area in areas)
        {
                
            // On récupère les vertices à l'intérieur du cercle
            for (int j = 0; j < meshData.vertices.Length; j++)
            {
                if (FillMapUtils.IsVertexInsideCircle(meshData.vertices[j], area.sphere.transform.position / uniformScale, area.data.size))
                {
                    area.vertices.Add(meshData.vertices[j]);
                }
            }
            foreach (Transform child in area.sphere.transform)
            {
                DestroyImmediate(child.gameObject);
            }
            FillArea.GenerateAreaContent(area, uniformScale);
        }
        
    }
}
