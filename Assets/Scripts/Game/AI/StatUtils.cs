using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatUtils
{
    public static string ConvertNumberToText(int number)
    {
        if (number < 1000)
        {
            return number.ToString();
        }
        else if (number < 1000000)
        {
            return (number / 1000).ToString() + "K";
        }
        else if (number < 1000000000)
        {
            return (number / 1000000).ToString() + "M";
        }
        else
        {
            return (number / 1000000000).ToString() + "B";
        }
    }
    
    public static Stat GetStatFromConstruction(Stat stat, int multiplier)
    {
        Stat newStat = new Stat();
        newStat.constructionCost = stat.constructionCost * multiplier;
        newStat.destructionCost = stat.destructionCost * multiplier;
        newStat.profits = stat.profits * multiplier;
        newStat.losses = stat.losses * multiplier;
        newStat.airQuality = stat.airQuality * multiplier;
        newStat.groundQuality = stat.groundQuality * multiplier;
        newStat.biodiversity = stat.biodiversity * multiplier;
        newStat.size = stat.size * multiplier;
        newStat.health = stat.health * multiplier;
        newStat.happiness = stat.happiness * multiplier;
        newStat.sensibilisation = stat.sensibilisation * multiplier;
        newStat.energyConstructionCost = stat.energyConstructionCost * multiplier;
        newStat.energyDestructionCost = stat.energyDestructionCost * multiplier;
        newStat.energyProduction = stat.energyProduction * multiplier;
        newStat.energyConsumption = stat.energyConsumption * multiplier;
        return newStat;
    }
    
    public static Stat GetStatFromDestruction(Stat stat, int multiplier)
    {
        Stat newStat = new Stat();
        newStat.constructionCost = stat.constructionCost * multiplier;
        newStat.destructionCost = stat.destructionCost * multiplier;
        newStat.profits = - stat.profits * multiplier;
        newStat.losses = - stat.losses * multiplier;
        newStat.airQuality = - stat.airQuality * multiplier;
        newStat.groundQuality = - stat.groundQuality * multiplier;
        newStat.biodiversity = - stat.biodiversity * multiplier;
        newStat.size = - stat.size * multiplier;
        newStat.health = - stat.health * multiplier;
        newStat.happiness = - stat.happiness * multiplier;
        newStat.sensibilisation = - stat.sensibilisation * multiplier;
        newStat.energyConstructionCost = stat.energyConstructionCost * multiplier;
        newStat.energyDestructionCost = stat.energyDestructionCost * multiplier;
        newStat.energyProduction = - stat.energyProduction * multiplier;
        newStat.energyConsumption = - stat.energyConsumption * multiplier;
        return newStat;
    }

    public static Stat GetStatFromUpgrade(Stat stat1, Stat stat2, int multiplier1, int multiplier2)
    {
        Stat newStat = new Stat();
        
        newStat.constructionCost = stat2.constructionCost * multiplier2;
        newStat.destructionCost = stat1.destructionCost * multiplier1;
        newStat.energyConstructionCost = stat2.energyConstructionCost * multiplier2;
        newStat.energyDestructionCost = stat1.energyDestructionCost * multiplier1;
        
        newStat.profits = stat2.profits * multiplier2 - stat1.profits * multiplier1;
        newStat.losses = stat2.losses * multiplier2 - stat1.losses * multiplier1;
        newStat.airQuality = stat2.airQuality * multiplier2 - stat1.airQuality * multiplier1;
        newStat.groundQuality = stat2.groundQuality * multiplier2 - stat1.groundQuality * multiplier1;
        newStat.biodiversity = stat2.biodiversity * multiplier2 - stat1.biodiversity * multiplier1;
        newStat.size = stat2.size * multiplier2 - stat1.size * multiplier1;
        newStat.health = stat2.health * multiplier2 - stat1.health * multiplier1;
        newStat.happiness = stat2.happiness * multiplier2 - stat1.happiness * multiplier1;
        newStat.sensibilisation = stat2.sensibilisation * multiplier2 - stat1.sensibilisation * multiplier1;
        newStat.energyProduction = stat2.energyProduction * multiplier2 - stat1.energyProduction * multiplier1;
        newStat.energyConsumption = stat2.energyConsumption * multiplier2 - stat1.energyConsumption * multiplier1;
        
        return newStat;
    }
}
