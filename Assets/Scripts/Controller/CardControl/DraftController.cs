using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraftController : MonoBehaviour
{
    internal ControlMode controlMode;
    
    public ObjectPose ovrPose;
    public ObjectPose keyboardPose;
    
    internal Transform meshTransform;
    internal UpdateTerrainRenderer updateTerrainRenderer;
    internal Transform centerEyeAnchorTransform;

    public void InitializeDraftController()
    {
        //if (controlMode == ControlMode.keyboard)
        //{
        //    transform.position = keyboardPose.position;
        //    transform.rotation = keyboardPose.rotation;
        //    transform.localScale = new Vector3(keyboardPose.scale, keyboardPose.scale, keyboardPose.scale);
            
        //} else if (controlMode == ControlMode.ovr)
        //{
        //    transform.rotation = ovrPose.rotation;
        //    transform.localScale = new Vector3(ovrPose.scale, ovrPose.scale, ovrPose.scale);
        //}
    }
    
    void Update()
    {
        if (controlMode == ControlMode.keyboard) return;
        
        //Vector3 shaderCenterMapPosition = updateTerrainRenderer.GetMapCenter();
        //Vector3 centerMapPosition = new Vector3(shaderCenterMapPosition.x, meshTransform.position.y, shaderCenterMapPosition.z);
        //transform.position = centerMapPosition + ovrPose.position;

        // TODO : use lookat
        //this.transform.LookAt(centerEyeAnchorTransform);
    }
}
