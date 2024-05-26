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
    public float health { get; set; }
    public float happiness { get; set; }
    public float sensibilisation { get; set; }
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
    
    public Stat Devide(int value)
    {
        Stat stat = new Stat();
        stat.constructionCost = constructionCost / value;
        stat.destructionCost = destructionCost / value;
        stat.profits = profits / value;
        stat.losses = losses / value;
        stat.airQuality = airQuality / value;
        stat.groundQuality = groundQuality / value;
        stat.biodiversity = biodiversity / value;
        stat.size = size / value;
        stat.health = health / value;
        stat.happiness = happiness / value;
        stat.sensibilisation = sensibilisation / value;
        stat.energyConstructionCost = energyConstructionCost / value;
        stat.energyDestructionCost = energyDestructionCost / value;
        stat.energyProduction = energyProduction / value;
        stat.energyConsumption = energyConsumption / value;
        return stat;
    }
    
    public void ResetPopulationStats()
    {
        health = 0;
        happiness = 0;
        sensibilisation = 0;
    }
    public void Overwrite(Stat stat)
    {
        // For each stat, if the value of stat is not 0, overwrite the value of the current stat
        if (stat.constructionCost != 0) constructionCost = stat.constructionCost;
        if (stat.destructionCost != 0) destructionCost = stat.destructionCost;
        if (stat.profits != 0) profits = stat.profits;
        if (stat.losses != 0) losses = stat.losses;
        if (stat.airQuality != 0) airQuality = stat.airQuality;
        if (stat.groundQuality != 0) groundQuality = stat.groundQuality;
        if (stat.biodiversity != 0) biodiversity = stat.biodiversity;
        if (stat.size != 0) size = stat.size;
        if (stat.health != 0) health = stat.health;
        if (stat.happiness != 0) happiness = stat.happiness;
        if (stat.sensibilisation != 0) sensibilisation = stat.sensibilisation;
        if (stat.energyConstructionCost != 0) energyConstructionCost = stat.energyConstructionCost;
        if (stat.energyDestructionCost != 0) energyDestructionCost = stat.energyDestructionCost;
        if (stat.energyProduction != 0) energyProduction = stat.energyProduction;
        if (stat.energyConsumption != 0) energyConsumption = stat.energyConsumption;
    }
    
    public void DisplayStats()
    {
        Debug.Log("Construction cost : " + constructionCost);
        Debug.Log("Destruction cost : " + destructionCost);
        Debug.Log("Profits : " + profits);
        Debug.Log("Losses : " + losses);
        Debug.Log("Air quality : " + airQuality);
        Debug.Log("Ground quality : " + groundQuality);
        Debug.Log("Biodiversity : " + biodiversity);
        Debug.Log("Size : " + size);
        Debug.Log("Health : " + health);
        Debug.Log("Happiness : " + happiness);
        Debug.Log("Sensibilisation : " + sensibilisation);
        Debug.Log("Energy construction cost : " + energyConstructionCost);
        Debug.Log("Energy destruction cost : " + energyDestructionCost);
        Debug.Log("Energy production : " + energyProduction);
        Debug.Log("Energy consumption : " + energyConsumption);
    }
}
