using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
    
    public MeshCollider meshCollider;
    
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
            //Destroy all children
            while (area.sphere.transform.childCount > 0)
            {
                Transform child = area.sphere.transform.GetChild(0);
                DestroyImmediate(child.gameObject);
            }

            area.uniformSize = area.data.size * uniformScale;
            area.uniformStartSize = area.data.startSize * uniformScale;
            
            area.areaGrid = FillMapUtils.CreateGrid(area.sphere.transform.position, area.uniformSize, area.data.gridCellSize);
            
            // int count = 0;
            // for (int i = 0; i < area.areaGrid.Count; i++)
            // {
            //     
            //         count++;
            //         Vector3 newPosition = area.areaGrid[i];
            //         newPosition.y = FillMapUtils.GetHeightFromRaycast(newPosition);
            //         
            //         Debug.Log(newPosition.y);
            //     
            //         GameObject cube = Instantiate(area.data.prefabs, newPosition, Quaternion.identity);
            //         cube.transform.parent = area.sphere.transform;
            //     
            // }
            // Debug.Log(count + " prefabs instantiated");


            int[,] roads = RoadGenerator.GenerateRoadContent(area);


            
                FillArea.GenerateAreaContent(area, roads);
            
            
        }
        
    }
}
