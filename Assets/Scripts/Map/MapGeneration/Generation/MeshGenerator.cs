using UnityEngine;
using System.Collections;

public static class MeshGenerator {

	// Fonction qui va générer un mesh à partir d'une heightMap
	public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve _heightCurve, int levelOfDetail, bool useFlatShading) {
		AnimationCurve heightCurve = new AnimationCurve (_heightCurve.keys);

		int meshSimplificationIncrement = (levelOfDetail == 0)?1:levelOfDetail * 2;

		int borderedSize = heightMap.GetLength (0);
		int meshSize = borderedSize - 2*meshSimplificationIncrement;
		int meshSizeUnsimplified = borderedSize - 2;

		float topLeftX = (meshSizeUnsimplified - 1) / -2f;
		float topLeftZ = (meshSizeUnsimplified - 1) / 2f;


		int verticesPerLine = (meshSize - 1) / meshSimplificationIncrement + 1;

		// Initialisation meshData
		MeshData meshData = new MeshData (verticesPerLine, useFlatShading);

		int[,] vertexIndicesMap = new int[borderedSize,borderedSize];
		int meshVertexIndex = 0;
		int borderVertexIndex = -1;

		// Assignation des indices aux vertices de la map, en différenciant ceux aux bords et ceux à l'intérieur
		for (int y = 0; y < borderedSize; y += meshSimplificationIncrement) {
			for (int x = 0; x < borderedSize; x += meshSimplificationIncrement) {
				bool isBorderVertex = y == 0 || y == borderedSize - 1 || x == 0 || x == borderedSize - 1;

				if (isBorderVertex) {
					vertexIndicesMap [x, y] = borderVertexIndex;
					borderVertexIndex--;
				} else {
					vertexIndicesMap [x, y] = meshVertexIndex;
					meshVertexIndex++;
				}
			}
		}

		// Création du mesh
		for (int y = 0; y < borderedSize; y += meshSimplificationIncrement) {
			for (int x = 0; x < borderedSize; x += meshSimplificationIncrement) {
				
				int vertexIndex = vertexIndicesMap [x, y];
				
				// Détermination de la position x, y, z du vertex
				Vector2 percent = new Vector2 ((x-meshSimplificationIncrement) / (float)meshSize, (y-meshSimplificationIncrement) / (float)meshSize);
				float height = heightCurve.Evaluate (heightMap [x, y]) * heightMultiplier;
				Vector3 vertexPosition = new Vector3 (topLeftX + percent.x * meshSizeUnsimplified, height, topLeftZ - percent.y * meshSizeUnsimplified);

				// Ajout du vertex au meshData
				meshData.AddVertex (vertexPosition, percent, vertexIndex);

				// Ajout des triangles au meshData
				if (x < borderedSize - 1 && y < borderedSize - 1) {
					int a = vertexIndicesMap [x, y];
					int b = vertexIndicesMap [x + meshSimplificationIncrement, y];
					int c = vertexIndicesMap [x, y + meshSimplificationIncrement];
					int d = vertexIndicesMap [x + meshSimplificationIncrement, y + meshSimplificationIncrement];
					meshData.AddTriangle (a,d,c);
					meshData.AddTriangle (d,a,b);
				}
			}
		}
		
		meshData.vertexIndicesMap = vertexIndicesMap;
		// meshData.ProcessMesh ();

		return meshData;

	}
}

public class MeshData {
	public Vector3[] vertices;
	int[] triangles;
	public Vector2[] uvs;
	Vector3[] bakedNormals;

	public Vector3[] borderVertices;
	int[] borderTriangles;

	int triangleIndex;
	int borderTriangleIndex;

	bool useFlatShading;
	
	public int[,] vertexIndicesMap;

	public MeshData(int verticesPerLine, bool useFlatShading) {
		this.useFlatShading = useFlatShading;

		vertices = new Vector3[verticesPerLine * verticesPerLine];
		uvs = new Vector2[verticesPerLine * verticesPerLine];
		triangles = new int[(verticesPerLine-1)*(verticesPerLine-1)*6];

		borderVertices = new Vector3[verticesPerLine * 4 + 4];
		borderTriangles = new int[24 * verticesPerLine];
	}

	public void AddVertex(Vector3 vertexPosition, Vector2 uv, int vertexIndex) {
		if (vertexIndex < 0) {
			borderVertices [-vertexIndex - 1] = vertexPosition;
		} 
		else {
			vertices [vertexIndex] = vertexPosition;
			
			// "You can put whatever data you want into uv"
			// uvs [vertexIndex] = new Vector2(0, vertexPosition.y);
			uvs[vertexIndex] = uv;
		}
	}

