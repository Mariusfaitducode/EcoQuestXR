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
    
    
    // public GameObject testCube;
    
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
    public float cellSize = 1f;
    
    
    public List<Area> areas;
    
    public RoadGenerator.RoadData roadData;

    public NatureGenerator.NatureData natureData;
    
    public RiverGenerator.RiverSettings riverSettings;
    

    internal bool validPosition = false;
    
    internal List<Vector3> roadVertices = new List<Vector3>();


    public Vector3 SetMapScale()
    {
        // mapGenerator.DrawMapInEditor();
        
        Vector3 meshScale = meshTerrain.transform.localScale;
        meshTerrain.transform.localScale = new Vector3(1, 1, 1);

        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = meshTerrain.GetComponent<MeshFilter>().sharedMesh;

        return meshScale;
    }
    
    
    public void GenerateMap()
    {
        mapGenerator.DrawMapInEditor();
        
        SetAreaInEditor();
        FillAreaInEditor();
        GenerateRoadOnMapInEditor();
        // GenerateNatureInEditor();
    }
    

    // Editor call
    public void SetAreaInEditor()
    {
        Vector3 meshScale = SetMapScale();
        
        if (mapGenerator.meshData != null)
        {
            PlaceAreaOnMap(mapGenerator.meshData);
        }
        
        meshTerrain.transform.localScale = meshScale;
    }
    
    public void FillAreaInEditor()
    {
        SetAreaShader(mapGenerator.meshData, mapGenerator.terrainData.uniformScale);
        Vector3 meshScale = SetMapScale();
        
        if (validPosition)
        {
            FillAreaOnMap(mapGenerator.meshData, mapGenerator.terrainData.uniformScale);
            
        }
        meshTerrain.transform.localScale = meshScale;
    }

    public void GenerateRoadOnMapInEditor()
    {
        Vector3 meshScale = SetMapScale();
        
        GenerateRoadOnMap(mapGenerator.meshData);
        
        meshTerrain.transform.localScale = meshScale;

    }
    
    public void GenerateNatureInEditor()
    {
        Vector3 meshScale = SetMapScale();
        
        // GenerateRoadOnMap(mapGenerator.meshData);
        GenerateNatureOnMap(mapGenerator.meshData);
        
        meshTerrain.transform.localScale = meshScale;

    }

    public void GenerateRiverInEditor()
    {
        Vector3 meshScale = SetMapScale();
        
        SetRiverShader(mapGenerator.meshData);

        meshTerrain.transform.localScale = meshScale;
 
    }
    
    
    // Map Modification
    
    public void PlaceAreaOnMap(MeshData meshData)
    {
        // int maxTries = this.maxTries;
        this.validPosition = SetAreaPosition.FindAreaPosition(areas, meshData,  minHeight,  this.maxTries, maxMapIteration);
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
            //Destroy all children to avoid duplicates
            while (area.sphere.transform.childCount > 0)
            {
                Transform child = area.sphere.transform.GetChild(0);
                DestroyImmediate(child.gameObject);
            }

            // Create sub folders to structure the hierarchy
            area.hierarchyBuildingFolder = new GameObject("Buildings");
            area.hierarchyBuildingFolder.transform.parent = area.sphere.transform;
            
            area.hierarchyRoadFolder = new GameObject("Roads");
            area.hierarchyRoadFolder.transform.parent = area.sphere.transform;

            // Create virtual grid
            area.CreateGrid(cellSize);
            
            // Place road with random pattern on grid
            RoadGenerator.GenerateRoadsOnGrid(area);
            
            // Fill area with roads and buildings
            FillArea.InstantiateRoadsAndBuildingsOnArea(area, prefabScale, roadData, mapDisplay);
            
        }
        
        Debug.Log("Filled Areas Successfully");

    }
    
    public void SetAreaShader(MeshData meshData, float uniformScale)
    {
        foreach (Area area in areas)
        {
            // FillArea.SetAreaShader(area, meshTerrain, uniformScale);
            
            mapGenerator.updateTerrainRenderer.SetAreaShader(area);
            
            FillArea.SetAreaVerticesInformation(area, meshData, uniformScale);
            
            Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            area.sphere.transform.rotation = rotation;
            
        }
        // Debug.Log(meshData);

        mapDisplay.DrawMesh(meshData);
    }
    
    public void GenerateRoadOnMap(MeshData meshData)
    {
        
        // Trace Roads
        Vector3[] extremityPoints = RoadGenerator.FindRoadExtremity(meshData, mapGenerator, meshTerrain, roadParent, roadData);

        List<FindPath.PathPoint> bigRoadPath = new List<FindPath.PathPoint>();

        if (extremityPoints.Length == 2)
        {
            Vector3[] validExtremityPoints = RoadGenerator.ExtremityOnTerrain(extremityPoints, areas, roadData, roadParent);
            
            // Debug.Log(validExtremityPoints);
            
            bigRoadPath = FindPath.FindPathWithAStar(areas,validExtremityPoints[0] , validExtremityPoints[1] , roadData,  roadParent);
        }
        
        List<List<FindPath.PathPoint>> listAreaRoads = new List<List<FindPath.PathPoint>>();
        List<FindPath.PathPoint> bigRoadCopy = new List<FindPath.PathPoint>(bigRoadPath);
        
        foreach (Area area in areas)
        {
            Vector3[] areaRoadExtremity = RoadGenerator.FindAreaClosestRoadCell(area, bigRoadCopy);
            
            // FillMapUtils.InstantiateObjectWithScale(testCube, roadParent.transform, areaRoadExtremity[0], Quaternion.identity, Vector3.one * roadData.roadScale);
            // FillMapUtils.InstantiateObjectWithScale(testCube, roadParent.transform, areaRoadExtremity[1], Quaternion.identity, Vector3.one * roadData.roadScale);
            
            List<FindPath.PathPoint> areaRoadPoints = FindPath.FindPathWithAStar(areas,areaRoadExtremity[0] , areaRoadExtremity[1] , roadData,  area.roadParent, false);
            
            listAreaRoads.Add(areaRoadPoints);
            bigRoadCopy = bigRoadCopy.Concat(areaRoadPoints).ToList();
        }
        
        // Get Path Vertices
        
        PathUtils.PathVertices pathVertices = PathUtils.GetPathVertices(bigRoadPath, meshData, roadData.roadWidth + 6);
        List<PathUtils.PathVertices> listAreaVertices = new List<PathUtils.PathVertices>();
        
        foreach (List<FindPath.PathPoint> areaRoad in listAreaRoads)
        {
            PathUtils.PathVertices areaPath = PathUtils.GetPathVertices(areaRoad, meshData, roadData.roadWidth + 6);
            listAreaVertices.Add(areaPath);
            
            pathVertices.listVertices = pathVertices.listVertices.Concat(areaPath.listVertices).ToList();
            pathVertices.listIndex = pathVertices.listIndex.Concat(areaPath.listIndex).ToList();
        }
        
        // Set Path Height and vertices height

        float testMean = FillMapUtils.CalculateMean(pathVertices.listVertices);
        float mean = PathUtils.MeanWithoutTooLowVertices(pathVertices.listVertices, minHeight);
        
        // Debug.Log("Mean diff : " + testMean+ " vs " + mean);
        
        foreach(int index in pathVertices.listIndex)
        {
            // Debug.DrawLine(meshData.vertices[index] *uniformScale, new Vector3(meshData.vertices[index].x, mean, meshData.vertices[index].z) * uniformScale, Color.blue, 60);
            meshData.vertices[index] = new Vector3(meshData.vertices[index].x, mean, meshData.vertices[index].z);
        }

        mapDisplay.DrawMesh(meshData);
        
        PathUtils.ResetPathHeight(bigRoadPath, mean);
        
        foreach( List<FindPath.PathPoint> areaRoad in listAreaRoads)
        {
            PathUtils.ResetPathHeight(areaRoad, mean);
        }
        
        this.roadVertices = pathVertices.listVertices;
        
        // Create Road Mesh
        RoadGenerator.GenerateRoadMesh(bigRoadPath, roadParent, roadData.roadWidth);
        // PathUtils.GetPathVertices(bigRoadPath, meshData, roadData.roadWidth);

        int i = 0;
        foreach (List<FindPath.PathPoint> areaRoad in listAreaRoads)
        {
            RoadGenerator.GenerateRoadMesh(areaRoad, areas[i].roadParent, roadData.roadWidth);
            // PathUtils.GetPathVertices(areaRoad, meshData, roadData.roadWidth);
            i++;
        }
        
        Debug.Log("Roads Generated Successfully");
    }
    
    public void GenerateNatureOnMap(MeshData meshData)
    {
        //Destroy all children
        while (natureData.natureParent.transform.childCount > 0)
        {
            Transform child = natureData.natureParent.transform.GetChild(0);
            DestroyImmediate(child.gameObject);
        }
        
        // foreach (NatureGenerator.ForestData forest in natureData.forestDatas)
        // {
        //     while (forest.forestParent.transform.childCount > 0)
        //     {
        //         Transform child = forest.forestParent.transform.GetChild(0);
        //         DestroyImmediate(child.gameObject);
        //     }
        // }
        NatureGenerator.GenerateNature(areas, natureData, mapGenerator, mapDisplay, minHeight, prefabScale, roadVertices);
        
        Debug.Log("Nature Generated Successfully");
    }
    
    public void SetRiverShader(MeshData meshData)
    {
        // Trouver le GameObject parent
        GameObject roadParent = GameObject.Find("BigRoadParent");
        
        // Récupérer tous les enfants du GameObject parent
        Transform[] transforms = roadParent.GetComponentsInChildren<Transform>();
        
        RiverGenerator.Generate(transforms, roadParent.transform, meshData, riverSettings.riverWidth);
        
        
        mapDisplay.DrawMesh(meshData);
        
        
    }


    public void TestButton()
    {
        List<ObjectProperties> objectsProperties = ObjectsInitialization.InitializeObjectsProperties("Csv/objects");
        
        ObjectProperties objectProp = objectsProperties.Find(o => o.areaType == AreaType.City);
        
        // ObjectGestion.PlaceObjectsOnMap(objectProp, 5, areas, prefabScale, null, mapGenerator.terrainData.uniformScale);
        
        ObjectGestion.RemoveObjectOnMap(objectProp, 4, areas);
    }

    
}
