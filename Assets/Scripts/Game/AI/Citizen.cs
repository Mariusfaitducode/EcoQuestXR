using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CitizenStats
{
    public float health;
    public float happiness;
    public float sensibilisation;
    public float acceptation;
    public float distance_to_workplace;
    public float salary;
}

public class Citizen
{
    public CitizenStats citizenStats;
    
    public Citizen(CitizenStats citizenStats){
        this.citizenStats = citizenStats;
    }
    public TransportMode GetTransportMode(List<TransportMode> transportModes) {

        List<float> scores = new List<float>();
        List<float> probabilities = new List<float>();
        float totalScore = 0;
        
        foreach (TransportMode transportMode in transportModes)
        {
            
            // Sum of the ponderation of each stat * the stat value (max 1 * number of parameters)
            float score = transportMode.ponderation.health * citizenStats.health +
                        transportMode.ponderation.happiness * citizenStats.happiness +
                        transportMode.ponderation.sensibilisation * citizenStats.sensibilisation +
                        StatUtils.GetFloatWeight(citizenStats.distance_to_workplace, transportMode.ponderation.meanDistance, transportMode.ponderation.stddevDistance) * citizenStats.distance_to_workplace +
                        StatUtils.GetFloatWeight(citizenStats.salary, transportMode.ponderation.meanSalary, transportMode.ponderation.stddevSalary) * citizenStats.salary+
                        2 * transportMode.qualityRate;
            
            scores.Add(score);
            totalScore += score;
        }
        
        foreach (float score in scores)
        {
            probabilities.Add(score / totalScore);
        }
        
        float randomValue = Random.Range(0f, 1f);
        float sum = 0;
        
        for (int i = 0; i < probabilities.Count; i++)
        {
            sum += probabilities[i];
            if (randomValue < sum)
            {
                return transportModes[i];
            }
        }
        
        Debug.LogError("No transport mode found");
        return transportModes[0];
    }
}
