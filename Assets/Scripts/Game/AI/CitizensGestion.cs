using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Probability
{
    public float mean;
    public float stddev;
}

public class TransportMode
{
    public int id { get; set; }
    public string name { get; set; }
    public bool isAvailable = false;
    public float qualityRate = 0.5f;
    public Ponderation ponderation = new Ponderation();
    public Stat stats = new Stat();
    public int dailyUsers = 0;

    public void Display()
    {
        string message = " - Transport Mode: \n" +
                         "id: " + id + "\n" +
                         "name: " + name + "\n" +
                         "isAvailable: " + isAvailable + "\n" +
                         "Quality Rate: " + qualityRate + "\n" +
                         " - Ponderation: \n" +
                         "Health: " + ponderation.health + "\n" +
                         "Happiness: " + ponderation.happiness + "\n" +
                         "Sensibilisation: " + ponderation.sensibilisation + "\n" +
                         " - Stats: \n" +
                         "co2EmissionPerMonth: " + stats.co2EmissionPerMonth + "\n" +
                         "wasteProductionPerMonth: " + stats.wasteProductionPerMonth + "\n" +
                         "energyConsumptionPerMonth: " + stats.energyConsumptionPerMonth + "\n" +
                         "health: " + stats.health + "\n";

        Debug.Log(message);
    }
}

public class Ponderation
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
    List<TransportMode> availableTransportModes = new List<TransportMode>();
    
    
    public void CitizensGestionStartInitialization()
    {
        StatInitialization.LoadPopulationStatsFromCsv(pathCSVPopulationStats, healthProbs, happinessProbs, sensibilisationProbs, distanceToWorkplaceProbs, salaryProbs);
        
        StatInitialization.LoadTransportPonderationsFromCsv(pathCSVTransportPonderations, transportModes);
        StatInitialization.LoadTransportStatsFromCsv(pathCSVTransportStats, transportModes);
        
        DisplayInitialProbs();
        
        foreach (TransportMode transportMode in transportModes)
        {
            // Active Initial Transport Modes
            if (transportMode.name == "walk" || transportMode.name == "bike" || transportMode.name == "car" || transportMode.name == "electricScooter")
            {
                transportMode.isAvailable = true;
            }
            
            transportMode.Display();
        }
        UpdateAvailableTransportModes();
    }
    
    private Citizen GenerateCitizen()
    {
        return new Citizen(GenerateCitizenStats());
    }
    
    public void ImpactPopulationStats(Stat stats, float percentage = 0.1f)
    {
        // Update the mean of the population stats
        healthProbs.mean += stats.health;
        happinessProbs.mean += stats.happiness;
        sensibilisationProbs.mean += stats.sensibilisation;
        
        DisplayInitialProbs();
        
        // Replace a part of the population
        int number = (int)(percentage * maxPopSize);
        RemoveCitizens(number);
        GenerateCitizens(number);
    }
    
    private void DisplayInitialProbs()
    {
        string message = " - Initial Population Stats: \n" +
                         "Health: " + healthProbs.mean + ", " + healthProbs.stddev + "\n" +
                         "Happiness: " + happinessProbs.mean + ", " + happinessProbs.stddev + "\n" +
                         "Sensibilisation: " + sensibilisationProbs.mean + ", " + sensibilisationProbs.stddev + "\n" +
                         "Distance to workplace: " + distanceToWorkplaceProbs.mean + ", " + distanceToWorkplaceProbs.stddev + "\n" +
                         "Salary: " + salaryProbs.mean + ", " + salaryProbs.stddev;

        Debug.Log(message);
    }
    
    public void UpdateAvailableTransportModes()
    {
        availableTransportModes.Clear();
        
        foreach (TransportMode transportMode in transportModes)
        {
            if (transportMode.isAvailable)
            {
                availableTransportModes.Add(transportMode);
            }
        }
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
            stats.health += citizen.citizenStats.health / citizens.Count;
            stats.happiness += citizen.citizenStats.happiness / citizens.Count;
            stats.sensibilisation += citizen.citizenStats.sensibilisation / citizens.Count;
        }
        
        return stats;
    }
    
    private void ResetDailyUsers()
    {
        foreach (TransportMode transportMode in transportModes)
        {
            transportMode.dailyUsers = 0;
        }
    }
    
    public string GetDailyTransportModeUsers()
    {
        string message = " - Daily Users: \n";
        foreach (TransportMode transportMode in availableTransportModes)
        {
            message += transportMode.name + ": " + transportMode.dailyUsers + "\n";
        }
        return message;
    }
    
    private void DisplayDailyUsers()
    {
        string message = GetDailyTransportModeUsers();
        Debug.Log(message);
    }
    
    public Stat ComputeInfluenceOnGlobalStats()
    {
        Stat totalCitizensStat = new Stat();
        totalCitizensStat.Reset();
        
        ResetDailyUsers();
        
        foreach (Citizen citizen in citizens)
        {
            TransportMode transportMode = citizen.GetTransportMode(availableTransportModes);
            transportMode.dailyUsers++;
            totalCitizensStat.Add(transportMode.stats);
        }
        
        
        
        totalCitizensStat.ResetPopulationStats();
        DisplayDailyUsers();
        
        return totalCitizensStat;
    }
}
