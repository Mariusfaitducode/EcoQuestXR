using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainChunk {

	GameObject meshObject;
	public Vector2 position;
	Bounds bounds;

	MeshRenderer meshRenderer;
	MeshFilter meshFilter;


	public TerrainChunk(Vector2 coord, int size, Transform parent, Material material) {
		position = coord * size;
		bounds = new Bounds(position,Vector2.one * size);
		Vector3 positionV3 = new Vector3(position.x,0,position.y);

		meshObject = new GameObject("Terrain Chunk");
		meshRenderer = meshObject.AddComponent<MeshRenderer>();
		meshFilter = meshObject.AddComponent<MeshFilter>();
		meshRenderer.material = material;

		meshObject.transform.position = positionV3;
		meshObject.transform.parent = parent;
		meshObject.SetActive (true);

		// mapGenerator.RequestMapData(OnMapDataReceived);
	}

	
	public void DrawMesh(MeshData meshData, float uniformScale)
	{
		// Debug.Log("DrawMesh called");
        
		meshFilter.sharedMesh = meshData.CreateMesh();
		// meshCollider.sharedMesh = meshFilter.sharedMesh;
        
		meshFilter.transform.localScale = Vector3.one * uniformScale;
        
		// Get 57 121 vertices (239 * 239) for LOD 0
		// Debug.Log(meshFilter.sharedMesh.vertices.Length);  
        
	}

}
