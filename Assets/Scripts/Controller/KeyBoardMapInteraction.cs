using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KeyBoardMapInteraction
{


    public static void Controller(Transform objectTransform, Vector3 initialPosition, Renderer objectRenderer, MapController.MouvementSettings settings, float mapSize )
    {

        // Détecter la touche Shift (Left ou Right)
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            // Scale
            if (Input.GetKey(KeyCode.UpArrow)) { MapMouvement.ScaleObjectAroundPoint(objectTransform, initialPosition, settings.scalingSpeed, settings.scalingMin); }
            else if (Input.GetKey(KeyCode.DownArrow)){ MapMouvement.ScaleObjectAroundPoint(objectTransform, initialPosition, -settings.scalingSpeed, settings.scalingMin); }
            
            // Rotation
            if (Input.GetKey(KeyCode.RightArrow)) { MapMouvement.RotateObjectAroundPoint(objectTransform, initialPosition, settings.rotationSpeed); }
            else if (Input.GetKey(KeyCode.LeftArrow)) { MapMouvement.RotateObjectAroundPoint(objectTransform, initialPosition, -settings.rotationSpeed); }
        }
        else
        {
            // Translation
            // x
            if (Input.GetKey(KeyCode.UpArrow)) { MapMouvement.Translate(objectTransform, initialPosition, objectRenderer, Vector2.up, settings.movingSpeed, mapSize); }
            else if (Input.GetKey(KeyCode.DownArrow)){ MapMouvement.Translate(objectTransform, initialPosition, objectRenderer, Vector2.down, settings.movingSpeed, mapSize); }
            
            // z
            if (Input.GetKey(KeyCode.RightArrow)) { MapMouvement.Translate(objectTransform, initialPosition, objectRenderer, Vector2.right, settings.movingSpeed, mapSize); }
            else if (Input.GetKey(KeyCode.LeftArrow)) { MapMouvement.Translate(objectTransform, initialPosition, objectRenderer, Vector2.left, settings.movingSpeed, mapSize); }
        }
    }
    
   
    
    
}
