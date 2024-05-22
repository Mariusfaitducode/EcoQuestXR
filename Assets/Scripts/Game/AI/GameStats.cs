using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameStats
{
    internal Stat globalStats = new Stat();
    
    internal List<ObjectScript> objects;
    
    public void StatsStartInitialization()
    {
        globalStats.Reset();
    }

    public void ComputeGlobalStats()
    {
        globalStats.Reset();
        foreach (ObjectScript objScript in objects)
        {
            if (objScript.objectProperties != null && objScript.objectProperties.stats != null)
            {
                globalStats.Add(objScript.objectProperties.stats);
            }
            else
            {
                Debug.LogWarning("Object " + objScript.gameObject.name + " doesn't have properties");
            }
            
        }
    }
}
