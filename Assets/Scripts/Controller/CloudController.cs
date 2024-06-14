using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    public ObjectPose ovrPose;
    
    internal Transform meshTransform;
    internal UpdateTerrainRenderer updateTerrainRenderer;
    internal Transform centerEyeAnchorTransform;

    public void InitializeCloudController()
    {
        transform.rotation = ovrPose.rotation;
        transform.localScale = new Vector3(ovrPose.scale, ovrPose.scale, ovrPose.scale);
    }
    
    void Update()
    {
        Vector3 shaderCenterMapPosition = updateTerrainRenderer.GetMapCenter();
        Vector3 centerMapPosition = new Vector3(shaderCenterMapPosition.x, meshTransform.position.y, shaderCenterMapPosition.z);
        transform.position = centerMapPosition + ovrPose.position;
    
        // TODO : use lookat
        // transform.rotation = meshTransform.rotation;
    }
}
