using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapSoundsGestion
{
    
    
    public static bool AreaAudioSource(Area area, Camera camera, float uniformScale, bool timePaused)
    {
        
        if (timePaused)
        {
            area.sphere.GetComponent<AudioSource>().volume = 0;
            return false;
        }
        
        Vector3 camPos = camera.transform.position;
        
        float distance = Vector3.Distance(camPos, area.sphere.transform.position);
        
        if (distance < area.data.radius * 4 * uniformScale)
        {
            float volume = 1 - distance / (area.data.radius * 2 * uniformScale);
            area.sphere.GetComponent<AudioSource>().volume = volume;
            
            // Debug.Log("Audio on : Area " + area.data.type + " - Volume : " + volume + " - Distance : " + distance + " - Radius : " + area.data.radius + " - Camera : " + camPos + " - Sphere : " + area.sphere.transform.position);
            return true;
        }
        
        area.sphere.GetComponent<AudioSource>().volume = 0;
        
        // Debug.Log("Audio off : Area " + area.data.type + " - Distance : " + distance + " - Radius : " + area.data.radius + " - Camera : " + camPos + " - Sphere : " + area.sphere.transform.position);
        return false;
        
    }
    
    
}
