using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class UpdateTerrainRenderer : MonoBehaviour
{
    private new Renderer renderer;
    private Material material;
    
    public GameObject citySphere;
    public GameObject agricultureSphere;
    public GameObject energySphere;
    public GameObject industrySphere;
    

    void Start()
    {
        renderer = GetComponent<Renderer>();
        material = renderer.material;
        UpdateLimitTerrainCenter();
    }


    public void UpdateMapInformations(bool updateCenter)
    {
        UpdateAreasCenter();

        if (updateCenter)
        {
            UpdateLimitTerrainCenter();
        }
    }
    
    
    public void UpdateLimitTerrainCenter()
    {
        material.SetVector("_Map_Center", new Vector2(this.transform.position.x, this.transform.position.z));
    }

    public void UpdateAreasCenter()
    {
        material.SetFloat("_Uniform_Scale", this.transform.localScale.x);
        
        material.SetVector("_Agriculture_Center", new Vector2(agricultureSphere.transform.position.x , agricultureSphere.transform.position.z ));
        material.SetVector("_City_Center", new Vector2(citySphere.transform.position.x , citySphere.transform.position.z ));
        material.SetVector("_Energy_Center", new Vector2(energySphere.transform.position.x , energySphere.transform.position.z ));
        material.SetVector("_Industry_Center", new Vector2(industrySphere.transform.position.x , industrySphere.transform.position.z ));
    }

    public void UpdateMeshHeights(float minHeight, float maxHeight)
    {
        renderer = GetComponent<Renderer>();
        material = renderer.sharedMaterial;

        material.SetFloat("_Min_Height", minHeight);
        material.SetFloat("_Max_Height", maxHeight);
    }
    
    
    public void SetAreaShader(Area area)
    {
        Vector2 position = new Vector2(area.sphere.transform.position.x, area.sphere.transform.position.z);
        
        material.SetFloat("_Uniform_Scale", this.transform.localScale.x);

        if (area.data.type == AreaType.City)
        {
            material.SetFloat("_City_Radius", area.data.radius);
            material.SetFloat("_City_Start_Radius", area.data.startRadius);
            material.SetVector("_City_Center", new Vector2(position.x, position.y));
        }
        
        if (area.data.type == AreaType.Industry)
        {
            material.SetFloat("_Industry_Radius", area.data.radius);
            material.SetFloat("_Industry_Start_Radius", area.data.startRadius);
            material.SetVector("_Industry_Center", new Vector2(position.x, position.y));
        }
        
        if (area.data.type == AreaType.Energy)
        {
            material.SetFloat("_Energy_Radius", area.data.radius);
            material.SetFloat("_Energy_Start_Radius", area.data.startRadius);
            material.SetVector("_Energy_Center", new Vector2(position.x, position.y));
        }
        
        if (area.data.type == AreaType.Agriculture)
        {
            material.SetFloat("_Agriculture_Radius", area.data.radius);
            material.SetFloat("_Agriculture_Start_Radius", area.data.startRadius);
            material.SetVector("_Agriculture_Center", new Vector2(position.x, position.y));
        }
    }
    
    
    // Objects visibility

    public void SetObjectsVisibility(GameManager gameManager)
    {
        Vector2 center = material.GetVector("_Map_Center");
        Vector3 mapCenter = new Vector3(center.x, 0, center.y);
        float limitTerrain = material.GetFloat("_Limit_Terrain");
        
        FillMapManager fillMapManager = gameManager.fillMapManager;

        foreach (Area area in fillMapManager.areas)
        {
            SetChildrenVisibility(area.hierarchyBuildingFolder, mapCenter, limitTerrain);
            SetChildrenVisibility(area.hierarchyRoadFolder, mapCenter, limitTerrain);
        }
        
        SetChildrenVisibility(fillMapManager.natureData.natureParent, mapCenter, limitTerrain);
        
        
        // Roads
        
        GameObject roadParent = fillMapManager.roadParent;
        
        Material roadMaterial = roadParent.GetComponent<Renderer>().sharedMaterial;
        
        roadMaterial.SetVector("_Map_Center", new Vector2(mapCenter.x, mapCenter.z));
        roadMaterial.SetFloat("_Limit_Terrain", limitTerrain);
        
        // roadMaterial.SetFloat("_Uniform_Scale", this.transform.localScale.x);
        
        
    }
    
    
    public void SetChildrenVisibility(GameObject parent, Vector2 center, float radius)
    {
        foreach (Transform child in parent.transform)
        {
            
            if (FillMapUtils.IsVertexInsideCircle(child.position, center, radius))
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
            // child.gameObject.SetActive(visible);
        }
    }
}
