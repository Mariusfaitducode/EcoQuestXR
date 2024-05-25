using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct Probability
{
    public float mean;
    public float stddev;
}

public class TransportMode
{
    public int id;
    public string name { get; set; }
    public bool isAvailable = false;
    public float qualityRate = 0.5f;
    public Ponderation ponderation;
    public Stat stats;
    public int dailyUsers = 0;
}

public struct Ponderation
{
    public float health { get; set; }
    public float happiness { get; set; }
    public float sensibilisation { get; set; }
    public float meanDistance { get; set; }
    public float stddevDistance { get; set; }
    public float meanSalary { get; set; }
    public float stddevSalary { get; set; }
}

public class CitizensGestion
{
    internal List<Citizen> citizens = new List<Citizen>();
    
    internal float initialPercentageOfCitizens = 0.8f;
    
    internal int maxPopSize = 0;
    internal int totalCitizens = 0;

    public string pathCSVPopulationStats = "Csv/initialPopStats";
    public string pathCSVTransportPonderations = "Csv/transportPonderations";
    public string pathCSVTransportStats = "Csv/transportStats";

    internal Probability healthProbs = new Probability();
    internal Probability happinessProbs = new Probability();
    internal Probability sensibilisationProbs = new Probability();
    internal Probability distanceToWorkplaceProbs = new Probability();
    internal Probability salaryProbs = new Probability();
    
    List<TransportMode> transportModes = new List<TransportMode>();
    
    
    public void CitizensGestionStartInitialization()
    {
        StatInitialization.LoadPopulationStatsFromCsv(pathCSVPopulationStats, healthProbs, happinessProbs, sensibilisationProbs, distanceToWorkplaceProbs, salaryProbs);
        
        StatInitialization.LoadTransportPonderationsFromCsv(pathCSVTransportPonderations, transportModes);
        StatInitialization.LoadTransportStatsFromCsv(pathCSVTransportStats, transportModes);
    }
    
    private Citizen GenerateCitizen()
    {
        return new Citizen(GenerateCitizenStats());
    }
    
    private CitizenStats GenerateCitizenStats()
    {
        CitizenStats citizenStats = new CitizenStats();
        
        citizenStats.health = Mathf.Clamp(GenerateGaussian(healthProbs), 0f, 1f);
        citizenStats.happiness = Mathf.Clamp(GenerateGaussian(happinessProbs), 0f, 1f);
        citizenStats.sensibilisation = Mathf.Clamp(GenerateGaussian(sensibilisationProbs), 0f, 1f);
        citizenStats.distance_to_workplace = Mathf.Clamp(GenerateGaussian(distanceToWorkplaceProbs), 0f, 1f);
        citizenStats.salary = Mathf.Clamp(GenerateGaussian(salaryProbs), 0f, 1f);
        
        return citizenStats;
    }

    private float GenerateGaussian(Probability prob) {
        float u1 = Random.value;
        float u2 = Random.value;

        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        float randNormal = prob.mean + prob.stddev * randStdNormal;

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
        Stat stats = new Stat();
        stats.Reset();

        foreach (Citizen citizen in citizens) {
            stats.health += citizen.citizenStats.health;
            stats.happiness += citizen.citizenStats.happiness;
            stats.sensibilisation += citizen.citizenStats.sensibilisation;
        }
        
        return stats;
    }
    
    public void ResetDailyUsers()
    {
        foreach (TransportMode transportMode in transportModes)
        {
            transportMode.dailyUsers = 0;
        }
    }
    
    public Stat ComputeInfluenceOnGlobalStats()
    {
        Stat totalCitizensStat = new Stat();
        totalCitizensStat.Reset();
        
        ResetDailyUsers();
        
        foreach (Citizen citizen in citizens)
        {
            TransportMode transportMode = citizen.GetTransportMode(transportModes);
            transportMode.dailyUsers++;
            totalCitizensStat.Add(transportMode.stats);
        }
        
        totalCitizensStat.ResetPopulationStats();
        
        return totalCitizensStat;
    }
}
