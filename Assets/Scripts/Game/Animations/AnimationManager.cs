using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioName {
    Construction,
    Destruction
}

[System.Serializable]
public struct AudioEvent
{
    public AudioName name;
    public AudioClip clip;
}

public class AnimationManager : MonoBehaviour
{
    internal GameManager gameManager;
    
    // Visual elements
    private Transform mapTransform;
    private UpdateTerrainRenderer mapUpdateTerrainRenderer;
    
    // Sound elements
    public List<AudioEvent> eventsAudioClips;
    public AudioSource audioSource;
    
    public void AnimationsStartInitialization()
    {
        mapTransform = gameManager.otherObjects.mesh.transform;
        mapUpdateTerrainRenderer = gameManager.otherObjects.mesh.GetComponent<UpdateTerrainRenderer>();
    }
    
    public float PlayAudioClipByName(AudioName audioName)
    {
        float clipLength = 0;
        foreach (AudioEvent audioEvent in eventsAudioClips)
        {
            if (audioEvent.name == audioName)
            {
                if (audioEvent.clip != null)
                {
                    audioSource.PlayOneShot(audioEvent.clip);
                    clipLength = audioEvent.clip.length;
                }
            }
        }
        if (clipLength == 0)
            Debug.LogError("Audio clip not found for " + audioName);
        
        return clipLength;
    }
    
