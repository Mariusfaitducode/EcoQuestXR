using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class MapInteraction 
{



    public static void Controller()
    {
        
    }
    
    
    
    public static void Scale(Transform transform, Vector2 leftAxis, float scalingSpeed)
    {
       
        float newS = transform.localScale.x + leftAxis.y * scalingSpeed * Time.deltaTime;
        
        if (newS < 0.0030f)
        {
            newS = 0.0030f;
        }
        
        transform.localScale = new Vector3(newS, newS, newS);
         
    }
    
    public static void Rotate(Transform transform, Vector3 rotationPoint, Vector2 leftAxis, float rotationSpeed)
    {
        
        // float newY = leftAxis.x * rotationSpeed * Time.deltaTime;
        // transform.Rotate(0.0f, newY, 0.0f, Space.World);
        
        RotateObjectAroundPoint(transform, rotationPoint, leftAxis.x * rotationSpeed * Time.deltaTime);
        
    }
    
    static void RotateObjectAroundPoint(Transform obj, Vector3 point, float angle)
    {
        // Transfert de l'objet au point pivot
        Vector3 direction = obj.position - point;
        
        // Création de la rotation
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        
        // Application de la rotation
        direction = rotation * direction;

        // Mise à jour de la position de l'objet
        obj.position = point + direction;

        // Rotation de l'objet autour de son axe
        obj.Rotate(0, angle, 0);
    }


    public static void Translate(Transform mapTransform, Transform tableTransform, Renderer renderer, Vector2 rightAxis, float translationSpeed, float originalSize)
    {
        
        //     vectorPlayer.y = 0;
        
        float limitTerrain = renderer.material.GetFloat("_Limit_Terrain");
        
        Debug.Log("Original Size : " + originalSize);
        Debug.Log("Mesh bounds" + renderer.bounds.size);
        
        Debug.Log("Local scale" + mapTransform.localScale);
        Debug.Log("Lossy scale" + mapTransform.lossyScale);
        
        float mapSize = originalSize;

        float mapRotation = renderer.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;

        
        Vector3 newCoordinates = mapTransform.position + new Vector3(rightAxis.x, 0, rightAxis.y).normalized * Time.deltaTime * translationSpeed;
        // mapTransform.position = newCoordinates;

        
        // if (!Utils.Collide(mapSize, mapRotation,tableTransform.position, limitTerrain, newCoordinates))
        // {
        //     mapTransform.position = newCoordinates;
        // }
        // else
        // {
        //     Debug.Log("Out Of Bound : Left - Right");
        // }
    }

    // public static void Move()
    // {
    //     // Get right controller thumbstick axis
    //     
    //     // Get right controller thumbstick axis
    //     bool rightButtonThumbstickTemp = OVRInput.Get(OVRInput.RawButton.RThumbstick);
    //     
    //     Vector3 vectorPlayer = _initialPosition - player.transform.position;
    //     vectorPlayer.y = 0;
    //     
    //     float circleRadius = _material.GetFloat("_Limit_Terrain");
    //     
    //     if (Math.Abs(rightAxisTemp.x) > 0.5)
    //     {
    //         Vector3 newHorizontalCoordinates = transform.position + new Vector3(vectorPlayer.z, vectorPlayer.y, -vectorPlayer.x).normalized * Time.deltaTime * movingSpeed * rightAxisTemp.x;
    //         if (!Utils.Collide(this.gameObject, _initialPosition, circleRadius, newHorizontalCoordinates))
    //         {
    //             transform.position = newHorizontalCoordinates;
    //         }
    //         else
    //         {
    //             Debug.Log("Out Of Bound : Left - Right");
    //         }
    //         
    //
    //     }
    //     
    //     if (Math.Abs(rightAxisTemp.y) > 0.5)
    //     {
    //         Vector3 newVerticalCoordinates = transform.position + vectorPlayer.normalized * Time.deltaTime * movingSpeed * rightAxisTemp.y;
    //         if (!Utils.Collide(this.gameObject, _initialPosition, circleRadius, newVerticalCoordinates))
    //         {
    //             transform.position = newVerticalCoordinates;
    //         }
    //         else
    //         {
    //             Debug.Log("Out Of Bound : Up - Down");
    //         }
    //     }
    //
    //     // reset position
    //     if (_initialPosition != transform.position & rightButtonThumbstickTemp)
    //     {
    //         transform.position = _initialPosition;
    //         //Debug.Log("Reset position");
    //     }
    // }
}
