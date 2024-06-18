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
        
        if (modelGestion!= null && objectProperties.subObjects.Count > 0)
        {
            foreach (SubObject subObject in objectProperties.subObjects)
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

    public bool canUpgrade(bool upgrade, ObjectProperties subObjectProperties2)
    {
        
        SubModel subModel = modelGestion.FindSubModelWithName(subObjectProperties2.prefabName);
        
        if (upgrade)
        {
            return !subModel.gameObject.activeSelf;
        }
        else
        {
            return subModel.gameObject.activeSelf;
        }
    }

    public GameObject UpgradeObject(ObjectProperties subObjectProperties2, bool upgrade)
    {
        Debug.Log(subObjectProperties2.prefabName);
        
        SubModel subModel = modelGestion.FindSubModelWithName(subObjectProperties2.prefabName);
        
        if (upgrade)
        {
            subModel.gameObject.SetActive(true);
        }
        else
        {
            subModel.gameObject.SetActive(false);
        }
        return subModel.gameObject;
    }
    
    public int GetMonthsSinceConstruction()
    {
        TimeSpan timeSpan = gameManager.timer.currentTime - constructionTime;
        return (int)(timeSpan.Days / 30);
    }

    public void upgradeStats(int idSubObject)
    {
        foreach (SubObject subObject in objectProperties.subObjects)
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
        foreach (SubObject subObject in objectProperties.subObjects)
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
