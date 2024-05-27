using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectScript : MonoBehaviour
{
    // All datas from CSV
    internal ObjectProperties objectProperties;
    
    // All datas from Game
    internal GameManager gameManager;
    
    // Init on Place building
    internal AreaPrefab areaPrefab;
    internal List<AreaCell> areaCells;
    
    public DateTime constructionTime;
    
    internal ModelGestion modelGestion;
    
    
    
    public void InitObjectScript(ObjectProperties objectProperties, GameManager gameManager)
    {
        this.objectProperties = objectProperties;
        this.gameManager = gameManager;
        this.constructionTime = gameManager.timer.currentTime;
        
        // Init modelGestion
        
        modelGestion = GetComponent<ModelGestion>();
        
        
        
        
        // Init subModels
        
        if (modelGestion != null && objectProperties.subObjects.Count > 0)
        {
            foreach (SubObjects subObject in objectProperties.subObjects)
            {
                SubModel subModel = modelGestion.FindSubModelWithName(subObject.prefabName);
                
                if (subModel.gameObject!= null)
                {
                    if (Random.Range(0f, 1f) < subModel.initiationProbability)
                    {
                        objectProperties.stats.Add(subObject.stats);
                        
                        subModel.gameObject.SetActive(true);
                    }
                    else
                    {
                        subModel.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
    
    public int GetMonthsSinceConstruction()
    {
        TimeSpan timeSpan = gameManager.timer.currentTime - constructionTime;
        return (int)(timeSpan.Days / 30);
    }

    public void upgradeStats(int idSubObject)
    {
        foreach (SubObjects subObject in objectProperties.subObjects)
        {
             if (subObject.id == idSubObject)
             {
                 objectProperties.stats.Add(subObject.stats);
                 return;
             }
        }
        Debug.LogError("SubObject not found");
    }
    
    public void downgradeStats(int idSubObject)
    {
        foreach (SubObjects subObject in objectProperties.subObjects)
        {
             if (subObject.id == idSubObject)
             {
                 objectProperties.stats.Substract(subObject.stats);
                 return;
             }
        }
        Debug.LogError("SubObject not found");
    }
    
    
}
