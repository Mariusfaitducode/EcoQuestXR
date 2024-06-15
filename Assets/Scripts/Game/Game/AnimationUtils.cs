using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationUtils
{
    public static IEnumerator AnimationPlaceObjects(GameManager gameManager, List<GameObject> objectsOnMap)
    {
        // Hide new objects
        ObjectUtils.HideObjects(objectsOnMap);

        // Find the center of the all objects
        Vector3 focusPoint = ObjectUtils.CalculateFocusPoint(objectsOnMap);
        
        // Animation of transition focus and zoom
        Transform mapTransform = gameManager.otherObjects.mesh.transform;
        UpdateTerrainRenderer mapUpdateTerrainRenderer = gameManager.otherObjects.mesh.GetComponent<UpdateTerrainRenderer>();
        Vector3 shaderPoint = mapUpdateTerrainRenderer.GetMapCenter();
        
        float startScale = mapTransform.localScale.x;
        float targetScale = 0.03f;
        
        Vector3 startPosition = mapTransform.position;
        Vector3 targetPosition = mapTransform.position + shaderPoint - focusPoint;
        Vector3 targetPositionAfterZoom = MapMouvement.GetPositionFromScaleObjectAroundPoint(targetPosition, shaderPoint, startScale, targetScale);

        float duration = 2f;
        float elapsedTime = 0f;

        // Animation de transition focus and zoom
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            mapTransform.position = Vector3.Lerp(startPosition, targetPositionAfterZoom, t);
            mapTransform.localScale = Vector3.Lerp(Vector3.one * startScale, Vector3.one * targetScale, t);
            
            mapUpdateTerrainRenderer.UpdateMapInformations(false);
            mapUpdateTerrainRenderer.SetObjectsVisibility(gameManager.fillMapManager);
            mapUpdateTerrainRenderer.SetRoadsVisibility(gameManager.fillMapManager);
            ObjectUtils.HideObjects(objectsOnMap);

            yield return null;
        }

        // S'assure que la transition est complétée
        mapTransform.position = targetPositionAfterZoom;
        mapTransform.localScale = Vector3.one * targetScale;

        // Affiche les GameObjects un par un avec un intervalle de 1 seconde
        foreach (var obj in objectsOnMap)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(1f);
        }
        
        // Animation de transition dezoom
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            mapTransform.position = Vector3.Lerp(targetPositionAfterZoom, targetPosition, t);
            mapTransform.localScale = Vector3.Lerp(Vector3.one * targetScale, Vector3.one * startScale, t);
            
            mapUpdateTerrainRenderer.UpdateMapInformations(false);
            mapUpdateTerrainRenderer.SetObjectsVisibility(gameManager.fillMapManager);
            mapUpdateTerrainRenderer.SetRoadsVisibility(gameManager.fillMapManager);

            yield return null;
        }
    }
    public static IEnumerator AnimationRemoveObjects(GameManager gameManager, List<GameObject> objectsOnMap)
    {
        // Find the center of the all objects
        Vector3 focusPoint = ObjectUtils.CalculateFocusPoint(objectsOnMap);
        
        // Animation of transition focus and zoom
        Transform mapTransform = gameManager.otherObjects.mesh.transform;
        UpdateTerrainRenderer mapUpdateTerrainRenderer = gameManager.otherObjects.mesh.GetComponent<UpdateTerrainRenderer>();
        Vector3 shaderPoint = mapUpdateTerrainRenderer.GetMapCenter();
        
        float startScale = mapTransform.localScale.x;
        float targetScale = 0.03f;
        
        Vector3 startPosition = mapTransform.position;
        Vector3 targetPosition = mapTransform.position + shaderPoint - focusPoint;
        Vector3 targetPositionAfterZoom = MapMouvement.GetPositionFromScaleObjectAroundPoint(targetPosition, shaderPoint, startScale, targetScale);

        float duration = 2f;
        float elapsedTime = 0f;

        // Animation de transition focus and zoom
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            mapTransform.position = Vector3.Lerp(startPosition, targetPositionAfterZoom, t);
            mapTransform.localScale = Vector3.Lerp(Vector3.one * startScale, Vector3.one * targetScale, t);
            
            mapUpdateTerrainRenderer.UpdateMapInformations(false);
            mapUpdateTerrainRenderer.SetObjectsVisibility(gameManager.fillMapManager);
            mapUpdateTerrainRenderer.SetRoadsVisibility(gameManager.fillMapManager);

            yield return null;
        }

        // S'assure que la transition est complétée
        mapTransform.position = targetPositionAfterZoom;
        mapTransform.localScale = Vector3.one * targetScale;

        // Cacher les GameObjects un par un avec un intervalle de 1 seconde
        foreach (var obj in objectsOnMap)
        {
            obj.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
        
        // Animation de transition dezoom
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            mapTransform.position = Vector3.Lerp(targetPositionAfterZoom, targetPosition, t);
            mapTransform.localScale = Vector3.Lerp(Vector3.one * targetScale, Vector3.one * startScale, t);
            
            mapUpdateTerrainRenderer.UpdateMapInformations(false);
            mapUpdateTerrainRenderer.SetObjectsVisibility(gameManager.fillMapManager);
            mapUpdateTerrainRenderer.SetRoadsVisibility(gameManager.fillMapManager);
            ObjectUtils.HideObjects(objectsOnMap);

            yield return null;
        }
        
        // S'assure que la transition est complétée
        mapTransform.position = targetPosition;
        mapTransform.localScale = Vector3.one * startScale;
        
        // Destroy the objects
        ObjectUtils.DestroyObjects(objectsOnMap);
    }
    public static IEnumerator AnimationUpgradeObjects(GameManager gameManager, List<GameObject> removedObjects, List<GameObject> placedObjects)
    {
        // Hide new objects
        ObjectUtils.HideObjects(placedObjects);
        
        // Find the center of the all objects
        Vector3 focusRemovePoint = ObjectUtils.CalculateFocusPoint(removedObjects);
        Vector3 focusPlacePoint = ObjectUtils.CalculateFocusPoint(placedObjects);
        
        // Animation of transition focus and zoom
        Transform mapTransform = gameManager.otherObjects.mesh.transform;
        UpdateTerrainRenderer mapUpdateTerrainRenderer = gameManager.otherObjects.mesh.GetComponent<UpdateTerrainRenderer>();
        Vector3 shaderPoint = mapUpdateTerrainRenderer.GetMapCenter();
        
        float startScale = mapTransform.localScale.x;
        float targetScale = 0.03f;
        
        Vector3 startPosition = mapTransform.position;
        Vector3 targetRemovePosition = mapTransform.position + shaderPoint - focusRemovePoint;
        Vector3 targetRemovePositionAfterZoom = MapMouvement.GetPositionFromScaleObjectAroundPoint(targetRemovePosition, shaderPoint, startScale, targetScale);
        Vector3 targetPlacePosition = targetRemovePosition + focusRemovePoint - focusPlacePoint;
        Vector3 targetPlacePositionAfterZoom = MapMouvement.GetPositionFromScaleObjectAroundPoint(targetPlacePosition, shaderPoint, startScale, targetScale);

        float duration = 2f;
        float elapsedTime = 0f;

        // Animation de transition focus and zoom on remove objects
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            mapTransform.position = Vector3.Lerp(startPosition, targetRemovePositionAfterZoom, t);
            mapTransform.localScale = Vector3.Lerp(Vector3.one * startScale, Vector3.one * targetScale, t);
            
            mapUpdateTerrainRenderer.UpdateMapInformations(false);
            mapUpdateTerrainRenderer.SetObjectsVisibility(gameManager.fillMapManager);
            mapUpdateTerrainRenderer.SetRoadsVisibility(gameManager.fillMapManager);
            ObjectUtils.HideObjects(placedObjects);

            yield return null;
        }

        // S'assure que la transition est complétée
        mapTransform.position = targetRemovePositionAfterZoom;
        mapTransform.localScale = Vector3.one * targetScale;

        // Cacher les GameObjects un par un avec un intervalle de 1 seconde
        foreach (var obj in removedObjects)
        {
            obj.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
        
        elapsedTime = 0f;

        // Animation de transition focus on placed objects
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            mapTransform.position = Vector3.Lerp(targetRemovePositionAfterZoom, targetPlacePositionAfterZoom, t);
            
            mapUpdateTerrainRenderer.UpdateMapInformations(false);
            mapUpdateTerrainRenderer.SetObjectsVisibility(gameManager.fillMapManager);
            mapUpdateTerrainRenderer.SetRoadsVisibility(gameManager.fillMapManager);
            ObjectUtils.HideObjects(removedObjects);
            ObjectUtils.HideObjects(placedObjects);

            yield return null;
        }

        // S'assure que la transition est complétée
        mapTransform.position = targetPlacePositionAfterZoom;

        // Show les GameObjects un par un avec un intervalle de 1 seconde
        foreach (var obj in placedObjects)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(1f);
        }
        
        // Animation de transition dezoom
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            mapTransform.position = Vector3.Lerp(targetPlacePositionAfterZoom, targetPlacePosition, t);
            mapTransform.localScale = Vector3.Lerp(Vector3.one * targetScale, Vector3.one * startScale, t);
            
            mapUpdateTerrainRenderer.UpdateMapInformations(false);
            mapUpdateTerrainRenderer.SetObjectsVisibility(gameManager.fillMapManager);
            mapUpdateTerrainRenderer.SetRoadsVisibility(gameManager.fillMapManager);
            ObjectUtils.HideObjects(removedObjects);

            yield return null;
        }
        
        // S'assure que la transition est complétée
        mapTransform.position = targetPlacePosition;
        mapTransform.localScale = Vector3.one * startScale;
        
        // Destroy the objects
        ObjectUtils.DestroyObjects(removedObjects);
    }
}
