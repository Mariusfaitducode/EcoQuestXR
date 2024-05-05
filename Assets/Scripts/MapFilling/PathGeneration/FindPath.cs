using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindPath
{
    public class  PathPoint
    {
        public Vector3 position;
        // float weight;
        public int count;

        public PathPoint parent;
        
        public GameObject cube;
        
        public PathPoint(Vector3 position, int count, PathPoint parent)
        {
            this.position = position;
            // this.weight = weight;
            this.count = count;
            
            this.parent = parent;
        }
        
        
        public float GetWeight(Vector3 end)
        {
            return Vector3.Distance(position, end) + count;
        }
    }
    
    
    
    
    
    public static void FindPathWithAStar(List<Area> areas, Vector3 start, Vector3 end, RoadGenerator.RoadData roadData, GameObject testCube,  GameObject roadParent, float scale)
    {
        float angle = roadData.angle;
        float radius = roadData.edgeLength * scale;
        float roadScale = roadData.roadScale;
        
        int count = 0;
        
        List<PathPoint> exploredPoints = new List<PathPoint>();
        // List<Vector3> exploredVertices = new List<Vector3>();

        PathPoint newPoint = new PathPoint(start, 0, null);
        
        exploredPoints.Add(newPoint);
        // exploredVertices.Add(newPoint.position);

        List<PathPoint> neighbours = new List<PathPoint>(0);
        
        int i = 0;
        
        // A star search algorithm
        while (!Arrive(newPoint.position, end, roadData.targetDistance * scale) && i < 500 )
        {
            FindNeighbours(neighbours, newPoint, areas, roadData, exploredPoints);
            
            PathPoint nextPoint = ChooseNextPosition(neighbours, end, roadData, exploredPoints);
            
            exploredPoints.Add(nextPoint);
            
            if (nextPoint == null)
            {
                Debug.Log("No path found");
                Debug.Log(neighbours.Count);
                return;
            }
            // exploredVertices.Add(nextPoint.position);
            newPoint = nextPoint;
            
            nextPoint.cube = FillMapUtils.InstantiateObjectWithScale(testCube, roadParent.transform, nextPoint.position, Vector3.one * scale * roadScale);
            nextPoint.cube.GetComponent<Renderer>().material = roadData.testMaterial;
            
            i++;
        }

        // Go back
        
        PathPoint lastPoint = newPoint;
        
        count = lastPoint.count;
        
        while (lastPoint.parent != null)
        {
            if (count == lastPoint.count)
            {
                lastPoint.cube.GetComponent<Renderer>().material = roadData.roadMaterial;
            }

            count = lastPoint.count - 1;
            lastPoint = lastPoint.parent;
        }
    }
    
    
    public static void FindNeighbours(List<PathPoint> neighbours, PathPoint parent, List<Area> areas, RoadGenerator.RoadData roadData, List<PathPoint> exploredPoints)
    {
        // List<PathPoint> neighbours = new List<PathPoint>();

        float angleRad = 0;
        
        while (angleRad < 2 * Mathf.PI)
        {
            float x = parent.position.x + roadData.edgeLength * Mathf.Cos(angleRad);
            float z = parent.position.z + roadData.edgeLength * Mathf.Sin(angleRad);
            
            
            bool hit = FillMapUtils.IsHitFromRayCast(new Vector3(x, parent.position.y, z));

            if (hit)
            {
                float y = FillMapUtils.GetHeightFromRaycast(new Vector3(x, parent.position.y, z));
            
                bool notValid = ValidPointPosition(new Vector3(x, y, z), areas, roadData, parent, exploredPoints);

                if (!notValid)
                {
                    neighbours.Add(new PathPoint(new Vector3(x, y, z), parent.count + 1, parent));
                }
            }
            angleRad += Mathf.Deg2Rad * roadData.angle;
        }
    }


    public static bool ValidPointPosition(Vector3 newPosition, List<Area> areas, RoadGenerator.RoadData roadData, PathPoint parent, List<PathPoint> exploredPoints)
    {
        //Verify point validity

        bool notValid = false;

        // No area collision
        foreach (Area area in areas)
        {
            if (FillMapUtils.IsVertexInsideCircle(new Vector3(newPosition.x, newPosition.y, newPosition.z), area.sphere.transform.position, area.uniformRadius))
            {
                notValid = true;
            }
        }
            
        // No mountain collision

        if ((parent.position.y > roadData.roadMinHeight && newPosition.y > roadData.roadMinHeight) && Mathf.Abs(newPosition.y - parent.position.y) > roadData.mountainGap)
        {
            notValid = true;
        }
            
        // Not placed 
        foreach( PathPoint point in exploredPoints)
        {
            if (Vector3.Distance(point.position, new Vector3(newPosition.x, newPosition.y, newPosition.z)) < roadData.edgeLength * 0.8f)
            {
                notValid = true;
            }
        }
        
        return notValid;
    }
    
    
    public static PathPoint ChooseNextPosition(List<PathPoint> neighbours, Vector3 end, RoadGenerator.RoadData roadData, List<PathPoint> exploredPoints)
    {
        PathPoint nextPoint = null;
        
        float minWeight = float.MaxValue;
        
        foreach (PathPoint neighbour in neighbours)
        {
            float weight = neighbour.GetWeight(end);

            bool notValid = false;
            
            foreach( PathPoint point in exploredPoints)
            {
                if (Vector3.Distance(point.position, neighbour.position) < roadData.edgeLength * 0.8f)
                {
                    notValid = true;
                }
            }
            
            if (weight < minWeight && !notValid)
            {
                minWeight = weight;
                nextPoint = neighbour;
            }
        }
        neighbours.Remove(nextPoint);
        
        return nextPoint;
    }
    
    public static bool Arrive(Vector3 position, Vector3 end, float radius)
    {
        return Vector3.Distance(position, end) < radius;
    }
}
