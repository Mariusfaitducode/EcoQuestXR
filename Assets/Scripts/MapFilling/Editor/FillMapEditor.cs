using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (FillMapArea))]
public class FillMapEditor : Editor {

    public override void OnInspectorGUI() {
        FillMapArea fillMap = (FillMapArea)target;

        if (DrawDefaultInspector ()) {
            if (fillMap.autoUpdate) {
                fillMap.SetAreaInEditor ();
            }
        }

        if (GUILayout.Button ("Generate")) {
            fillMap.SetAreaInEditor ();
        }
    }
}
