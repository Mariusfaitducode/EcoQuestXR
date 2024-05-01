using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FillMapArea : MonoBehaviour
{
    
    public struct Sphere
    {
        public Vector3 position;
        public int size;
        public AreaType type;
    }
    
    
    public GameObject testCube;
    
    public MapGenerator mapGenerator;

    public GameObject meshTerrain;
    
    public GameObject roadParent;
    
    public bool autoUpdate;
    
    public float minHeight = 0.1f;
    
    
    public int maxTries = 5;
    public int maxMapIteration = 200;
    
    
    
    public List<Area> areas;
    
    // public RoadData roadData;
    
    

    
    
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
    
    public void GenerateRoadOnMapInEditor()
    {
        mapGenerator.DrawMapInEditor();
        
        GenerateRoadOnMap(mapGenerator.meshData, mapGenerator.terrainData.uniformScale);
        
    }
    
    public void PlaceAreaOnMap(MeshData meshData, float uniformScale)
    {
        // int maxTries = this.maxTries;
        this.validPosition = SetAreaPosition.FindAreaPosition(areas, meshData, uniformScale,  minHeight,  this.maxTries, maxMapIteration);
        // Debug.Log(this.maxTries + " tries");

        if (validPosition)
        {
            Debug.Log("All areas placed successfully");
            
            foreach (Area area in areas)
            {
                area.position = area.sphere.transform.position;
                FillMapUtils.SetChildHeight(area.sphere);
            }
            
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

            area.uniformRadius = area.data.radius * uniformScale;
            area.uniformStartRadius = area.data.startSize * uniformScale;
            
            area.CreateGrid();
            

            int[,] roads = RoadGenerator.GenerateRoadArea(area);

            FillArea.GenerateAreaContent(area, roads, uniformScale);
        }
        
        
        
        
    }

    public void GenerateRoadOnMap(MeshData meshData, float uniformScale)
    {
        
        while (roadParent.transform.childCount > 0)
        {
            Transform child = roadParent.transform.GetChild(0);
            DestroyImmediate(child.gameObject);
        }
        
        bool valid = false;

        Vector3 randVertex = meshData.borderVertices[Random.Range(0, mapGenerator.meshData.borderVertices.Length)]; 

        Vector3 randVertex2 = Vector3.zero;
        
        while (!valid)
        {
            randVertex2 = mapGenerator.meshData.borderVertices[Random.Range(0, mapGenerator.meshData.borderVertices.Length)];
            
            float distance = Vector3.Distance(randVertex, randVertex2);
            
            
            Renderer renderer = meshTerrain.GetComponent<Renderer>();
            
            Vector3 size = renderer.bounds.size;

            if (distance > size.x)
            {
                GameObject cube = GameObject.Instantiate(testCube, randVertex * uniformScale, Quaternion.identity);
                GameObject cube2 = GameObject.Instantiate(testCube, randVertex2 * uniformScale, Quaternion.identity);

                cube.transform.parent = roadParent.transform;
                cube2.transform.parent = roadParent.transform;
                
                valid = true;
            }
        }
        
        // TODO : Rely vertex1 to vertex2 with A* pathfinding
        
        // List<Vector3> neighbours = RoadGenerator.FindNeighbours(randVertex, 45, 10);
        //
        // foreach (Vector3 neighbour in neighbours)
        // {
        //     GameObject cube = GameObject.Instantiate(testCube, neighbour * uniformScale, Quaternion.identity);
        //     cube.transform.parent = roadParent.transform;
        // }
        
        
        RoadGenerator.FindPathWithAStar(randVertex, randVertex2, 45, 10, testCube, uniformScale, roadParent);
        
        
        
        
    }
}