	public void AddTriangle(int a, int b, int c) {
		if (a < 0 || b < 0 || c < 0) {
			borderTriangles [borderTriangleIndex] = a;
			borderTriangles [borderTriangleIndex + 1] = b;
			borderTriangles [borderTriangleIndex + 2] = c;
			borderTriangleIndex += 3;
		} else {
			triangles [triangleIndex] = a;
			triangles [triangleIndex + 1] = b;
			triangles [triangleIndex + 2] = c;
			triangleIndex += 3;
		}
	}

	Vector3[] CalculateNormals() {
		// Créer un tableau de normales pour tous les vertices (normaux + bordure)
		Vector3[] vertexNormals = new Vector3[vertices.Length + borderVertices.Length];

		// 1. Calcul des normales pour les triangles normaux
		int triangleCount = triangles.Length / 3;
		for (int i = 0; i < triangleCount; i++) {
			int normalTriangleIndex = i * 3;
			int vertexIndexA = triangles[normalTriangleIndex];
			int vertexIndexB = triangles[normalTriangleIndex + 1];
			int vertexIndexC = triangles[normalTriangleIndex + 2];

			Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
			vertexNormals[vertexIndexA] += triangleNormal;
			vertexNormals[vertexIndexB] += triangleNormal;
			vertexNormals[vertexIndexC] += triangleNormal;
		}

		// 2. Calcul des normales pour les triangles de bordure
		int borderTriangleCount = borderTriangles.Length / 3;
		for (int i = 0; i < borderTriangleCount; i++) {
			int normalTriangleIndex = i * 3;
			int vertexIndexA = borderTriangles[normalTriangleIndex];
			int vertexIndexB = borderTriangles[normalTriangleIndex + 1];
			int vertexIndexC = borderTriangles[normalTriangleIndex + 2];

			Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
			
			// Ajouter la normale aux vertices appropriés (en gérant les indices négatifs)
			AddNormalToVertex(vertexNormals, vertexIndexA, triangleNormal);
			AddNormalToVertex(vertexNormals, vertexIndexB, triangleNormal);
			AddNormalToVertex(vertexNormals, vertexIndexC, triangleNormal);
		}

		// 3. Normalisation de toutes les normales
		for (int i = 0; i < vertexNormals.Length; i++) {
			vertexNormals[i].Normalize();
		}

		return vertexNormals;
	}

	// Nouvelle méthode helper pour ajouter une normale à un vertex
	private void AddNormalToVertex(Vector3[] vertexNormals, int vertexIndex, Vector3 normal) {
		if (vertexIndex >= 0) {
			// Vertex normal
			vertexNormals[vertexIndex] += normal;
		} else {
			// Border vertex (convertir l'index négatif en positif et ajouter l'offset)
			int borderIndex = vertices.Length + (-vertexIndex - 1);
			vertexNormals[borderIndex] += normal;
		}
	}

	Vector3 SurfaceNormalFromIndices(int indexA, int indexB, int indexC) {
		// Obtenir les positions des vertices en tenant compte des indices négatifs
		Vector3 pointA = GetVertexPosition(indexA);
		Vector3 pointB = GetVertexPosition(indexB);
		Vector3 pointC = GetVertexPosition(indexC);

		Vector3 sideAB = pointB - pointA;
		Vector3 sideAC = pointC - pointA;
		return Vector3.Cross(sideAB, sideAC).normalized;
	}

	// Nouvelle méthode helper pour obtenir la position d'un vertex
	private Vector3 GetVertexPosition(int index) {
		if (index >= 0) {
			return vertices[index];
		} else {
			return borderVertices[-index - 1];
		}
	}

	// public void ProcessMesh() {
	// 	if (useFlatShading) {
	// 		FlatShading ();
	// 	} else {
	// 		BakeNormals ();
	// 	}
	// }

	void BakeNormals() {
		bakedNormals = CalculateNormals ();
	}

