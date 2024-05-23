using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameStats
{
    internal Stat globalStats = new Stat();
    
    internal int initialMoneyInBank = 0;
    internal int initialEnergyInStock = 0;

    internal int currentMoneyInBank = 0;
    internal int currentEnergyInStock = 0;
    internal int overallEcologyRate = 0;
    internal int overallPopulationAcceptationRate = 0;
    
    public void StatsStartInitialization()
    {
        globalStats.Reset();
        currentMoneyInBank = initialMoneyInBank;
        currentEnergyInStock = initialEnergyInStock;
    }
    
    public void UpdateGlobalStatsFromCard(Card card)
    {
        currentMoneyInBank -= card.actionCost;
        currentEnergyInStock -= card.actionEnergyCost;
    }
    
    public void UpdateGlobalStatsFromObject(List<ObjectScript> objects)
    {
        foreach (ObjectScript objScript in objects)
        {
            if (objScript.objectProperties != null && objScript.objectProperties.stats != null)
            {
                currentMoneyInBank += objScript.objectProperties.stats.profits;
                currentMoneyInBank -= objScript.objectProperties.stats.losses;
                
                currentEnergyInStock += objScript.objectProperties.stats.energyProduction;
                currentEnergyInStock -= objScript.objectProperties.stats.energyConsumption;
            }
            else
            {
                Debug.LogWarning("Object " + objScript.gameObject.name + " doesn't have properties");
            }
        }
    }

    public void UpdateObjectStatsFromObject(List<ObjectScript> objects)
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
