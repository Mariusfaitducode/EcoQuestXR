using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FillMapManager : MonoBehaviour
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
    public MeshCollider meshCollider;
    
    public GameObject roadParent;
    
    public bool autoUpdate;
    
    public float minHeight = 0.1f;
    
    
    public int maxTries = 5;
    public int maxMapIteration = 200;

    public float prefabScale = 0.1f;
    
    
    public List<Area> areas;
    
    public RoadGenerator.RoadData roadData;
    public RiverGenerator.RiverSettings riverSettings;
    

    internal bool validPosition = false;


    public Vector3 SetMapScale()
    {
        mapGenerator.DrawMapInEditor();
        
        Vector3 meshScale = meshTerrain.transform.localScale;
        meshTerrain.transform.localScale = new Vector3(1, 1, 1);

        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = meshTerrain.GetComponent<MeshFilter>().sharedMesh;

        return meshScale;
    }
    

    public void SetAreaInEditor()
    {
        Vector3 meshScale = SetMapScale();
        
        if (mapGenerator.meshData != null)
        {
            PlaceAreaOnMap(mapGenerator.meshData, 1);
        }
        
        meshTerrain.transform.localScale = meshScale;
    }
    
    public void FillAreaInEditor()
    {
        Vector3 meshScale = SetMapScale();
        
        if (validPosition)
        {
            FillAreaOnMap(mapGenerator.meshData, 1);
        }
        meshTerrain.transform.localScale = meshScale;

    }
    
    public void SetAreaShaderInEditor()
    {
        // Vector3 meshScale = SetMapScale();
     
        if (validPosition)
        {
            SetAreaShader(mapGenerator.meshData);
        }
    }
    
    public void GenerateRoadOnMapInEditor()
    {
        Vector3 meshScale = SetMapScale();
        
        GenerateRoadOnMap(mapGenerator.meshData, 1);
        
        meshTerrain.transform.localScale = meshScale;

    }

    public void GenerateRiverInEditor()
    {
        Vector3 meshScale = SetMapScale();
        
        SetRiverShader(mapGenerator.meshData);

        meshTerrain.transform.localScale = meshScale;
 
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

            RoadGenerator.GenerateRoadArea(area);
            
            float mapSize = meshTerrain.GetComponent<MeshFilter>().sharedMesh.bounds.size.x * uniformScale;

            FillArea.GenerateAreaContent(area, prefabScale);
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
        
        foreach (Area area in areas)
        {
            while (area.roadParent.transform.childCount > 0)
            {
                Transform child = area.roadParent.transform.GetChild(0);
                DestroyImmediate(child.gameObject);
            }
        }
        
        Vector3[] extremityPoints = RoadGenerator.FindRoadExtremity(meshData, mapGenerator, meshTerrain, testCube, roadParent, uniformScale, roadData);

        List<FindPath.PathPoint> bigRoadPath = new List<FindPath.PathPoint>();

        if (extremityPoints.Length == 2)
        {
            Vector3[] validExtremityPoints = RoadGenerator.ExtremityOnTerrain(extremityPoints, areas, roadData,
                testCube, roadParent, uniformScale);
            
            Debug.Log(validExtremityPoints);
            
            bigRoadPath = FindPath.FindPathWithAStar(areas,validExtremityPoints[0] , validExtremityPoints[1] , roadData, testCube, roadParent, uniformScale);
        }
        
        List<List<FindPath.PathPoint>> listAreaRoads = new List<List<FindPath.PathPoint>>();
        List<FindPath.PathPoint> bigRoadCopy = new List<FindPath.PathPoint>(bigRoadPath);
        
        foreach (Area area in areas)
        {
            Vector3[] areaRoadExtremity = RoadGenerator.FindAreaClosestRoadCell(area, bigRoadCopy);
            
            FillMapUtils.InstantiateObjectWithScale(testCube, roadParent.transform, areaRoadExtremity[0], Vector3.one * uniformScale * roadData.roadScale);
            FillMapUtils.InstantiateObjectWithScale(testCube, roadParent.transform, areaRoadExtremity[1], Vector3.one * uniformScale * roadData.roadScale);
            
            List<FindPath.PathPoint> areaRoadPoints = FindPath.FindPathWithAStar(areas,areaRoadExtremity[0] , areaRoadExtremity[1] , roadData, testCube, area.roadParent, uniformScale, false);
            
            listAreaRoads.Add(areaRoadPoints);
            bigRoadCopy = bigRoadCopy.Concat(areaRoadPoints).ToList();
        }
        
        // Create Road Mesh
        RoadGenerator.GenerateRoadMesh(bigRoadPath, roadParent, roadData.roadWidth);

        int i = 0;
        foreach (List<FindPath.PathPoint> areaRoad in listAreaRoads)
        {
            RoadGenerator.GenerateRoadMesh(areaRoad, areas[i].roadParent, roadData.roadWidth);
            i++;
        }
    }
    
    

    public void SetRiverShader(MeshData meshData)
    {
        // Trouver le GameObject parent
        GameObject roadParent = GameObject.Find("RoadParent");
        
        // Récupérer tous les enfants du GameObject parent
        Transform[] transforms = roadParent.GetComponentsInChildren<Transform>();
        
        RiverGenerator.Generate(transforms, roadParent.transform, meshData, riverSettings.riverWidth);
        
        
        mapDisplay.DrawMesh(meshData);
    }

    
}