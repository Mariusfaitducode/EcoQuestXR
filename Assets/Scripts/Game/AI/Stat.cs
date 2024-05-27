using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum statType
{
    constructionCost,
    destructionCost,
    profitsPerMonth,
    lossesPerMonth,
    constructionCo2Emission,
    destructionCo2Emission,
    co2EmissionPerMonth,
    co2AbsorptionPerMonth,
    constructionWasteProduction,
    destructionWasteProduction,
    wasteProductionPerMonth,
    wasteDestructionPerMonth,
    greenSpaces,
    PopulationSize,
    health,
    happiness,
    sensibilisation,
    energyConstructionCost,
    energyDestructionCost,
    energyProductionPerMonth,
    energyConsumptionPerMonth
}

public class Stat
{
    public float constructionCost { get; set; }
    public float destructionCost { get; set; }
    public float profitsPerMonth { get; set; }
    public float lossesPerMonth { get; set; }
    public float constructionCo2Emission { get; set; }
    public float destructionCo2Emission { get; set; }
    public float co2EmissionPerMonth { get; set; }
    public float co2AbsorptionPerMonth { get; set; }
    public float constructionWasteProduction { get; set; }
    public float destructionWasteProduction { get; set; }
    public float wasteProductionPerMonth { get; set; }
    public float wasteDestructionPerMonth { get; set; }
    public int greenSpaces { get; set; }
    public int populationSize { get; set; }
    public float health { get; set; }
    public float happiness { get; set; }
    public float sensibilisation { get; set; }
    public float energyConstructionCost { get; set; }
    public float energyDestructionCost { get; set; }
    public float energyProductionPerMonth { get; set; }
    public float energyConsumptionPerMonth { get; set; }

    public void Reset()
    {
        constructionCost = 0;
        destructionCost = 0;
        profitsPerMonth = 0;
        lossesPerMonth = 0;
        constructionCo2Emission = 0;
        destructionCo2Emission = 0;
        co2EmissionPerMonth = 0;
        co2AbsorptionPerMonth = 0;
        constructionWasteProduction = 0;
        destructionWasteProduction = 0;
        wasteProductionPerMonth = 0;
        wasteDestructionPerMonth = 0;
        greenSpaces = 0;
        populationSize = 0;
        health = 0;
        happiness = 0;
        sensibilisation = 0;
        energyConstructionCost = 0;
        energyDestructionCost = 0;
        energyProductionPerMonth = 0;
        energyConsumptionPerMonth = 0;
    }

    public void Add(Stat stat)
    {
        constructionCost += stat.constructionCost;
        destructionCost += stat.destructionCost;
        profitsPerMonth += stat.profitsPerMonth;
        lossesPerMonth += stat.lossesPerMonth;
        constructionCo2Emission += stat.constructionCo2Emission;
        destructionCo2Emission += stat.destructionCo2Emission;
        co2EmissionPerMonth += stat.co2EmissionPerMonth;
        co2AbsorptionPerMonth += stat.co2AbsorptionPerMonth;
        constructionWasteProduction += stat.constructionWasteProduction;
        destructionWasteProduction += stat.destructionWasteProduction;
        wasteProductionPerMonth += stat.wasteProductionPerMonth;
        wasteDestructionPerMonth += stat.wasteDestructionPerMonth;
        greenSpaces += stat.greenSpaces;
        populationSize += stat.populationSize;
        health += stat.health;
        happiness += stat.happiness;
        sensibilisation += stat.sensibilisation;
        energyConstructionCost += stat.energyConstructionCost;
        energyDestructionCost += stat.energyDestructionCost;
        energyProductionPerMonth += stat.energyProductionPerMonth;
        energyConsumptionPerMonth += stat.energyConsumptionPerMonth;
    }
    
