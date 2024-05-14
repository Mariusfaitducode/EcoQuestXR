using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUtils 
{

    public struct PathEdge
    {
        public Vector2 point1;
        public Vector2 point2;
        public Vector3 line;
        
        public PathEdge(Vector2 point1, Vector2 point2, Vector3 line)
        {
            this.point1 = point1;
            this.point2 = point2;
            this.line = line;
        }
    }
    
    public struct PathVertices
    {
        public List<int> listIndex;
        public List<Vector3> listVertices;
    }


    public static PathVertices GetPathVertices(List<FindPath.PathPoint> pathPoints, MeshData meshData, float pathWidth )
    {
        
        
        List<Vector2> pathPositions = new List<Vector2>();

        foreach (FindPath.PathPoint pathPoint in pathPoints)
        {
            pathPositions.Add(new Vector2(pathPoint.position.x, pathPoint.position.z));
        }
        
        // Get lines from points
        
        List<Vector3> lines = new List<Vector3>();
        // Vector3[] lines = new Vector3[positionsArray.Length - 1];
        
        List<PathEdge> pathEdges = new List<PathEdge>();
        // PathEdge[] riverPieces = new RiverEdge[positionsArray.Length - 1];
        
        for (int i = 0; i < pathPositions.Count - 1; i++)
        {
            lines.Add(FillMapUtils.GetLineFrom2Points(pathPositions[i], pathPositions[i + 1]));
            
            pathEdges.Add(new PathEdge(pathPositions[i], pathPositions[i + 1], lines[i]));
        }
        
        
        // Get vertices
        
        PathVertices pathVertices = new PathVertices();
        List<Vector3> listVertices = new List<Vector3>();
        List<int> listIndex = new List<int>();
        
        Vector3[] vertices = meshData.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            foreach (PathEdge riverPiece in pathEdges)
            {
                Vector2 vertice2D = new Vector2(vertices[i].x, vertices[i].z);
                float distToLine = FillMapUtils.DistancePointToLine(vertice2D, riverPiece.line);
                // Check if vertices are close enough to the line
                if (distToLine < pathWidth / 2)
                {
                    float distToPoint1 = Vector2.Distance(vertice2D, riverPiece.point1);
                    float distToPoint2 = Vector2.Distance(vertice2D, riverPiece.point2);
                    float distPoint1ToPoint2 = Vector2.Distance(riverPiece.point1, riverPiece.point2);
                    float maxDistToPoints = Mathf.Sqrt(distPoint1ToPoint2 * distPoint1ToPoint2 +
                                                       (pathWidth / 2) * (pathWidth / 2));
                    // Debug.DrawLine(
                    //     new Vector3(vertice2D.x, 10, vertice2D.y),
                    //     new Vector3(vertice2D.x, 25, vertice2D.y),
                    //     Color.red,
                    //     100);
                    if (distToPoint1 < maxDistToPoints && distToPoint2 < maxDistToPoints)
                    {
                        // isRiver[i] = true;

                        listVertices.Add(vertices[i]);
                        listIndex.Add(i);
                    }
                }
            }
        }
        
        pathVertices.listVertices = listVertices;
        pathVertices.listIndex = listIndex;

        return pathVertices;
    }

    public static void ResetPathHeight(List<FindPath.PathPoint> pathPoints, float meanHeight)
    {
        foreach (FindPath.PathPoint pathPoint in pathPoints)
        {
            pathPoint.position = new Vector3(pathPoint.position.x, meanHeight, pathPoint.position.z);
        }
    }
    
    
    public static float MeanWithoutTooLowVertices(List<Vector3> vertices, float minHeight)
    {
        float sum = 0;
        int count = 0;
        foreach (Vector3 vertex in vertices)
        {
            if (vertex.y > minHeight)
            {
                sum += vertex.y;
                count++;
            }
        }

        return sum / count;
    }
}
