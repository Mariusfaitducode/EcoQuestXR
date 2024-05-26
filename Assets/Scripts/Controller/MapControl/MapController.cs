using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class MapController : MonoBehaviour
{
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

    // Move
    private GameObject table;

    // private bool playerHasMoved = false;
    // private bool tableFound = false;
    
    private new Renderer renderer;
    
    public GameObject ovrPlayer;
    
    public UpdateTerrainRenderer updateTerrainRenderer;
    
    public GameManager gameManager;

    public bool useKeyboard = true;
    
    
    private float originalSize;
    // private float originalScale;
    
    // private float epsilon = 0.0001f;
    
    
    
    
    
    void Start()
    {
        renderer = GetComponent<Renderer>();
        originalSize = GetComponent<MeshFilter>().mesh.bounds.size.x;
    }
    
    void Update()
    {

        if (useKeyboard)
        {
            bool moved = KeyBoardMapInteraction.Controller(this.transform, Vector3.zero, renderer, mouvementSettings, originalSize);


            if (moved)
            {
                updateTerrainRenderer.UpdateMapInformations(false);
                // updateTerrainRenderer.SetObjectsVisibility(gameManager);
            }
        }
        else
        {
            
            if (table == null)
            {
                table = GameObject.FindGameObjectWithTag("Table");

                if (table != null)
                {
                    Vector3 initialPosition = table.transform.position;
                    transform.position = initialPosition;
                    updateTerrainRenderer.InitShaderCenter();
                }
                return;
            }


            bool moved = OvrMapInteraction.Controller(this.transform, updateTerrainRenderer.GetMapCenter(), renderer, mouvementSettings, originalSize, ovrPlayer.transform);

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
