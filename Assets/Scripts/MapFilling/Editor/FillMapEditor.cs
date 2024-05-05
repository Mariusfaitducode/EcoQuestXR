using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (FillMapManager))]
public class FillMapEditor : Editor {

    public override void OnInspectorGUI() {
        FillMapManager fillMap = (FillMapManager)target;

        if (DrawDefaultInspector ()) {
            if (fillMap.autoUpdate) {
                fillMap.SetAreaInEditor ();
            }
        }

        if (GUILayout.Button ("Set Position")) {
            fillMap.SetAreaInEditor ();
        }
        
        if (GUILayout.Button ("Fill Area")) {
            fillMap.FillAreaInEditor();
        }
        
        if (GUILayout.Button ("Set Area Shader")) {
            fillMap.SetAreaShaderInEditor();
        }
        
        if (GUILayout.Button ("Generate Roads")) {
            fillMap.GenerateRoadOnMapInEditor();
        }
        
        if (GUILayout.Button ("Generate river")) {
            fillMap.GenerateRiverInEditor();
        }
    }
}
