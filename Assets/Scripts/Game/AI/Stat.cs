using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum statType
{
    constructionCost,
    destructionCost,
    profits,
    losses,
    airQuality,
    groundQuality,
    biodiversity,
    size,
    health,
    happiness,
    sensibilisation,
    energyConstructionCost,
    energyDestructionCost,
    energyProduction,
    energyConsumption
}

public class Stat
{
    public int constructionCost { get; set; }
    public int destructionCost { get; set; }
    public int profits { get; set; }
    public int losses { get; set; }
    public int airQuality { get; set; }
    public int groundQuality { get; set; }
    public int biodiversity { get; set; }
    public int size { get; set; }
    public int health { get; set; }
    public int happiness { get; set; }
    public int sensibilisation { get; set; }
    public int energyConstructionCost { get; set; }
    public int energyDestructionCost { get; set; }
    public int energyProduction { get; set; }
    public int energyConsumption { get; set; }

    public void Reset()
    {
        constructionCost = 0;
        destructionCost = 0;
        profits = 0;
        losses = 0;
        airQuality = 0;
        groundQuality = 0;
        biodiversity = 0;
        size = 0;
        health = 0;
        happiness = 0;
        sensibilisation = 0;
        energyConstructionCost = 0;
        energyDestructionCost = 0;
        energyProduction = 0;
        energyConsumption = 0;
    }
    public void Add(Stat stat)
    {
        constructionCost += stat.constructionCost;
        destructionCost += stat.destructionCost;
        profits += stat.profits;
        losses += stat.losses;
        airQuality += stat.airQuality;
        groundQuality += stat.groundQuality;
        biodiversity += stat.biodiversity;
        size += stat.size;
        health += stat.health;
        happiness += stat.happiness;
        sensibilisation += stat.sensibilisation;
        energyConstructionCost += stat.energyConstructionCost;
        energyDestructionCost += stat.energyDestructionCost;
        energyProduction += stat.energyProduction;
        energyConsumption += stat.energyConsumption;
    }
    public string GetStatString(statType statType)
    {
        switch (statType)
        {
            case statType.constructionCost:
                return StatUtils.ConvertNumberToText(constructionCost);
            case statType.destructionCost:
                return StatUtils.ConvertNumberToText(destructionCost);
            case statType.profits:
                return StatUtils.ConvertNumberToText(profits);
            case statType.losses:
                return StatUtils.ConvertNumberToText(losses);
            case statType.airQuality:
                return StatUtils.ConvertNumberToText(airQuality);
            case statType.groundQuality:
                return StatUtils.ConvertNumberToText(groundQuality);
            case statType.biodiversity:
                return StatUtils.ConvertNumberToText(biodiversity);
            case statType.size:
                return StatUtils.ConvertNumberToText(size);
            case statType.health:
                return StatUtils.ConvertNumberToText(health);
            case statType.happiness:
                return StatUtils.ConvertNumberToText(happiness);
            case statType.sensibilisation:
                return StatUtils.ConvertNumberToText(sensibilisation);
            case statType.energyConstructionCost:
                return StatUtils.ConvertNumberToText(energyConstructionCost);
            case statType.energyDestructionCost:
                return StatUtils.ConvertNumberToText(energyDestructionCost);
            case statType.energyProduction:
                return StatUtils.ConvertNumberToText(energyProduction);
            case statType.energyConsumption:
                return StatUtils.ConvertNumberToText(energyConsumption);
            default:
                return "Error: statType not found";
        }
    }
    
}