    public void Substract(Stat stat)
    {
        constructionCost -= stat.constructionCost;
        destructionCost -= stat.destructionCost;
        profitsPerMonth -= stat.profitsPerMonth;
        lossesPerMonth -= stat.lossesPerMonth;
        constructionCo2Emission -= stat.constructionCo2Emission;
        destructionCo2Emission -= stat.destructionCo2Emission;
        co2EmissionPerMonth -= stat.co2EmissionPerMonth;
        co2AbsorptionPerMonth -= stat.co2AbsorptionPerMonth;
        constructionWasteProduction -= stat.constructionWasteProduction;
        destructionWasteProduction -= stat.destructionWasteProduction;
        wasteProductionPerMonth -= stat.wasteProductionPerMonth;
        wasteDestructionPerMonth -= stat.wasteDestructionPerMonth;
        greenSpaces -= stat.greenSpaces;
        populationSize -= stat.populationSize;
        health -= stat.health;
        happiness -= stat.happiness;
        sensibilisation -= stat.sensibilisation;
        energyConstructionCost -= stat.energyConstructionCost;
        energyDestructionCost -= stat.energyDestructionCost;
        energyProductionPerMonth -= stat.energyProductionPerMonth;
        energyConsumptionPerMonth -= stat.energyConsumptionPerMonth;
    }
    
    public Stat Multiply(int value)
    {
        Stat stat = new Stat();
        stat.constructionCost = constructionCost * value;
        stat.destructionCost = destructionCost * value;
        stat.profitsPerMonth = profitsPerMonth * value;
        stat.lossesPerMonth = lossesPerMonth * value;
        stat.constructionCo2Emission = constructionCo2Emission * value;
        stat.destructionCo2Emission = destructionCo2Emission * value;
        stat.co2EmissionPerMonth = co2EmissionPerMonth * value;
        stat.co2AbsorptionPerMonth = co2AbsorptionPerMonth * value;
        stat.constructionWasteProduction = constructionWasteProduction * value;
        stat.destructionWasteProduction = destructionWasteProduction * value;
        stat.wasteProductionPerMonth = wasteProductionPerMonth * value;
        stat.wasteDestructionPerMonth = wasteDestructionPerMonth * value;
        stat.greenSpaces = greenSpaces * value;
        stat.populationSize = populationSize * value;
        stat.health = health * value;
        stat.happiness = happiness * value;
        stat.sensibilisation = sensibilisation * value;
        stat.energyConstructionCost = energyConstructionCost * value;
        stat.energyDestructionCost = energyDestructionCost * value;
        stat.energyProductionPerMonth = energyProductionPerMonth * value;
        stat.energyConsumptionPerMonth = energyConsumptionPerMonth * value;
        return stat;
    }

    public Stat Devide(int value)
    {
        Stat stat = new Stat();
        stat.constructionCost = constructionCost / value;
        stat.destructionCost = destructionCost / value;
        stat.profitsPerMonth = profitsPerMonth / value;
        stat.lossesPerMonth = lossesPerMonth / value;
        stat.constructionCo2Emission = constructionCo2Emission / value;
        stat.destructionCo2Emission = destructionCo2Emission / value;
        stat.co2EmissionPerMonth = co2EmissionPerMonth / value;
        stat.co2AbsorptionPerMonth = co2AbsorptionPerMonth / value;
        stat.constructionWasteProduction = constructionWasteProduction / value;
        stat.destructionWasteProduction = destructionWasteProduction / value;
        stat.wasteProductionPerMonth = wasteProductionPerMonth / value;
        stat.wasteDestructionPerMonth = wasteDestructionPerMonth / value;
        stat.greenSpaces = greenSpaces / value;
        stat.populationSize = populationSize / value;
        stat.health = health / value;
        stat.happiness = happiness / value;
        stat.sensibilisation = sensibilisation / value;
        stat.energyConstructionCost = energyConstructionCost / value;
        stat.energyDestructionCost = energyDestructionCost / value;
        stat.energyProductionPerMonth = energyProductionPerMonth / value;
        stat.energyConsumptionPerMonth = energyConsumptionPerMonth / value;
        return stat;
    }

    public void ResetPopulationStats()
    {
        health = 0;
        happiness = 0;
        sensibilisation = 0;
    }
    
