using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class UpdateTerrainRenderer : MonoBehaviour
{
    private new Renderer renderer;
    private Material material;
    
    // public GameObject shaderCenter;
    
    public GameObject citySphere;
    public GameObject agricultureSphere;
    public GameObject energySphere;
    public GameObject industrySphere;
    
    internal Vector3 lastMapPosition;
    internal float lastMapScale;
    
    

    void Start()
    {
        renderer = GetComponent<Renderer>();
        material = renderer.material;
        // UpdateLimitTerrainCenter();
    }
    
    
    public Vector3 GetMapCenter()
    {
        Vector2 center = material.GetVector("_Map_Center");
        
        return new Vector3(center.x, 0, center.y);
    }


    public void UpdateMapInformations(bool updateCenter)
    {
        UpdateAreasCenter();

        if (updateCenter)
        {
            UpdateLimitTerrainCenter();
        }
        else
        {
            lastMapPosition = this.transform.position;
            lastMapScale = this.transform.localScale.x;
        }
    }
    
    
    public void UpdateLimitTerrainCenter()
    {
        Vector3 translation = this.transform.position - lastMapPosition;
        float scale = this.transform.localScale.x / lastMapScale;
        
        // Debug.Log("SCALE : " + scale);
        
        Vector2 center = material.GetVector("_Map_Center");
        
        material.SetVector("_Map_Center", new Vector2(center.x + translation.x, center.y + translation.z));
        // this.shaderCenter.transform.position = new Vector3(center.x + translation.x, 0, center.y + translation.z);
        
        material.SetFloat("_Limit_Terrain", material.GetFloat("_Limit_Terrain") * scale);
        
        lastMapPosition = this.transform.position;
        lastMapScale = this.transform.localScale.x;
    }
    
    
    public void InitShaderCenter()
    {
        material.SetVector("_Map_Center", new Vector2(this.transform.position.x, this.transform.position.z));
        
        lastMapPosition = this.transform.position;
        lastMapScale = this.transform.localScale.x;
        // shaderCenter = new Vector3(
        //     this.transform.position.x,
        //     this.transform.position.y,
        //     this.transform.position.z
        // );
    }
    
    public void InitShaderLimitTerrain(float scale)
    {
        float limitTerrain = material.GetFloat("_Limit_Terrain");
        material.SetFloat("_Limit_Terrain", limitTerrain * scale);
    }

    public void UpdateAreasCenter()
    {
        material.SetFloat("_Uniform_Scale", this.transform.localScale.x);
        
        if (agricultureSphere != null) material.SetVector("_Agriculture_Center", new Vector2(agricultureSphere.transform.position.x , agricultureSphere.transform.position.z ));
        if (citySphere != null) material.SetVector("_City_Center", new Vector2(citySphere.transform.position.x , citySphere.transform.position.z ));
        if (energySphere != null) material.SetVector("_Energy_Center", new Vector2(energySphere.transform.position.x , energySphere.transform.position.z ));
        if (industrySphere != null) material.SetVector("_Industry_Center", new Vector2(industrySphere.transform.position.x , industrySphere.transform.position.z ));
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

    public void SetObjectsVisibility(FillMapManager fillMapManager)
    {
        Vector2 center = material.GetVector("_Map_Center");
        
        Vector3 mapCenter = new Vector3(center.x, 0, center.y);
        float limitTerrain = material.GetFloat("_Limit_Terrain");

        foreach (Area area in fillMapManager.areas)
        {
            SetChildrenVisibility(area.hierarchyBuildingFolder, mapCenter, limitTerrain);
            SetChildrenVisibility(area.hierarchyRoadFolder, mapCenter, limitTerrain);
        }
        
        SetChildrenVisibility(fillMapManager.natureData.natureParent, mapCenter, limitTerrain);
        SetChildrenVisibility(fillMapManager.roadCarsFolder, mapCenter, limitTerrain);
        
        
        
    }

    public void SetRoadsVisibility(FillMapManager fillMapManager)
    {
        Vector2 center = material.GetVector("_Map_Center");
        
        // shaderCenter.transform.position = new Vector3(center.x, 0, center.y);
        
        Vector3 mapCenter = new Vector3(center.x, 0, center.y);
        float limitTerrain = material.GetFloat("_Limit_Terrain");
        
        // Roads
        
        GameObject roadParent = fillMapManager.bigRoad;
        
        Material roadMaterial = roadParent.GetComponent<Renderer>().sharedMaterial;
        
        roadMaterial.SetVector("_Map_Center", new Vector2(mapCenter.x, mapCenter.z));
        roadMaterial.SetFloat("_Limit_Terrain", limitTerrain);
        
        roadMaterial.SetFloat("_Uniform_Scale", this.transform.localScale.x);   
    }
    
    
    public void SetChildrenVisibility(GameObject parent, Vector3 center, float radius)
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
