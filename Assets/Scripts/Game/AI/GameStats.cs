using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        foreach (Stat stat in objects.Select(obj => obj.objectProperties.stats))
        {
            globalStats.price += stat.price;
            globalStats.ecology += stat.ecology;
            globalStats.population += stat.population;
            globalStats.energy += stat.energy;
            globalStats.pollution += stat.pollution;
        }
    }
}
