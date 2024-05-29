using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Probability
{
    public float mean;
    public float stddev;
}

public class CitizensStats
{
    internal float happiness = 0;
    internal float health = 0;
    internal float sensibilisation = 0;
    internal float acceptation = 0;
    
    public void Reset()
    {
        happiness = 0;
        health = 0;
        sensibilisation = 0;
        acceptation = 0;
    }
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
    
    internal CitizensStats citizensStats = new CitizensStats();
    internal string dailyTransportsUsers = "";
    internal Stat dailyTransportsStats = new Stat();
    
    internal float initialPercentageOfCitizens = 0.8f;
    internal float percentageOfCitizens = 0.8f;
    internal Vector2 factorOfStatsChagement = new Vector2(0.9f, 0.1f);
    
    public int maxPopSize = 1000;
    internal int totalHouseholds = 0;
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
    
    public void UpdatePopulationStatsFromCard(Stat cardStats, float percentOfPopulation = 0.1f)
    {
        // Update the mean of the population stats
        healthProbs.mean = (healthProbs.mean * factorOfStatsChagement.x) + (cardStats.health * factorOfStatsChagement.y);
        happinessProbs.mean += (happinessProbs.mean * factorOfStatsChagement.x) + (cardStats.happiness * factorOfStatsChagement.y);
        sensibilisationProbs.mean += (sensibilisationProbs.mean * factorOfStatsChagement.x) + (cardStats.sensibilisation * factorOfStatsChagement.y);
        
        DisplayInitialProbs();
        
        // Replace a part of the population
        int number = (int)(percentOfPopulation * totalCitizens);
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
        totalHouseholds = maxPopSize;
        totalCitizens = (int)(initialPercentageOfCitizens * maxPopSize);
        
        for (int i = 0; i < totalCitizens; i++)
        {
            citizens.Add(GenerateCitizen());
        }
        
        Debug.Log("Generated " + totalCitizens + " citizens.");
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

    public void UpdateCitizensStats()
    {
        citizensStats.Reset();

        foreach (Citizen citizen in citizens) {
            citizensStats.health += citizen.citizenStats.health / citizens.Count;
            citizensStats.happiness += citizen.citizenStats.happiness / citizens.Count;
            citizensStats.sensibilisation += citizen.citizenStats.sensibilisation / citizens.Count;
            citizensStats.acceptation += citizen.citizenStats.acceptation / citizens.Count;
        }
    }
    
    private void ResetDailyUsers()
    {
        foreach (TransportMode transportMode in transportModes)
        {
            transportMode.dailyUsers = 0;
        }
    }
    
    public void UpdatePopulationSize(int newHouseholds)
    {
        int newCitizens = (int)(newHouseholds * percentageOfCitizens);
        
        if (newCitizens > 0)
        {
            GenerateCitizens(newCitizens);
            totalCitizens += newCitizens;
        }
        else if (newCitizens < 0)
        {
            RemoveCitizens(-newCitizens);
            totalCitizens -= newCitizens;
        }
    }
    
    private void UpdateDailyTransportModeUsers()
    {
        dailyTransportsUsers = "";
        foreach (TransportMode transportMode in availableTransportModes)
        {
            dailyTransportsUsers += transportMode.name + ": " + transportMode.dailyUsers + "\n";
        }
    }
    
    public void UpdateDailyTransportsStats()
    {
        dailyTransportsStats.Reset();
        ResetDailyUsers();
        
        foreach (Citizen citizen in citizens)
        {
            TransportMode transportMode = citizen.GetTransportMode(availableTransportModes);
            transportMode.dailyUsers++;
            dailyTransportsStats.Add(transportMode.stats);
        }

        UpdateDailyTransportModeUsers();
    }
}
