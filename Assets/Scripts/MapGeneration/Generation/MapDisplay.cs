using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRender;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public MeshCollider meshCollider;
    public void DrawTexture(Texture2D texture){
        
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3 (texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData)
    {
        // Debug.Log("DrawMesh called");
        
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshCollider.sharedMesh = meshFilter.sharedMesh;
        
        meshFilter.transform.localScale = Vector3.one * FindObjectOfType<MapGenerator>().terrainData.uniformScale;
        
        // Get 57 121 vertices (239 * 239) for LOD 0
        // Debug.Log(meshFilter.sharedMesh.vertices.Length);  
        
    }
}
