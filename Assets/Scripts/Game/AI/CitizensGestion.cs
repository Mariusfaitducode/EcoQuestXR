using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct Probability
{
    public float mean;
    public float stddev;
}

public class CitizensGestion
{
    internal List<Citizen> citizens = new List<Citizen>();
    
    internal float initialPercentageOfCitizens = 0.8f;
    
    internal int maxPopSize = 0;
    internal int totalCitizens = 0;

    internal Probability healthProbs;
    internal Probability happinessProbs;
    internal Probability sensibilisationProbs;
    internal int maxDistanceToWorkplace = 0;
    internal int minDistanceToWorkplace = 0;
    
    public void CitizensGestionStartInitialization()
    {
        ResetProbs();
    }
    
    public void ResetProbs()
    {
        healthProbs.mean = 0.5f;
        healthProbs.stddev = 0.1f;
        
        happinessProbs.mean = 0.5f;
        happinessProbs.stddev = 0.1f;
        
        sensibilisationProbs.mean = 0.5f;
        sensibilisationProbs.stddev = 0.1f;
    }
    
    private Citizen GenerateCitizen() {
        float healthy = GenerateGaussian(healthProbs.mean, healthProbs.stddev);
        float happiness = GenerateGaussian(happinessProbs.mean, happinessProbs.stddev);
        float environment = GenerateGaussian(sensibilisationProbs.mean, sensibilisationProbs.stddev);
        int distance = Random.Range(minDistanceToWorkplace, maxDistanceToWorkplace);

        // Clamp values to be within the range [0, 1]
        healthy = Mathf.Clamp(healthy, 0f, 1f);
        happiness = Mathf.Clamp(happiness, 0f, 1f);
        environment = Mathf.Clamp(environment, 0f, 1f);

        return new Citizen(healthy, happiness, environment, distance);
    }

    private float GenerateGaussian(float mean, float stddev) {
        float u1 = Random.value;
        float u2 = Random.value;

        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        float randNormal = mean + stddev * randStdNormal;

        return randNormal;
    }
    
    public void GenerateInitialsCitizens(int maxPopSize)
    {
        this.maxPopSize = maxPopSize;
        
        int numberOfCitizens = (int)(initialPercentageOfCitizens * maxPopSize);
        
        for (int i = 0; i < numberOfCitizens; i++)
        {
            citizens.Add(GenerateCitizen());
        }
        
        Debug.Log("Generated " + numberOfCitizens + " citizens.");
    }
    
    public void GenerateCitizens(int number)
    {
        for (int i = 0; i < number; i++)
        {
            citizens.Add(GenerateCitizen());
        }
    }
    
    // Remove random citizens
    public void RemoveCitizens(int number)
    {
        for (int i = 0; i < number; i++)
        {
            citizens.RemoveAt(Random.Range(0, citizens.Count));
        }
    }

    public Stat GetCitizensStats()
    {
        float totalHealth = 0f;
        float totalHappiness = 0f;
        float totalSensibilisation = 0f;
        float totalAcceptation = 0f;

        foreach (Citizen citizen in citizens) {
            totalHealth += citizen.health;
            totalHappiness += citizen.happiness;
            totalSensibilisation += citizen.sensibilisation;
            totalAcceptation += citizen.acceptation;
        }
        
        totalHealth /= citizens.Count;
        totalHappiness /= citizens.Count;
        totalSensibilisation /= citizens.Count;
        
        Stat stats = new Stat();
        stats.Reset();
        stats.health = (int)(totalHealth*1000);
        stats.happiness = (int)(totalHappiness*1000);
        stats.sensibilisation = (int)(totalSensibilisation*1000);
        stats.DisplayStats();
        return stats;
    }
    
    public Stat ComputeInfluenceOnGlobalStats()
    {
        Stat totalCitizensStat = new Stat();
        
        foreach (Citizen citizen in citizens)
        {
            totalCitizensStat.Add(citizen.GetStatsFromTransportMode());
        }
        
        totalCitizensStat.ResetPopulationStats();
        
        // totalCitizensStat.DisplayStats();
        
        return totalCitizensStat;
    }

    
}
