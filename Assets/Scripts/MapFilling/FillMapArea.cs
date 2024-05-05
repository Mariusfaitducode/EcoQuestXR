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
    
    public MapDisplay mapDisplay;

    public GameObject meshTerrain;
    
    public GameObject roadParent;
    
    public bool autoUpdate;
    
    public float minHeight = 0.1f;
    
    
    public int maxTries = 5;
    public int maxMapIteration = 200;
    
    
    
    public List<Area> areas;
    
    public RoadGenerator.RoadData roadData;
    
    

    
    
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
    
    public void SetAreaShaderInEditor()
    {
        mapGenerator.DrawMapInEditor();
        if (validPosition)
        {
            SetAreaShader(mapGenerator.meshData);
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
            if (area.data.type != AreaType.City)
            {
                break;
            }
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
            
            float mapSize = meshTerrain.GetComponent<MeshFilter>().sharedMesh.bounds.size.x * uniformScale;

            FillArea.GenerateAreaContent(area, roads, mapSize, uniformScale);
        }
    }


    public void SetAreaShader(MeshData meshData)
    {
        foreach (Area area in areas)
        {
            FillArea.SetAreaVerticesInformation(area, meshData);
            
        }
        Debug.Log(meshData);

        mapDisplay.DrawMesh(meshData);


    }

    public void GenerateRoadOnMap(MeshData meshData, float uniformScale)
    {

        // float roadScale = 3f;
        
        while (roadParent.transform.childCount > 0)
        {
            Transform child = roadParent.transform.GetChild(0);
            DestroyImmediate(child.gameObject);
        }
        
        Vector3[] extremityPoints = RoadGenerator.FindRoadExtremity(meshData, mapGenerator, meshTerrain, testCube, roadParent, uniformScale, roadData);


        if (extremityPoints.Length == 2)
        {
            Vector3[] validExtremityPoints = RoadGenerator.ExtremityOnTerrain(extremityPoints, areas, roadData);
            
            FindPath.FindPathWithAStar(areas,validExtremityPoints[0] , validExtremityPoints[1] , roadData, testCube, roadParent, uniformScale);

        }
        
        
        
        
        
    }
}
