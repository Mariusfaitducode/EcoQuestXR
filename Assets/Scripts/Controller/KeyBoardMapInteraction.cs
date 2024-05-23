using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class KeyBoardMapInteraction
{


    public static bool Controller(Transform objectTransform, Vector3 initialPosition, Renderer objectRenderer, MapController.MouvementSettings settings, float mapSize )
    {
        bool moved = false;

        // Détecter la touche Shift (Left ou Right)
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            // Scale
            if (Input.GetKey(KeyCode.UpArrow)) { MapMouvement.ScaleObjectAroundPoint(objectTransform, initialPosition, settings.scalingSpeed, settings.scalingMin); }
            else if (Input.GetKey(KeyCode.DownArrow)){ MapMouvement.ScaleObjectAroundPoint(objectTransform, initialPosition, -settings.scalingSpeed, settings.scalingMin); }
            
            // Rotation
            if (Input.GetKey(KeyCode.RightArrow)) { MapMouvement.RotateObjectAroundPoint(objectTransform, initialPosition, settings.rotationSpeed); }
            else if (Input.GetKey(KeyCode.LeftArrow)) { MapMouvement.RotateObjectAroundPoint(objectTransform, initialPosition, -settings.rotationSpeed); }
            
            moved = true;
        }
        else
        {
            int leftKey = Input.GetKey(KeyCode.LeftArrow) ? 1 : 0;
            int rightKey = Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
            int upKey = Input.GetKey(KeyCode.UpArrow) ? 1 : 0;
            int downKey = Input.GetKey(KeyCode.DownArrow) ? 1 : 0;
            
            Vector2 vectorPlayer = new Vector2(rightKey - leftKey, upKey - downKey);

            if (vectorPlayer != Vector2.zero)
            {
                float mapRotation = objectRenderer.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
                //
                vectorPlayer = MapMouvement.RotateReference(vectorPlayer, mapRotation);
                
                Vector2 xVectorPlayer = MapMouvement.RotateReference(new Vector2(vectorPlayer.x, 0), -mapRotation); 
                Vector2 yVectorPlayer = MapMouvement.RotateReference(new Vector2(0, vectorPlayer.y), -mapRotation); 

            
                MapMouvement.Translate(objectTransform, initialPosition, objectRenderer, xVectorPlayer, settings.movingSpeed, mapSize);
                MapMouvement.Translate(objectTransform, initialPosition, objectRenderer, yVectorPlayer, settings.movingSpeed, mapSize);
                
                moved = true;
            }
        }
        return moved;
    }
    
   
    
    
}
