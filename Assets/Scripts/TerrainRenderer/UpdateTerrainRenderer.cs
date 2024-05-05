using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class UpdateTerrainRenderer : MonoBehaviour
{
    private new Renderer renderer;
    private Material material;
    
    float savedMinHeight; 
    float savedMaxHeight;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        material = renderer.material;
        UpdateCenter();
    }

    public void UpdateCenter()
    {
        
        material.SetVector("_Map_Center", new Vector2(this.transform.position.x, this.transform.position.z));
    }

    public void UpdateMeshHeights(float minHeight, float maxHeight)
    {
        // Debug.Log("UpdateMeshHeights");
        
        renderer = GetComponent<Renderer>();
        material = renderer.sharedMaterial;
       
        savedMinHeight = minHeight;
        savedMaxHeight = maxHeight;

        material.SetFloat("_Min_Height", minHeight);
        material.SetFloat("_Max_Height", maxHeight);

    }
    void Update()
    {
        // Update center continuously if needed, or call UpdateCenter() only when necessary
        // UpdateCenter();
    }
}