	void FlatShading() {
        // En flat shading, chaque triangle utilise ses propres vertices
        // pour avoir des arêtes nettes entre les triangles
        Vector3[] flatShadedVertices = new Vector3[triangles.Length + borderTriangles.Length];
        Vector2[] flatShadedUvs = new Vector2[triangles.Length + borderTriangles.Length];
        
        // 1. Traitement des triangles normaux
        for (int i = 0; i < triangles.Length; i++) {
            // Pour chaque index dans triangles, on crée un nouveau vertex
            flatShadedVertices[i] = vertices[triangles[i]];
            flatShadedUvs[i] = uvs[triangles[i]];
            // L'index devient séquentiel puisque chaque vertex est unique
            triangles[i] = i;
        }
        
        // 2. Traitement des triangles de bordure
        for (int i = 0; i < borderTriangles.Length; i++) {
            int vertexIndex = borderTriangles[i];
            if (vertexIndex >= 0) {
                // Vertex normal
                flatShadedVertices[triangles.Length + i] = vertices[vertexIndex];
                flatShadedUvs[triangles.Length + i] = uvs[vertexIndex];
            } else {
                // Border vertex (index négatif)
                flatShadedVertices[triangles.Length + i] = borderVertices[-vertexIndex - 1];
                Vector3 vertex = borderVertices[-vertexIndex - 1];
                flatShadedUvs[triangles.Length + i] = new Vector2(vertex.x, vertex.z);
            }
            // Mise à jour de l'index pour pointer vers le nouveau vertex
            borderTriangles[i] = triangles.Length + i;
        }
        
        // 3. Remplacement des anciens tableaux par les nouveaux
        vertices = flatShadedVertices;
        uvs = flatShadedUvs;
    }

	public Mesh CreateMesh() {
		Mesh mesh = new Mesh();
		
		// 1. Préparation des tableaux combinés
		// On crée des nouveaux tableaux qui vont contenir à la fois les données normales et les données de bordure
		Vector3[] allVertices = new Vector3[vertices.Length + borderVertices.Length];
		Vector2[] allUvs = new Vector2[vertices.Length + borderVertices.Length];
		
		// 2. Copie des données normales (intérieures)
		// On copie d'abord tous les vertices normaux au début des tableaux
		System.Array.Copy(vertices, 0, allVertices, 0, vertices.Length);
		System.Array.Copy(uvs, 0, allUvs, 0, vertices.Length);
		
		// 3. Ajout des vertices de bordure
		// On copie les border vertices à la suite des vertices normaux
		System.Array.Copy(borderVertices, 0, allVertices, vertices.Length, borderVertices.Length);
		
		// 4. Création des UVs pour les border vertices
		// Pour chaque border vertex, on crée une coordonnée UV basée sur sa position x,z
		// Cela permet d'avoir une texture correctement mappée sur les bordures
		for (int i = 0; i < borderVertices.Length; i++) {
			Vector3 vertex = borderVertices[i];
			allUvs[vertices.Length + i] = new Vector2(vertex.x, vertex.z);
		}
		
		// 5. Combinaison des triangles
		// On crée un tableau qui va contenir tous les triangles (normaux + bordure)
		int[] allTriangles = new int[triangles.Length + borderTriangles.Length];
		// On copie d'abord les triangles normaux
		System.Array.Copy(triangles, 0, allTriangles, 0, triangles.Length);
		
		// 6. Traitement des triangles de bordure
		// Les border triangles utilisent des indices négatifs pour référencer les border vertices
		// On doit les convertir en indices positifs qui pointent vers la bonne position dans allVertices
		for (int i = 0; i < borderTriangles.Length; i++) {
			if (borderTriangles[i] >= 0) {
				// Si l'index est positif, il référence un vertex normal
				allTriangles[triangles.Length + i] = borderTriangles[i];
			} else {
				// Si l'index est négatif, il référence un border vertex
				// On le convertit en index positif en:
				// 1. Le rendant positif (-index - 1)
				// 2. Ajoutant un offset de vertices.Length pour pointer dans la section border vertices
				allTriangles[triangles.Length + i] = vertices.Length + (-borderTriangles[i] - 1);
			}
		}
		
		// 7. Application des données au mesh
		mesh.vertices = allVertices;
		mesh.triangles = allTriangles;
		mesh.uv = allUvs;
		
		// 8. Traitement final selon le mode de shading
		if (useFlatShading) {
			FlatShading();
			mesh.RecalculateNormals();
		} else {
			// Calculer les normales pour tous les vertices
			Vector3[] normals = CalculateNormals();
			mesh.vertices = allVertices;  // Assurez-vous que ceci est fait avant
			mesh.normals = normals;       // d'assigner les normales
		}
		
		return mesh;
	}
	
	
	
	

}
