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

	// Ajoutez ce dictionnaire comme variable de classe
	private Dictionary<Vector2, List<(Vector3 worldPosition, MeshData mesh, int borderIndex)>> borderVerticesMap = new Dictionary<Vector2, List<(Vector3 worldPosition, MeshData mesh, int borderIndex)>>();

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
		
		float[,] allWorldNoiseMap = Noise.GenerateNoiseMap(
			(mapChunkSize + 2) * worldChunkLarge, 
			(mapChunkSize + 2) * worldChunkLarge, 
			noiseData.seed, 
			noiseData.noiseScale, 
			noiseData.octaves, 
			noiseData.persistance, 
			noiseData.lacunarity, 
			noiseData.offset, 
			noiseData.normalizeMode);

		// Réinitialiser le dictionnaire
		borderVerticesMap.Clear();

		// int testIndex = 0;
		
		// Premier passage : génération des chunks et collecte des vertices de bordure
		Dictionary<Vector2, TerrainChunk> chunks = new Dictionary<Vector2, TerrainChunk>();
		Dictionary<Vector2, MeshData> meshDatas = new Dictionary<Vector2, MeshData>();

		for (int xOffset = 0; xOffset < worldChunkLarge; xOffset++) {
			for (int yOffset = 0; yOffset < worldChunkLarge; yOffset++) {
				Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
				TerrainChunk terrainChunk = new TerrainChunk(viewedChunkCoord, (mapChunkSize - 1), transform, terrainMaterial);
				
				// MapData terrainMapData = GenerateMapData(Vector2.zero);
				
				float[,] subMatrix = new float[mapChunkSize, mapChunkSize];

				int xVal = xOffset;
				int yVal = yOffset;
				
				if (yOffset == 2){ yVal = 0; }
				else if (yOffset == 0){ yVal = 2; }
				
				int startRow = xVal * (mapChunkSize);
				int startCol = yVal * (mapChunkSize);
				
				for (int x = 0; x < (mapChunkSize); x++) // Lignes de la sous-matrice
				{
					for (int y = 0; y < (mapChunkSize); y++) // Colonnes de la sous-matrice
					{
						subMatrix[x, y] = allWorldNoiseMap[startRow + x, startCol + y];
					}
				}
				
				MeshData terrainMeshData = MeshGenerator.GenerateTerrainMesh(
					subMatrix, 
					terrainData.meshHeightMultiplier, 
					terrainData.meshHeightCurve, 
					editorPreviewLOD, 
					terrainData.useFlatShading);

				// if (testIndex == 0){
					// Collecter les vertices de bordure
				CollectBorderVertices(terrainMeshData, xOffset, yOffset);
					
				// }
				// testIndex++;

				// Stocker les références pour la mise à jour ultérieure
				chunks[viewedChunkCoord] = terrainChunk;
				meshDatas[viewedChunkCoord] = terrainMeshData;

			}
		}
		
		// Deuxième passage : ajuster les hauteurs
		AdjustBorderHeights();

		// Troisième passage : mettre à jour tous les meshes
		foreach (var kvp in chunks)
		{
			Vector2 coord = kvp.Key;
			TerrainChunk chunk = kvp.Value;
			MeshData meshData = meshDatas[coord];
			
			// Recréer le mesh avec les vertices ajustés
			chunk.DrawMesh(meshData, terrainData.uniformScale);
		}
	}

	private void CollectBorderVertices(MeshData meshData, int chunkX, int chunkY)
	{
		Debug.Log("Collecting border vertices");
		
		// Visualiser tous les vertices normaux en bleu pour référence
		// foreach (var vertex in meshData.vertices) {
		// 	Debug.DrawLine(
		// 		vertex,
		// 		vertex + Vector3.up * 0.5f,
		// 		Color.blue,
		// 		5f
		// 	);
		// }

		foreach (var vertex in meshData.borderVertices) {
			Debug.Log("Vertex: " + vertex);

			// Calculer la position mondiale du vertex en tenant compte de la position du chunk
			Vector3 worldPos = new Vector3(
				vertex.x + chunkX * (mapChunkSize - 1),
				vertex.y,
				vertex.z + chunkY * (mapChunkSize - 1)
			);

			// Visualiser les border vertices en rouge
			Debug.DrawLine(
				worldPos,
				worldPos + Vector3.up * 5f,
				Color.red,
				5f
			);

			// Créer une clé 2D basée sur la position x,z mondiale (arrondie pour éviter les problèmes de précision)
			Vector2 key = new Vector2(
				Mathf.Round(worldPos.x * 100f) / 100f,
				Mathf.Round(worldPos.z * 100f) / 100f
			);

			// Stocker les informations nécessaires pour modifier le vertex plus tard
			if (!borderVerticesMap.ContainsKey(key)) {
				borderVerticesMap[key] = new List<(Vector3 worldPosition, MeshData mesh, int borderIndex)>();
			}
			
			// Ajouter le vertex avec sa position mondiale et les références nécessaires
			borderVerticesMap[key].Add((worldPos, meshData, Array.IndexOf(meshData.borderVertices, vertex)));
		}
	}

	private void AdjustBorderHeights()
	{
		Debug.Log($"Adjusting border heights for {borderVerticesMap.Count} positions");
		
		foreach (var kvp in borderVerticesMap)
		{
			Vector2 position = kvp.Key;
			var vertices = kvp.Value;
			
			if (vertices.Count > 1)
			{
				// Visualiser les positions où il y a des vertices superposés
				Debug.DrawLine(
					new Vector3(position.x, 0, position.y),
					new Vector3(position.x, 10, position.y),
					Color.green,
					30f
				);

				// Calculer la hauteur moyenne
				float avgHeight = 0;
				foreach (var (worldPos, _, _) in vertices)
				{
					avgHeight += worldPos.y;
				}
				avgHeight /= vertices.Count;

				Debug.Log($"Position {position}: Adjusting {vertices.Count} vertices to height {avgHeight}");

				// Appliquer la hauteur moyenne à tous les vertices à cette position
				foreach (var (worldPos, mesh, borderIndex) in vertices)
				{
					// Visualiser l'ajustement
					Debug.DrawLine(
						worldPos,
						new Vector3(worldPos.x, avgHeight, worldPos.z),
						Color.yellow,
						100f
					);

					// Convertir la position mondiale en position locale pour le mesh
					Vector3 localPos = new Vector3(
						mesh.borderVertices[borderIndex].x,
						avgHeight,
						mesh.borderVertices[borderIndex].z
					);

					// Mettre à jour le vertex dans le mesh
					mesh.borderVertices[borderIndex] = localPos;
				}
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