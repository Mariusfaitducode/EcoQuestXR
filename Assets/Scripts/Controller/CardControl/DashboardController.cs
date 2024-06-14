using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ObjectPose
{
    public Vector3 Position;
    public Quaternion Rotation;
    public float Scale;
    
}
public class DashboardController : MonoBehaviour
{
    internal ControlMode controlMode;
    
    public ObjectPose ovrPose;
    public ObjectPose keyboardPose;
    
    internal Transform meshTransform;
    internal Transform centerEyeAnchorTransform;
    

    public void InitializeDashboardController()
    {
        if (controlMode == ControlMode.keyboard)
        {
            transform.position = keyboardPose.Position;
            transform.rotation = keyboardPose.Rotation;
            transform.localScale = new Vector3(keyboardPose.Scale, keyboardPose.Scale, keyboardPose.Scale);
        }
    }
}
