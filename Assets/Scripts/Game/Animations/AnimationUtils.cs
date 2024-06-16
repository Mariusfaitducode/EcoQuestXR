using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationUtils
{
    public static IEnumerator AnimationMapTransition(
        Transform mapTransform,
        UpdateTerrainRenderer mapUpdateTerrainRenderer,
        FillMapManager fillMapManager,
        float animationDuration,
        Vector3 startLocation,
        Vector3 targetLocation,
        float startFloatScale = 1f,
        float targetFloatScale = 1f,
        bool isScaleEnabled = true,
        List<GameObject> objectsToHide = null)
    {
        // Animation de transition dezoom
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;

            mapTransform.position = Vector3.Lerp(startLocation, targetLocation, t);
            
            if (isScaleEnabled)
                mapTransform.localScale = Vector3.Lerp(Vector3.one * startFloatScale, Vector3.one * targetFloatScale, t);
            
            mapUpdateTerrainRenderer.UpdateMapInformations(false);
            mapUpdateTerrainRenderer.SetObjectsVisibility(fillMapManager);
            mapUpdateTerrainRenderer.SetRoadsVisibility(fillMapManager);
            
            if (objectsToHide != null)
                ObjectUtils.HideObjects(objectsToHide);

            yield return null;
        }
        
        // Ensure that at the end of the transition, the map is at the exact target location
        mapTransform.position = targetLocation;
        
        if (isScaleEnabled)
            mapTransform.localScale = Vector3.one * targetFloatScale;
    }
    
    public static IEnumerator AnimationHideObjects(List<GameObject> objectsToHide, float animationDuration)
    {
        float intervalDuration = animationDuration / objectsToHide.Count;
        foreach (var obj in objectsToHide)
        {
            obj.SetActive(false);
            yield return new WaitForSeconds(intervalDuration);
        }
    }
    
    public static IEnumerator AnimationShowObjects(List<GameObject> objectsToShow, float animationDuration)
    {
        float intervalDuration = animationDuration / objectsToShow.Count;
        foreach (var obj in objectsToShow)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(intervalDuration);
        }
    }
}
