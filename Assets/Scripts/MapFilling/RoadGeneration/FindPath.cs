using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPath
{
    public struct  PathPoint
    {
        public Vector3 position;
        // float weight;
        public int count;
        
        public PathPoint(Vector3 position,  int count)
        {
            this.position = position;
            // this.weight = weight;
            this.count = count;
        }
        
        public float GetWeight(Vector3 end)
        {
            return Vector3.Distance(position, end) + count;
        }
    }
    
    
    public static List<Vector3> FindNeighbours(Vector3 position, float angle, float radius, List<Area> areas)
    {
        List<Vector3> neighbours = new List<Vector3>();

        float angleRad = 0;
        
        while (angleRad < 2 * Mathf.PI)
        {
            float x = position.x + radius * Mathf.Cos(angleRad);
            float z = position.z + radius * Mathf.Sin(angleRad);
            
            float y = FillMapUtils.GetHeightFromRaycast(new Vector3(x, position.y, z));
            
            //Verify point validity

            bool insideCircle = false;

            foreach (Area area in areas)
            {
                if (FillMapUtils.IsVertexInsideCircle(new Vector3(x, y, z), area.sphere.transform.position, area.uniformRadius))
                {
                    insideCircle = true;
                }
            }

            if (!insideCircle)
            {
                neighbours.Add(new Vector3(x, y, z));
            }
            
            
            angleRad += Mathf.Deg2Rad * angle;
        }
        
        
        return neighbours;
    }
    
    
    public static void FindPathWithAStar(List<Area> areas, Vector3 start, Vector3 end, float angle, float radius, GameObject testCube,  GameObject roadParent, float scale, float roadScale)
    {
        int count = 0;
        
        List<PathPoint> exploredVertices = new List<PathPoint>();

        // Vector3 newPosition = start;
        
        PathPoint newPoint = new PathPoint(start, 0);
        exploredVertices.Add(newPoint);
        
        int i = 0;
        
        while (!Arrive(newPoint.position, end, 20 * scale) && i < 200 )
        {
            List<Vector3> neighbours = FindNeighbours(newPoint.position, angle, radius, areas);
            
            PathPoint nextPoint = ChooseNextPosition(neighbours, end, newPoint);
            
            exploredVertices.Add(nextPoint);
            newPoint = nextPoint;
            
            FillMapUtils.InstantiateObjectWithScale(testCube, roadParent.transform, newPoint.position, Vector3.one * scale * roadScale);
            
            // GameObject cube = GameObject.Instantiate(testCube, newPoint.position * scale, Quaternion.identity);
            // cube.transform.parent = roadParent.transform;
            i++;
        }
    }
    
    public static PathPoint ChooseNextPosition(List<Vector3> neighbours, Vector3 end, PathPoint lastPoint)
    {
        PathPoint nextPoint = new PathPoint();
        
        Vector3 nextPosition = Vector3.zero;
        
        float minWeight = float.MaxValue;
        
        foreach (Vector3 neighbour in neighbours)
        {
            PathPoint point = new PathPoint(neighbour, lastPoint.count + 1);
            
            float weight = point.GetWeight(end);
            
            if (weight < minWeight)
            {
                minWeight = weight;
                nextPoint = point;
                nextPosition = neighbour;
            }
        }
        neighbours.Remove(nextPosition);
        
        return nextPoint;
    }
    
    public static bool Arrive(Vector3 position, Vector3 end, float radius)
    {
        return Vector3.Distance(position, end) < radius;
    }
}