    public IEnumerator AnimationPlaceObjects(List<GameObject> placedObjects)
    {
        // Hide new objects
        ObjectUtils.HideObjects(placedObjects);

        // Find the center of the all objects
        Vector3 focusPoint = ObjectUtils.CalculateFocusPoint(placedObjects);
        
        // Get recurent variables from gameManager
        Vector3 shaderPoint = mapUpdateTerrainRenderer.GetMapCenter();
        
        // ==== Informations for the animation ====
        
        // Scales
        float startScale = mapTransform.localScale.x;
        float targetScale = 0.03f;
        // Positions
        Debug.Log(mapTransform.position);
        Vector3 startPosition = mapTransform.position;
        Vector3 targetPosition = mapTransform.position + shaderPoint - focusPoint;
        targetPosition = new Vector3(targetPosition.x, mapTransform.position.y, targetPosition.z);
        Vector3 targetPositionAfterZoom = MapMouvement.GetPositionFromScaleObjectAroundPoint(targetPosition, shaderPoint, startScale, targetScale);
        // Durations
        float durationFocus = 2f;
        float durationDezoom = 1.5f;
        
        // ==== Animation ====
        
        // Animation de transition focus and zoom
        yield return StartCoroutine(
            AnimationUtils.AnimationMapTransition(
                mapTransform,
                mapUpdateTerrainRenderer,
                gameManager.fillMapManager,
                durationFocus,
                startPosition,
                targetPositionAfterZoom,
                startScale,
                targetScale,
                true,
                placedObjects));
        
        // Show les GameObjects un par un avec un intervalle de 1 seconde
        float durationShow = PlayAudioClipByName(AudioName.Construction);
        yield return StartCoroutine(AnimationUtils.AnimationShowObjects(placedObjects, durationShow));
        
        // Animation de transition dezoom
        yield return StartCoroutine(
            AnimationUtils.AnimationMapTransition(
                mapTransform,
                mapUpdateTerrainRenderer,
                gameManager.fillMapManager,
                durationDezoom,
                targetPositionAfterZoom,
                targetPosition,
                targetScale,
                startScale));
    }
    public IEnumerator AnimationRemoveObjects(List<GameObject> removedObjects)
    {
        // Find the center of the all objects
        Vector3 focusPoint = ObjectUtils.CalculateFocusPoint(removedObjects);
        
        // Get recurent variables from gameManager
        Vector3 shaderPoint = mapUpdateTerrainRenderer.GetMapCenter();
        
        // ==== Informations for the animation ====
        
        // Scales
        float startScale = mapTransform.localScale.x;
        float targetScale = 0.03f;
        // Positions
        Vector3 startPosition = mapTransform.position;
        Vector3 targetPosition = mapTransform.position + shaderPoint - focusPoint;
        targetPosition = new Vector3(targetPosition.x, mapTransform.position.y, targetPosition.z);
        Vector3 targetPositionAfterZoom = MapMouvement.GetPositionFromScaleObjectAroundPoint(targetPosition, shaderPoint, startScale, targetScale);
        // Durations
        float durationFocus = 2f;
        float durationDezoom = 1.5f;
        
        // ==== Animation ====
        
        // Animation de transition focus and zoom
        yield return StartCoroutine(
            AnimationUtils.AnimationMapTransition(
                mapTransform,
                mapUpdateTerrainRenderer,
                gameManager.fillMapManager,
                durationFocus,
                startPosition,
                targetPositionAfterZoom,
                startScale,
                targetScale));
        
        // Cacher les GameObjects un par un avec un intervalle de 1 seconde
        float durationShow = PlayAudioClipByName(AudioName.Destruction);
        yield return StartCoroutine(AnimationUtils.AnimationHideObjects(removedObjects, durationShow));
        
        // Destroy the objects
        ObjectUtils.DestroyObjects(removedObjects);
        
        // Animation de transition dezoom
        yield return StartCoroutine(
            AnimationUtils.AnimationMapTransition(
                mapTransform,
                mapUpdateTerrainRenderer,
                gameManager.fillMapManager,
                durationDezoom,
                targetPositionAfterZoom,
                targetPosition,
                targetScale,
                startScale,
                true));
    }
    public IEnumerator AnimationReplacementObjects(List<GameObject> removedObjects, List<GameObject> placedObjects)
    {
        // Hide new objects
        ObjectUtils.HideObjects(placedObjects);
        
        // Find the center of the all objects
        Vector3 focusRemovePoint = ObjectUtils.CalculateFocusPoint(removedObjects);
        Vector3 focusPlacePoint = ObjectUtils.CalculateFocusPoint(placedObjects);
        
        // Get recurent variables from gameManager
        Vector3 shaderPoint = mapUpdateTerrainRenderer.GetMapCenter();
        
        // ==== Informations for the animation ====
        
        // Scales
        float startScale = mapTransform.localScale.x;
        float targetScale = 0.03f;
        // Positions
        Vector3 startPosition = mapTransform.position;
        Vector3 targetRemovePosition = mapTransform.position + shaderPoint - focusRemovePoint;
        targetRemovePosition = new Vector3(targetRemovePosition.x, mapTransform.position.y, targetRemovePosition.z);
        Vector3 targetRemovePositionAfterZoom = MapMouvement.GetPositionFromScaleObjectAroundPoint(targetRemovePosition, shaderPoint, startScale, targetScale);
        Vector3 targetPlacePosition = targetRemovePosition + focusRemovePoint - focusPlacePoint;
        targetPlacePosition = new Vector3(targetPlacePosition.x, targetRemovePosition.y, targetPlacePosition.z);
        Vector3 targetPlacePositionAfterZoom = MapMouvement.GetPositionFromScaleObjectAroundPoint(targetPlacePosition, shaderPoint, startScale, targetScale);
        // Durations
        float durationFocus = 2f;
        float durationTransition = 1.5f;
        float durationDezoom = 1.5f;
        
        // ==== Animation ====
        
        // Animation de transition focus and zoom on remove objects
        yield return StartCoroutine(
            AnimationUtils.AnimationMapTransition(
                mapTransform,
                mapUpdateTerrainRenderer,
                gameManager.fillMapManager,
                durationFocus,
                startPosition,
                targetRemovePositionAfterZoom,
                startScale,
                targetScale,
                true));
        
        // Cacher les GameObjects un par un avec un intervalle de 1 seconde
        float durationHide = PlayAudioClipByName(AudioName.Destruction);
        yield return StartCoroutine(AnimationUtils.AnimationHideObjects(removedObjects, durationHide));
        
        // Destroy the objects to remove we hided just above 
        ObjectUtils.DestroyObjects(removedObjects);
        
        // Animation de transition focus on placed objects
        yield return StartCoroutine(
            AnimationUtils.AnimationMapTransition(
                mapTransform,
                mapUpdateTerrainRenderer,
                gameManager.fillMapManager,
                durationTransition,
                targetRemovePositionAfterZoom,
                targetPlacePositionAfterZoom,
                targetScale,
                targetScale,
                false));
        
        // Show les GameObjects un par un avec un intervalle de 1 seconde
        float durationShow = PlayAudioClipByName(AudioName.Construction);
        yield return StartCoroutine(AnimationUtils.AnimationShowObjects(placedObjects, durationShow));
        
        // Animation de transition dezoom
        yield return StartCoroutine(
            AnimationUtils.AnimationMapTransition(
                mapTransform,
                mapUpdateTerrainRenderer,
                gameManager.fillMapManager,
                durationDezoom,
                targetPlacePositionAfterZoom,
                targetPlacePosition,
                targetScale,
                startScale));
    }
    
    
   public IEnumerator AnimationUpgradeObjects(List<GameObject> placedObjects)
    {
        // Hide new objects
        ObjectUtils.ShowObjects(placedObjects);

        // Find the center of the all objects
        Vector3 focusPoint = ObjectUtils.CalculateFocusPoint(placedObjects);
        
        // Get recurent variables from gameManager
        Vector3 shaderPoint = mapUpdateTerrainRenderer.GetMapCenter();
        
        // ==== Informations for the animation ====
        
        // Scales
        float startScale = mapTransform.localScale.x;
        float targetScale = 0.03f;
        // Positions
        Debug.Log(mapTransform.position);
        Vector3 startPosition = mapTransform.position;
        Vector3 targetPosition = mapTransform.position + shaderPoint - focusPoint;
        targetPosition = new Vector3(targetPosition.x, mapTransform.position.y, targetPosition.z);
        Vector3 targetPositionAfterZoom = MapMouvement.GetPositionFromScaleObjectAroundPoint(targetPosition, shaderPoint, startScale, targetScale);
        // Durations
        float durationFocus = 2f;
        float durationDezoom = 1.5f;
        
        // ==== Animation ====
        
        // Animation de transition focus and zoom
        yield return StartCoroutine(
            AnimationUtils.AnimationMapTransition(
                mapTransform,
                mapUpdateTerrainRenderer,
                gameManager.fillMapManager,
                durationFocus,
                startPosition,
                targetPositionAfterZoom,
                startScale,
                targetScale,
                true,
                placedObjects));
        
        // Show les GameObjects un par un avec un intervalle de 1 seconde
        float durationShow = PlayAudioClipByName(AudioName.Construction);
        yield return StartCoroutine(AnimationUtils.AnimationShowObjects(placedObjects, durationShow));
        
        // Animation de transition dezoom
        yield return StartCoroutine(
            AnimationUtils.AnimationMapTransition(
                mapTransform,
                mapUpdateTerrainRenderer,
                gameManager.fillMapManager,
                durationDezoom,
                targetPositionAfterZoom,
                targetPosition,
                targetScale,
                startScale));
    }
   
