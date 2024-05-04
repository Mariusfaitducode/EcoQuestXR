using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RiverGeneration
{
    public struct RiverPiece
    {
        public Vector2 Point1;
        public Vector2 Point2;
        public Vector3 Line;
    }
    public static void Generate(Transform[] transforms, Transform roadParentTransform, MeshData meshData, float riverWidth)
    { 
        // Only get positions
        List<Vector2> positions = new List<Vector2>();
        foreach (Transform transform in transforms)
        {
            // Skip the parent's transform
            if (transform == roadParentTransform)
                continue;
            
            // Add the position of the child to the list
            positions.Add(new Vector2(transform.position.x, transform.position.z));
        }
        
        // Draw positions
        // foreach (Vector2 position in positions)
        // {
        //     Debug.DrawLine(
        //         new Vector3(position.x, 10, position.y),
        //         new Vector3(position.x, 25, position.y),
        //         Color.red,
        //         100);
        // }
        
        // Convert the list of positions to an array
        Vector2[] positionsArray = positions.ToArray();
        
        // Get lines from points
        Vector3[] lines = new Vector3[positionsArray.Length - 1];
        RiverPiece[] riverPieces = new RiverPiece[positionsArray.Length - 1];
        for (int idx = 0; idx < positionsArray.Length - 2; idx++)
        {
            lines[idx] = UtilsGeneration.GetLineFrom2Points(positions[idx], positionsArray[idx + 1]);
            riverPieces[idx].Point1 = positions[idx];
            riverPieces[idx].Point2 = positions[idx + 1];
            riverPieces[idx].Line = lines[idx];
        }
        
        // Get vertices
        Vector3[] vertices = meshData.vertices;
        
        // Check if vertices are close enough to the centerline of the river
        for (int i = 0; i < vertices.Length; i++)
        {
            foreach (RiverPiece riverPiece in riverPieces)
            {
                Vector2 vertice2D = new Vector2(vertices[i].x, vertices[i].z);
                float distToLine = UtilsGeneration.DistancePointToLine(vertice2D, riverPiece.Line);
                // Check if vertices are close enough to the line
                if (distToLine < riverWidth / 2)
                {
                    float distToPoint1 = Vector2.Distance(vertice2D, riverPiece.Point1);
                    float distToPoint2 = Vector2.Distance(vertice2D, riverPiece.Point2);
                    float distPoint1ToPoint2 = Vector2.Distance(riverPiece.Point1, riverPiece.Point2);
                    float maxDistToPoints = (float)Math.Sqrt(distPoint1ToPoint2 * distPoint1ToPoint2 +
                                                             (riverWidth / 2) * (riverWidth / 2));
                    if (distToPoint1 < maxDistToPoints || distToPoint2 < maxDistToPoints)
                    {
                        meshData.uvs[i] = new Vector2(7, 0);
                        Debug.DrawLine(
                            new Vector3(vertice2D.x, 10, vertice2D.y),
                            new Vector3(vertice2D.x, 25, vertice2D.y),
                            Color.red,
                            100);
                    }
                }
            }
        }
    }
}