    public void ResetConstructionDestructionStats()
    {
        constructionCost = 0;
        destructionCost = 0;
        constructionCo2Emission = 0;
        destructionCo2Emission = 0;
        constructionWasteProduction = 0;
        destructionWasteProduction = 0;
        energyConstructionCost = 0;
        energyDestructionCost = 0;
    }

    public void Overwrite(Stat stat)
    {
        // For each stat, if the value of stat is not 0, overwrite the value of the current stat
        if (stat.constructionCost != 0) constructionCost = stat.constructionCost;
        if (stat.destructionCost != 0) destructionCost = stat.destructionCost;
        if (stat.profitsPerMonth != 0) profitsPerMonth = stat.profitsPerMonth;
        if (stat.lossesPerMonth != 0) lossesPerMonth = stat.lossesPerMonth;
        if (stat.constructionCo2Emission != 0) constructionCo2Emission = stat.constructionCo2Emission;
        if (stat.destructionCo2Emission != 0) destructionCo2Emission = stat.destructionCo2Emission;
        if (stat.co2EmissionPerMonth != 0) co2EmissionPerMonth = stat.co2EmissionPerMonth;
        if (stat.co2AbsorptionPerMonth != 0) co2AbsorptionPerMonth = stat.co2AbsorptionPerMonth;
        if (stat.constructionWasteProduction != 0) constructionWasteProduction = stat.constructionWasteProduction;
        if (stat.destructionWasteProduction != 0) destructionWasteProduction = stat.destructionWasteProduction;
        if (stat.wasteProductionPerMonth != 0) wasteProductionPerMonth = stat.wasteProductionPerMonth;
        if (stat.wasteDestructionPerMonth != 0) wasteDestructionPerMonth = stat.wasteDestructionPerMonth;
        if (stat.greenSpaces != 0) greenSpaces = stat.greenSpaces;
        if (stat.populationSize != 0) populationSize = stat.populationSize;
        if (stat.health != 0) health = stat.health;
        if (stat.happiness != 0) happiness = stat.happiness;
        if (stat.sensibilisation != 0) sensibilisation = stat.sensibilisation;
        if (stat.energyConstructionCost != 0) energyConstructionCost = stat.energyConstructionCost;
        if (stat.energyDestructionCost != 0) energyDestructionCost = stat.energyDestructionCost;
        if (stat.energyProductionPerMonth != 0) energyProductionPerMonth = stat.energyProductionPerMonth;
        if (stat.energyConsumptionPerMonth != 0) energyConsumptionPerMonth = stat.energyConsumptionPerMonth;
    }

    public void DisplayStats()
    {
        Debug.Log("Construction cost : " + constructionCost);
        Debug.Log("Destruction cost : " + destructionCost);
        Debug.Log("Profits per month : " + profitsPerMonth);
        Debug.Log("Losses per month : " + lossesPerMonth);
        Debug.Log("Construction CO2 emission : " + constructionCo2Emission);
        Debug.Log("Destruction CO2 emission : " + destructionCo2Emission);
        Debug.Log("CO2 emission per month : " + co2EmissionPerMonth);
        Debug.Log("CO2 absorption per month : " + co2AbsorptionPerMonth);
        Debug.Log("Construction waste production : " + constructionWasteProduction);
        Debug.Log("Destruction waste production : " + destructionWasteProduction);
        Debug.Log("Waste production per month : " + wasteProductionPerMonth);
        Debug.Log("Waste destruction per month : " + wasteDestructionPerMonth);
        Debug.Log("Green spaces : " + greenSpaces);
        Debug.Log("Population size : " + populationSize);
        Debug.Log("Health : " + health);
        Debug.Log("Happiness : " + happiness);
        Debug.Log("Sensibilisation : " + sensibilisation);
        Debug.Log("Energy construction cost : " + energyConstructionCost);
        Debug.Log("Energy destruction cost : " + energyDestructionCost);
        Debug.Log("Energy production per month : " + energyProductionPerMonth);
        Debug.Log("Energy consumption per month : " + energyConsumptionPerMonth);
    }
}