   public IEnumerator AnimationDemonstrationObjects()
    {
        // Find the center of the all objects
        
        
        // Get recurent variables from gameManager
        Vector3 shaderPoint = mapUpdateTerrainRenderer.GetMapCenter();
        
        // ==== Informations for the animation ====
        
        // Scales
        float startScale = mapTransform.localScale.x;
        float targetScale = 0.03f;
        float targetdezoomScale = 0.002f;
        // Positions
        Debug.Log(mapTransform.position);
        Vector3 startPosition = mapTransform.position;
        Vector3 targetPosition = mapTransform.position;
        targetPosition = new Vector3(targetPosition.x, mapTransform.position.y, targetPosition.z);
        Vector3 targetPositionAfterDeZoom = MapMouvement.GetPositionFromScaleObjectAroundPoint(targetPosition, shaderPoint, startScale, targetdezoomScale);
        // Durations
        float durationDezoom = 1.5f;
        float durationFocus = 2f;
        float durationTransition = 10f;
        
        
        // ==== Animation ====
        
        // Animation de transition dezoom
        yield return StartCoroutine(
            AnimationUtils.AnimationMapTransition(
                mapTransform,
                mapUpdateTerrainRenderer,
                gameManager.fillMapManager,
                durationDezoom,
                startPosition,
                targetPositionAfterDeZoom,
                startScale,
                targetdezoomScale,
                true));

        DemonstrationGestion demonstrationGestion = GameObject.FindObjectOfType<DemonstrationGestion>();
        GameObject place = demonstrationGestion.gameObject;
        demonstrationGestion.StartDemonstration(30);
        
        Vector3 targetPositionPlace = mapTransform.position + shaderPoint - place.transform.position;
        targetPositionPlace = new Vector3(targetPositionPlace.x, targetPosition.y, targetPositionPlace.z);
        Vector3 targetPositionPlaceAfterZoom = MapMouvement.GetPositionFromScaleObjectAroundPoint(targetPositionPlace, shaderPoint, targetdezoomScale, targetScale);
        
        // Animation de transition focus and zoom
        yield return StartCoroutine(
            AnimationUtils.AnimationMapTransition(
                mapTransform,
                mapUpdateTerrainRenderer,
                gameManager.fillMapManager,
                durationFocus,
                targetPositionAfterDeZoom,
                targetPositionPlaceAfterZoom,
                targetdezoomScale,
                targetScale,
                true));
        
        yield return new WaitForSeconds(durationTransition);
        gameManager.settingsController.end_game();


    }
}
