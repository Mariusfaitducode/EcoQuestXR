using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats
{
    internal int money = 0;
    
    internal int populationSize = 0;
    internal int populationHealth = 0;
    internal int populationHappiness = 0;
    internal int populationSensibilisation = 0;
    internal int populationAcceptation = 0;
    
    internal int energyStock = 0;
    internal int energyProduction = 0;
    internal int energyConsumption = 0;
    
    internal int ecology = 0;
    internal int biodiversity = 0;
    
    internal int pollutionAir = 0;
    internal int pollutionGround = 0;
    internal int pollutionNoise = 0;
    internal int pollutionVisual = 0;
    
    public void UpdateFromCard(Card card)
    {
        // TODO : Improve card stats calculation
        money += card.cardProperties.money;
        
        populationSize += card.cardProperties.populationSize;
        // populationHealth += card.cardProperties.population;
        // populationHappiness += card.cardProperties.population;
        // populationSensibilisation += card.cardProperties.population;
        // populationAcceptation += card.cardProperties.population;
        
        energyStock += card.cardProperties.energyStock;
        // energyProduction += card.cardProperties.energy;
        // energyConsumption += card.cardProperties.energy;
        
        ecology += card.cardProperties.ecology;
        // biodiversity += card.cardProperties.ecology;
        
        pollutionAir += card.cardProperties.pollutionAir;
        // pollutionGround += card.cardProperties.pollution;
        // pollutionNoise += card.cardProperties.pollution;
        // pollutionVisual += card.cardProperties.pollution;
    }
}
