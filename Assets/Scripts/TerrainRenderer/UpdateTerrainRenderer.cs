using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class UpdateTerrainRenderer : MonoBehaviour
{
    private Renderer _renderer;
    private Material _material;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material; // Assurez-vous que cela n'impacte pas d'autres objets partageant le même matériau
        UpdateCenter();
    }

    void UpdateCenter()
    {
        
        _material.SetVector("_Map_Center", new Vector2(this.transform.position.x, this.transform.position.z));
    }

    void Update()
    {
        // Update center continuously if needed, or call UpdateCenter() only when necessary
        UpdateCenter();
    }
}
