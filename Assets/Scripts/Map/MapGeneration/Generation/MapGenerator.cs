using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;
using MapGeneration.Generation;

public class MapGenerator : MonoBehaviour {

	public enum DrawMode {NoiseMap, Mesh, FalloffMap};
	public DrawMode drawMode;

	public TerrainData terrainData;
	public NoiseData noiseData;
	
	public UpdateTerrainRenderer updateTerrainRenderer;
	
	public Material terrainMaterial;
	// public TextureData textureData;
	// public FillMapArea fillMapArea;
	
	[Range(0,6)]
	public int editorPreviewLOD;
	public bool autoUpdate;
	float[,] falloffMap;

	public MeshData meshData;
	public MapData mapData;

	void Awake() {
		
		updateTerrainRenderer.UpdateMeshHeights(terrainData.minHeight, terrainData.maxHeight);
	}
	
	void OnValuesUpdated() {
		if (!Application.isPlaying) {
			DrawMapInEditor ();
		}
	}

	public int mapChunkSize {
		get {
			// Ces tailles sont choisies pour permettre une bonne subdivision du terrain en fonction du LOD et pour limiter la surcharge mémoire
			if (terrainData.useFlatShading) {
				return 95;
			} else {
				return 239;
			}
		}
	}

	// Function called when generate button clicked
	public void DrawMapInEditor() {
		
		// Texture update
		// textureData.UpdateMeshHeights (terrainMaterial, terrainData.minHeight, terrainData.maxHeight);
		updateTerrainRenderer.UpdateMeshHeights(terrainData.minHeight, terrainData.maxHeight);
		
		
		// Generate map data : PerlinNoise map and colour map with all parameters
		this.mapData = GenerateMapData (Vector2.zero);

		MapDisplay display = FindObjectOfType<MapDisplay> ();
		
		if (drawMode == DrawMode.NoiseMap) {
			// Draw noise map on a plane
			display.DrawTexture (TextureGenerator.TextureFromHeightMap (mapData.heightMap));
		}  
		else if (drawMode == DrawMode.FalloffMap) {
			// Draw falloff map on a plane
			display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunkSize, terrainData.falloffStart, terrainData.falloffEnd)));
		}
		else if (drawMode == DrawMode.Mesh) {

			// Generate mesh data
			this.meshData = MeshGenerator.GenerateTerrainMesh (mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, editorPreviewLOD, terrainData.useFlatShading);
			
			// Display mesh data
			display.DrawMesh (meshData);
		}
	}


	public void DrawMultiMapInEditor()
	{
		// Remove all children
		//Destroy all children to avoid duplicates
		while (transform.childCount > 0)
		{
			Transform child = transform.GetChild(0);
			DestroyImmediate(child.gameObject);
		}
		
		// Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
		// List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();
		
		// int chunksVisibleInViewDst = 1;
		
		int currentChunkCoordX = 0;
		int currentChunkCoordY = 0;
		
		// MapDisplay display = FindObjectOfType<MapDisplay> ();
		
		int worldChunkLarge = 3;
		
		float[,] allWorldNoiseMap = Noise.GenerateNoiseMap ((mapChunkSize + 2) * worldChunkLarge, (mapChunkSize + 2) * worldChunkLarge, noiseData.seed, noiseData.noiseScale, noiseData.octaves, noiseData.persistance, noiseData.lacunarity, noiseData.offset, noiseData.normalizeMode);

		for (int xOffset = 0; xOffset < worldChunkLarge; xOffset++) {
			for (int yOffset = 0; yOffset < worldChunkLarge; yOffset++) {
				
				Vector2 viewedChunkCoord = new Vector2 (currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
				
				TerrainChunk terrainChunk = new TerrainChunk(viewedChunkCoord, (mapChunkSize - 1), transform, terrainMaterial);
				
				// MapData terrainMapData = GenerateMapData(Vector2.zero);
				
				float[,] subMatrix = new float[mapChunkSize + 2, mapChunkSize + 2];

				int xVal = xOffset;
				int yVal = yOffset;
				
				if (yOffset == 2){ yVal = 0; }
				else if (yOffset == 0){ yVal = 2; }
				
				int startRow = xVal * (mapChunkSize + 2);
				int startCol = yVal * (mapChunkSize + 2);
				
				for (int x = 0; x < (mapChunkSize + 2); x++) // Lignes de la sous-matrice
				{
					for (int y = 0; y < (mapChunkSize + 2); y++) // Colonnes de la sous-matrice
					{
						subMatrix[x, y] = allWorldNoiseMap[startRow + x, startCol + y];
					}
				}
				
				
				
				MeshData terrainMeshData = MeshGenerator.GenerateTerrainMesh (subMatrix, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, editorPreviewLOD, terrainData.useFlatShading);

				terrainChunk.DrawMesh (terrainMeshData, terrainData.uniformScale);
			}
		}
	}

	// Generate map data : PerlinNoise map and colour map with all parameters
	MapData GenerateMapData(Vector2 centre) {
		float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize + 2, mapChunkSize + 2, noiseData.seed, noiseData.noiseScale, noiseData.octaves, noiseData.persistance, noiseData.lacunarity, centre + noiseData.offset, noiseData.normalizeMode);

		// if (terrainData.useFalloff)
		// {
		// 	// if (falloffMap == null)
		// 	// {
		// 	falloffMap = FalloffGenerator.GenerateFalloffMap (mapChunkSize+2, terrainData.falloffStart, terrainData.falloffEnd);
		// 	// }
		// 	for (int y = 0; y < mapChunkSize+2; y++) {
		// 		for (int x = 0; x < mapChunkSize+2; x++) {
		// 			if (terrainData.useFalloff) {
		// 				noiseMap [x, y] = Mathf.Clamp01(noiseMap [x, y] - falloffMap [x, y]);
		// 			}
		// 		}
		// 	}
		// }

		return new MapData (noiseMap);
	}

	// Update subscription
	void OnValidate() {

		if (terrainData != null)
		{
			terrainData.OnValuesUpdated -= OnValuesUpdated;
			terrainData.OnValuesUpdated += OnValuesUpdated;
		}
		if (noiseData != null)
		{
			noiseData.OnValuesUpdated -= OnValuesUpdated;
			noiseData.OnValuesUpdated += OnValuesUpdated;
		}
		// if (textureData != null)
		// {
		// 	textureData.OnValuesUpdated -= OnTextureValuesUpdated;
		// 	textureData.OnValuesUpdated += OnTextureValuesUpdated;
		// }
	}
}

[System.Serializable]


public struct MapData {
	public readonly float[,] heightMap;

	public MapData (float[,] heightMap)
	{
		this.heightMap = heightMap;
	}
	
}