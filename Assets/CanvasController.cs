using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CanvasController : MonoBehaviour
{
    internal ControlMode controlMode;

    internal Transform meshTransform;
    internal UpdateTerrainRenderer updateTerrainRenderer;
    internal Transform centerEyeAnchorTransform;

    public ObjectPose ovrPose;
    public ObjectPose keyboardPose;

    public Transform TerrainCanvaParent;



    // Start is called before the first frame update
    public void InitializeCanvasController()
    {
        if (controlMode == ControlMode.keyboard)
        {
            TerrainCanvaParent.position = keyboardPose.position;
            TerrainCanvaParent.rotation = keyboardPose.rotation;
            TerrainCanvaParent.localScale = new Vector3(keyboardPose.scale, keyboardPose.scale, keyboardPose.scale);

        }
        else if (controlMode == ControlMode.ovr)
        {
            TerrainCanvaParent.rotation = ovrPose.rotation;
            TerrainCanvaParent.localScale = new Vector3(ovrPose.scale, ovrPose.scale, ovrPose.scale);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (controlMode == ControlMode.keyboard) return;

        Vector3 shaderCenterMapPosition = updateTerrainRenderer.GetMapCenter();
        Vector3 centerMapPosition = new Vector3(shaderCenterMapPosition.x, meshTransform.position.y, shaderCenterMapPosition.z);
        TerrainCanvaParent.position = centerMapPosition + ovrPose.position;


        TerrainCanvaParent.LookAt(centerEyeAnchorTransform);

    }
}
