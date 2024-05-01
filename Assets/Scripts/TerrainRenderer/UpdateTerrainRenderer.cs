using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class UpdateTerrainRenderer : MonoBehaviour
{
    private Renderer _renderer;
    public Material _material;
    
    float savedMinHeight; 
    float savedMaxHeight;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material; // Assurez-vous que cela n'impacte pas d'autres objets partageant le m�me mat�riau
        UpdateCenter();
    }

    void UpdateCenter()
    {
        
        _material.SetVector("_Map_Center", new Vector2(this.transform.position.x, this.transform.position.z));
    }

    public void UpdateMeshHeights(float minHeight, float maxHeight)
    {
        // Debug.Log("UpdateMeshHeights");
       
        savedMinHeight = minHeight;
        savedMaxHeight = maxHeight;

        _material.SetFloat("_Min_Height", minHeight);
        _material.SetFloat("_Max_Height", maxHeight);

    }
    void Update()
    {
        // Update center continuously if needed, or call UpdateCenter() only when necessary
        UpdateCenter();
    }
}
