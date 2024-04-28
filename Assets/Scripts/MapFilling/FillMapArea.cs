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
    
    public MapGenerator mapGenerator;
    
    public bool autoUpdate;
    
    
    public void SetAreaInEditor()
    {
        mapGenerator.DrawMapInEditor();
        PlaceAreaOnMap(mapGenerator.meshData, mapGenerator.terrainData.uniformScale);
    }
    
    public void PlaceAreaOnMap(MeshData meshData, float uniformScale)
    {
        // int maxTries = this.maxTries;
        bool valid = SetAreaPosition.findPosition(areas, meshData, uniformScale,  minHeight, flatnessThreshold, this.maxTries, maxMapIteration);
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
}
