using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public static class RoadGenerator
{
    [Serializable]
    public class RoadData
    {
        public float edgeLength = 10;
        public float angle = 45;
        public float roadScale = 3f;
        public float mountainGap = 0.2f;
        public float roadMinHeight = 0.1f;
        public float targetDistance = 20f;
        
        public Material roadMaterial = null;
        public Material testMaterial = null;
    }
    
    // private Random random = new Random();


    public static Vector3[] FindRoadExtremity(MeshData meshData, MapGenerator mapGenerator, GameObject meshTerrain, GameObject testCube, GameObject roadParent, float uniformScale, RoadData roadData)
    {
        bool valid = false;

        Vector3 randVertex = meshData.borderVertices[Random.Range(0, mapGenerator.meshData.borderVertices.Length)];
        Vector3 randVertex2 = Vector3.zero;
        
        Vector3[] listPoints = new Vector3[2];
        listPoints[0] = randVertex * uniformScale;

        int i = 0;

        while (!valid && i < 500)
        {
            randVertex2 = mapGenerator.meshData.borderVertices[Random.Range(0, mapGenerator.meshData.borderVertices.Length)];
            
            float distance = Vector3.Distance(randVertex, randVertex2);

            // Renderer meshRenderer = meshTerrain.GetComponent<MeshFilter>();
            
            Vector3 size = meshTerrain.GetComponent<MeshFilter>().sharedMesh.bounds.size * uniformScale;
            
            Debug.Log(size);
            
            

            if (distance > size.x)
            {
                FillMapUtils.InstantiateObjectWithScale(testCube, roadParent.transform, randVertex * uniformScale, Vector3.one * uniformScale * roadData.roadScale);
                GameObject cube = FillMapUtils.InstantiateObjectWithScale(testCube, roadParent.transform, randVertex2 * uniformScale, Vector3.one * uniformScale * roadData.roadScale);
                
                cube.GetComponent<Renderer>().material = roadData.testMaterial;
                
                listPoints[1] = randVertex2 * uniformScale;
                valid = true;
            }
            i++;
        }
        
        

        // Debug.Log(i);
        return listPoints;
    }


    public static Vector3[] ExtremityOnTerrain(Vector3[] listPoints, List<Area> areas, RoadData roadData, GameObject testCube, GameObject roadParent, float uniformScale)
    {
        List<FindPath.PathPoint> startList = new List<FindPath.PathPoint>();
        List<FindPath.PathPoint> endList = new List<FindPath.PathPoint>();
        
        startList.Add(new FindPath.PathPoint(listPoints[0], 0, null));
        endList.Add(new FindPath.PathPoint(listPoints[1], 0, null));

        for (int i = 0; i < 3; i++)
        {
            List<FindPath.PathPoint> copyStartList = new List<FindPath.PathPoint>(startList);
            foreach (FindPath.PathPoint point in copyStartList)
            {
                FindPath.FindNeighbours(startList, point,  areas, roadData, copyStartList);

            }

            List<FindPath.PathPoint> copyEndList = new List<FindPath.PathPoint>(endList);

            foreach (FindPath.PathPoint point in copyEndList)
            {
                FindPath.FindNeighbours(endList, point,  areas, roadData, copyEndList);
            }
        }
        Debug.Log(startList.Count);
        
        foreach (FindPath.PathPoint point in startList)
        {
            if (point.position.y > roadData.roadMinHeight)
            {
                listPoints[0] = point.position;
                break;
            }
        }
        foreach (FindPath.PathPoint point in endList)
        {
            if (point.position.y > roadData.roadMinHeight)
            {
                listPoints[1] = point.position;
                break;
            }
        }
        
        FillMapUtils.InstantiateObjectWithScale(testCube, roadParent.transform, listPoints[0] * uniformScale, Vector3.one * uniformScale * roadData.roadScale);
        GameObject cube = FillMapUtils.InstantiateObjectWithScale(testCube, roadParent.transform, listPoints[1] * uniformScale, Vector3.one * uniformScale * roadData.roadScale);
                
        cube.GetComponent<Renderer>().material = roadData.testMaterial;

        return listPoints;
    }
    
    
    public static void GenerateRoadArea(Area area)
    {
        int width = area.areaGrid.GetLength(0);
        int height = area.areaGrid.GetLength(1);
        
        
        // int[,] grid = new int[width, height];
        // Array.Copy(grid, newGrid, grid.Length);


        // Initialise avec des routes horizontales
        for (int i = 0; i < width; i += area.data.pavilionWidth + 1)
        {
            for (int j = 0; j < height; j++)
            {
                area.areaGrid[i, j].type = CellType.Road;
            }
        }
        
        // Génère des intersections verticales aléatoires
        for (int j = 1; j < height; j += area.data.pavilionWidth + 1)
        {

            int nextRoadCount = 0;

            int randomPavilionLength = FillMapUtils.GenerateGaussian((double)area.data.pavilionHeightMean);
            
            Debug.Log("Gaussian value : " + randomPavilionLength);

            nextRoadCount += randomPavilionLength;

            while (nextRoadCount < width)
            {
                for (int roadCount = 0; roadCount < area.data.pavilionWidth; roadCount++)
                {
                    if (j + roadCount < height && nextRoadCount < width)
                    {
                        area.areaGrid[ j + roadCount, nextRoadCount].type = CellType.Road;
                        
                    }
                }
                nextRoadCount += FillMapUtils.GenerateGaussian((double)area.data.pavilionHeightMean);
                
            }
        }
    }



    public static void GenerateRoadMesh(List<FindPath.PathPoint> pathPoints, GameObject roadObject, float roadWidth)
    {
        if (roadObject.GetComponent<MeshFilter>() == null)
        {
            roadObject.AddComponent<MeshFilter>();
        }

        Mesh mesh = new Mesh();
        roadObject.GetComponent<MeshFilter>().mesh = mesh;

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        
        Vector3 A = pathPoints[0].position;
        Vector3 B = pathPoints[1].position;
        Vector3 direction = (B - A).normalized;
        Vector3 perpendicular = Vector3.Cross(direction, Vector3.up) * roadWidth / 2;

        Vector3 p1 = A + perpendicular;
        Vector3 p2 = A - perpendicular;
        
        List<Vector3> endSegments = SetRoadSegment(pathPoints, vertices, triangles, 0, roadWidth, p1, p2);
        
        
        for (int i = 1; i < pathPoints.Count - 1; i++)
        {
            endSegments = SetRoadSegment(pathPoints, vertices, triangles, i, roadWidth, endSegments[0], endSegments[1]);

        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        roadObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }


    public static List<Vector3> SetRoadSegment(List<FindPath.PathPoint> pathPoints, List<Vector3> vertices, List<int> triangles, int index, float roadWidth, Vector3 p1, Vector3 p2)
    {
        Vector3 A = pathPoints[index].position;
        Vector3 B = pathPoints[index + 1].position;
        Vector3 direction = (B - A).normalized;
        Vector3 perpendicular = Vector3.Cross(direction, Vector3.up) * roadWidth / 2;

        // Vector3 p1 = A + perpendicular;
        // Vector3 p2 = A - perpendicular;
        Vector3 p3 = B + perpendicular;
        Vector3 p4 = B - perpendicular;

        p1.y = FillMapUtils.GetHeightFromRaycast(p1) + 0.1f;
        p2.y = FillMapUtils.GetHeightFromRaycast(p2) + 0.1f;
        p3.y = FillMapUtils.GetHeightFromRaycast(p3) + 0.1f;
        p4.y = FillMapUtils.GetHeightFromRaycast(p4) + 0.1f;


        vertices.Add(p1);
        vertices.Add(p2);
        vertices.Add(p3);
        vertices.Add(p4);

        int start = index * 4;
        triangles.Add(start);
        triangles.Add(start + 2);
        triangles.Add(start + 1);
        triangles.Add(start + 2);
        triangles.Add(start + 3);
        triangles.Add(start + 1);

        List<Vector3> endSegments = new List<Vector3>();
        endSegments.Add(p3);
        endSegments.Add(p4);

        return endSegments;
    }


    

    

    
    
}
