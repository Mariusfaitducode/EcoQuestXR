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
	public TextureData textureData;
	
	public Material terrainMaterial;
	
	// public FillMapArea fillMapArea;
	
	[Range(0,6)]
	public int editorPreviewLOD;
	public bool autoUpdate;
	float[,] falloffMap;

	public MeshData meshData;

	void Awake() {
		textureData.ApplyToMaterial(terrainMaterial);
		textureData.UpdateMeshHeights (terrainMaterial, terrainData.minHeight, terrainData.maxHeight);
	}
	
	void OnValuesUpdated() {
		if (!Application.isPlaying) {
			DrawMapInEditor ();
		}
	}
	
	void OnTextureValuesUpdated() {
		textureData.ApplyToMaterial (terrainMaterial);
	}

	public int mapChunkSize {
		get {
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
		textureData.UpdateMeshHeights (terrainMaterial, terrainData.minHeight, terrainData.maxHeight);
		
		// Generate map data : PerlinNoise map and colour map with all parameters
		MapData mapData = GenerateMapData (Vector2.zero);

		MapDisplay display = FindObjectOfType<MapDisplay> ();
		
		if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap (mapData.heightMap));
		} else if (drawMode == DrawMode.Mesh) {
			
			// Generate mesh data
			this.meshData = MeshGenerator.GenerateTerrainMesh (mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, editorPreviewLOD, terrainData.useFlatShading);
			
			// Display mesh data
			display.DrawMesh (meshData);
			
			// Place area on map
			// fillMapArea.PlaceAreaOnMap(meshData, terrainData.uniformScale);

		} else if (drawMode == DrawMode.FalloffMap) {
			display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunkSize, terrainData.falloffStart, terrainData.falloffEnd)));
		}
	}

	// Generate map data : PerlinNoise map and colour map with all parameters
	MapData GenerateMapData(Vector2 centre) {
		float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize + 2, mapChunkSize + 2, noiseData.seed, noiseData.noiseScale, noiseData.octaves, noiseData.persistance, noiseData.lacunarity, centre + noiseData.offset, noiseData.normalizeMode);

		if (terrainData.useFalloff)
		{
			// if (falloffMap == null)
			// {
			falloffMap = FalloffGenerator.GenerateFalloffMap (mapChunkSize+2, terrainData.falloffStart, terrainData.falloffEnd);
			// }
			for (int y = 0; y < mapChunkSize+2; y++) {
				for (int x = 0; x < mapChunkSize+2; x++) {
					if (terrainData.useFalloff) {
						noiseMap [x, y] = Mathf.Clamp01(noiseMap [x, y] - falloffMap [x, y]);
					}
				}
			}
		}

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
		if (textureData != null)
		{
			textureData.OnValuesUpdated -= OnTextureValuesUpdated;
			textureData.OnValuesUpdated += OnTextureValuesUpdated;
		}
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