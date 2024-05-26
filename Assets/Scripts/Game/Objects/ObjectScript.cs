using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    
    internal ObjectProperties objectProperties;
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
}
