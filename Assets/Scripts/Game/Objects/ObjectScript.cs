using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    
    
    public void InitObjectScript(ObjectProperties objectProperties, GameManager gameManager)
    {
        this.objectProperties = objectProperties;
        this.gameManager = gameManager;
        this.constructionTime = gameManager.timer.currentTime;
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
