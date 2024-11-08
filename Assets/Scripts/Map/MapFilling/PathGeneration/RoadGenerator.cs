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
        public float roadWidth = 2f;
        
        public RoadTiles roadTiles;
        
        public Material roadMaterial;
    }

    [Serializable]
    public struct RoadTiles
    {
        public GameObject road1;
        public GameObject road2;
        public GameObject road3;
        public GameObject road4;
    }
    
    public struct RoadTile
    {
        public GameObject tile;
        public Quaternion rotation;
        
        public RoadTile(GameObject tile, Quaternion rotation)
        {
            this.tile = tile;
            this.rotation = rotation;
        }
    }
    
    // private Random random = new Random();

    // Big road extremity
    public static Vector3[] FindRoadExtremity(MeshData meshData, MapGenerator mapGenerator, GameObject meshTerrain, GameObject roadParent, RoadData roadData)
    {
        bool valid = false;

        Vector3 randVertex = meshData.borderVertices[Random.Range(0, mapGenerator.meshData.borderVertices.Length)];
        Vector3 randVertex2 = Vector3.zero;
        
        Vector3[] listPoints = new Vector3[2];
        listPoints[0] = randVertex;

        int i = 0;

        while (!valid && i < 500)
        {
            randVertex2 = mapGenerator.meshData.borderVertices[Random.Range(0, mapGenerator.meshData.borderVertices.Length)];
            
            float distance = Vector3.Distance(randVertex, randVertex2);

            // Renderer meshRenderer = meshTerrain.GetComponent<MeshFilter>();
            
            Vector3 size = meshTerrain.GetComponent<MeshFilter>().sharedMesh.bounds.size;
            
            // Debug.Log(size);
            
            

            if (distance > size.x)
            {
                // FillMapUtils.InstantiateObjectWithScale(testCube, roadParent.transform, randVertex, Quaternion.identity, Vector3.one * roadData.roadScale);
                // GameObject cube = FillMapUtils.InstantiateObjectWithScale(testCube, roadParent.transform, randVertex2, Quaternion.identity, Vector3.one * roadData.roadScale);
                //
                // cube.GetComponent<Renderer>().material = roadData.testMaterial;
                
                listPoints[1] = randVertex2;
                valid = true;
            }
            i++;
        }
        
        

        // Debug.Log(i);
        return listPoints;
    }
    
    public static Vector3[] ExtremityOnTerrain(Vector3[] listPoints, List<Area> areas, RoadData roadData, GameObject roadParent)
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
                FindPath.FindNeighbours(startList, point,  areas, roadData, copyStartList, true);

            }

            List<FindPath.PathPoint> copyEndList = new List<FindPath.PathPoint>(endList);

            foreach (FindPath.PathPoint point in copyEndList)
            {
                FindPath.FindNeighbours(endList, point,  areas, roadData, copyEndList, true);
            }
        }
        // Debug.Log(startList.Count);
        
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
        
        // FillMapUtils.InstantiateObjectWithScale(testCube, roadParent.transform, listPoints[0],  Quaternion.identity, Vector3.one * roadData.roadScale);
        // GameObject cube = FillMapUtils.InstantiateObjectWithScale(testCube, roadParent.transform, listPoints[1], Quaternion.identity,  Vector3.one * roadData.roadScale);
        //         
        // cube.GetComponent<Renderer>().material = roadData.testMaterial;

        return listPoints;
    }

    // Road area
    public static void GenerateRoadsOnGrid(Area area)
    {
        int width = area.areaGrid.GetLength(0);
        int height = area.areaGrid.GetLength(1);

        // Initialize horizontal roads
        for (int i = 0; i < width; i += area.data.pavilionWidth + 1)
        {
            for (int j = 0; j < height; j++)
            {
                area.areaGrid[i, j].type = CellType.Road;
            }
        }
        
        // Generate random vertical roads
        for (int j = 1; j < height; j += area.data.pavilionWidth + 1)
        {

            int nextRoadCount = 0;

            int randomPavilionLength = FillMapUtils.GenerateGaussian((double)area.data.pavilionHeightMean);

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


    public static Vector3[] FindAreaClosestRoadCell(Area area, List<FindPath.PathPoint> pathPoints)
    {
        int width = area.areaGrid.GetLength(0);
        int height = area.areaGrid.GetLength(1);
        
        float minDistance = float.MaxValue;
        FindPath.PathPoint closestPoint = null;

        // Search big road closest point
        foreach (FindPath.PathPoint point in pathPoints)
        {
            float distance = Vector3.Distance(point.position, area.sphere.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPoint = point;
            }
        }

        // Search closest area road cell
        FindPath.PathPoint areaPoint = null;

        if (closestPoint != null)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (area.areaGrid[i, j].type == CellType.Road)
                    {
                        float distance = Vector3.Distance(area.areaGrid[i, j].cellPosition.transform.position, closestPoint.position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            areaPoint = new FindPath.PathPoint(area.areaGrid[i, j].cellPosition.transform.position, 0, null);
                        }
                    }
                }
            }
        }
        if (areaPoint != null)
        {
            Vector3[] areaPoints = new Vector3[2];
            areaPoints[0] = closestPoint.position;
            areaPoints[1] = areaPoint.position;
            return areaPoints;
        }

        return null;
    }
    

    // Road mesh
    public static void GenerateRoadMesh(List<FindPath.PathPoint> pathPoints, GameObject roadObject, float roadWidth)
    {
        if (roadObject.GetComponent<MeshFilter>() == null)
        {
            roadObject.AddComponent<MeshFilter>();
        }
        if (roadObject.GetComponent<MeshRenderer>() == null)
        {
            roadObject.AddComponent<MeshRenderer>();
        }
        if (roadObject.GetComponent<MeshCollider>() == null)
        {
            roadObject.AddComponent<MeshCollider>();
        }

        Mesh mesh = new Mesh();
        roadObject.GetComponent<MeshFilter>().mesh = null;
        roadObject.GetComponent<MeshFilter>().mesh = mesh;

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        
        if (pathPoints.Count < 2)
        {
            return;
        }
        
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
        Vector3 current = pathPoints[index].position;
        Vector3 next = pathPoints[index + 1].position;
        Vector3 segmentDirection = (next - current).normalized;
        Vector3 perpendicular;

        if (index < pathPoints.Count - 2)
        {
            Vector3 nextSegmentDirection = (pathPoints[index + 2].position - next).normalized;
            perpendicular = FillMapUtils.GetPerpendicularDirection(segmentDirection, nextSegmentDirection) * roadWidth / 2;
        }
        else
        {
            perpendicular = Vector3.Cross(segmentDirection, Vector3.up) * roadWidth / 2;
        }

        // Vector3 p1 = A + perpendicular;
        // Vector3 p2 = A - perpendicular;
        Vector3 p3 = next + perpendicular;
        Vector3 p4 = next - perpendicular;

        p1.y = FillMapUtils.GetHeightFromRaycast(p1) + 0.05f;
        p2.y = FillMapUtils.GetHeightFromRaycast(p2) + 0.05f;
        p3.y = FillMapUtils.GetHeightFromRaycast(p3) + 0.05f;
        p4.y = FillMapUtils.GetHeightFromRaycast(p4) + 0.05f;


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

    

    public static RoadTile FindGoodRoadTile(bool up, bool right, bool down, bool left, RoadTiles roadTiles)
    {
        int count = (up ? 1 : 0) + (right ? 1 : 0) + (down ? 1 : 0) + (left ? 1 : 0);

        switch (count)
        {
            case 1:  // Cul-de-sac
                if (up) return new RoadTile(roadTiles.road1, Quaternion.identity);
                if (right) return new RoadTile(roadTiles.road1, Quaternion.Euler(0, 90, 0));
                if (down) return new RoadTile(roadTiles.road1, Quaternion.Euler(0, 180, 0));
                if (left) return new RoadTile(roadTiles.road1, Quaternion.Euler(0, 270, 0));
                break;

            case 2:  // Ligne droite ou virage
                if (up && down) return new RoadTile(roadTiles.road1, Quaternion.identity);
                if (left && right) return new RoadTile(roadTiles.road1, Quaternion.Euler(0, 90, 0));
                
                if (up && right) return new RoadTile(roadTiles.road2, Quaternion.Euler(0, 90, 0));
                if (right && down) return new RoadTile(roadTiles.road2, Quaternion.Euler(0, 270, 0));
                if (down && left) return new RoadTile(roadTiles.road2, Quaternion.Euler(0, 180, 0) );
                if (left && up) return new RoadTile(roadTiles.road2, Quaternion.identity);
                break;

            case 3:  // T intersection
                if (!left) return new RoadTile(roadTiles.road3,Quaternion.Euler(0, 180, 0));
                if (!up) return new RoadTile(roadTiles.road3, Quaternion.Euler(0, 270, 0));
                if (!right) return new RoadTile(roadTiles.road3, Quaternion.identity);
                if (!down) return new RoadTile(roadTiles.road3, Quaternion.Euler(0, 90, 0));
                break;

            case 4:  // Croisement à 4 sorties
                return new RoadTile(roadTiles.road4, Quaternion.identity);

            default:
                return new RoadTile(null, Quaternion.identity);
        }
        return new RoadTile(null, Quaternion.identity);
    }
    

    

    
    
}
