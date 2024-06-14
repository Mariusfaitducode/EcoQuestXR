using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class MapController : MonoBehaviour
{
    
    internal GameManager gameManager;

    
    [System.Serializable]
    public struct MouvementSettings
    {
        public float rotationSpeed;
        public float movingSpeed;
        public float scalingSpeed;
        public float scalingMin;
        
        public float angleAdapter;

    }
    
    public MouvementSettings mouvementSettings;
    
    
    private new Renderer renderer;
    private UpdateTerrainRenderer updateTerrainRenderer;
    private float originalSize;
    
    // OVR
    
    internal GameObject ovrCameraRig;
    internal GameObject centerEyeAnchor;
    
    
    public void InitializeController()
    {
        renderer = GetComponent<Renderer>();
        originalSize = GetComponent<MeshFilter>().mesh.bounds.size.x;
        
        updateTerrainRenderer = GetComponent<UpdateTerrainRenderer>();


        Vector3 initialPosition = centerEyeAnchor.transform.position + new Vector3(0.0f, 0.0f, 0.0f);
        transform.position = initialPosition;
        updateTerrainRenderer.InitShaderCenter();

    }
    
    void Update()
    {

        if (gameManager.controlMode == ControlMode.keyboard)
        {
            bool moved = KeyBoardMapInteraction.Controller(this.transform, Vector3.zero, renderer, mouvementSettings, originalSize);


            if (moved)
            {
                updateTerrainRenderer.UpdateMapInformations(false);
                // updateTerrainRenderer.SetObjectsVisibility(gameManager);
            }
        }
        else // OVR mode
        {

            bool moved = OvrMapInteraction.Controller(this.transform, updateTerrainRenderer.GetMapCenter(), 
                renderer, mouvementSettings, originalSize, ovrCameraRig.transform);

            if (moved)
            {
                updateTerrainRenderer.UpdateMapInformations(false);
                updateTerrainRenderer.SetObjectsVisibility(gameManager.fillMapManager);
                updateTerrainRenderer.SetRoadsVisibility(gameManager.fillMapManager);

            }
            else if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.5f || OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.5f)
            {
                updateTerrainRenderer.UpdateMapInformations(true);
                updateTerrainRenderer.SetObjectsVisibility(gameManager.fillMapManager);
                updateTerrainRenderer.SetRoadsVisibility(gameManager.fillMapManager);
            }
        }
    }
    
    

    

    
}
