using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTerraforming : MonoBehaviour
{

    
    // private void OnTriggerStay(Collider collision)
    // {
    //     Debug.Log("Trigger continue!");
    //     // Envoyer un message ou activer un événement, etc.
    //     // Récupérer le point de contact
    //     Vector3 contactPoint = collision.ClosestPoint(transform.position);
    //         
    //     // Déformer le terrain au point de contact
    //     DeformTerrain(contactPoint);
    // }
    
    public Transform sphere; // La sphère qui déforme le terrain
    public float deformationRadius = 1f; // Rayon de déformation de la sphère

    void Start()
    {
        
    }

    void Update()
    {
        // Récupérer le mesh du terrain
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        //Obtenir la pos des vertices
        Vector3[] vertices = mesh.vertices;
        
        // Obtenir la position de la sphère
        Vector3 spherePosition = sphere.position;

        // Parcourir tous les vertices du mesh
        for (int i = 0; i < vertices.Length; i++)
        {
            // Obtenir la position du vertice dans l'espace mondial
            Vector3 vertexPosition = transform.TransformPoint(vertices[i]);
            
            // Vérifier si le vertice est en dessous de la sphère
            if (vertexPosition.y > spherePosition.y)
            {
                // Calculer la distance entre le vertice et la sphère (sans prendre en compte la hauteur)
                float distance = Vector2.Distance(new Vector2(vertexPosition.x, vertexPosition.z), new Vector2(spherePosition.x, spherePosition.z));
                // Debug.Log("id :  " + i + "height : " + vertexPosition.y);

                // Si le vertice est à l'intérieur du rayon de déformation
                if (distance < deformationRadius)
                {
                    // // Déformer le mesh à la hauteur de la sphère à cet endroit
                    // Vector3 deformationPoint = new Vector3(vertexPosition.x, spherePosition.y, vertexPosition.z);
                    // // Convertir la position déformée en espace local
                    // Vector3 localDeformationPoint = transform.InverseTransformPoint(deformationPoint);
                    // Mettre à jour la position du vertice
                    vertices[i] = new Vector3(vertexPosition.x, spherePosition.y, vertexPosition.z);
                    Debug.DrawLine(vertexPosition, vertexPosition + new Vector3(0, 1, 0), Color.red, 100f);
                    Debug.Log("sphere : " + spherePosition + ", vertice : " +vertexPosition);
                }
            }
        }
        
        mesh.vertices = vertices;

        // Mettre à jour le mesh
        // meshFilter.mesh.RecalculateNormals();
        // meshFilter.mesh.RecalculateBounds();
    }
}
