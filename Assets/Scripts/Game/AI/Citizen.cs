using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum transportMode
{
    walk,
    car,
    bike,
    bus
}

public class Citizen
{
    internal float health = 0;
    internal float happiness = 0;
    internal float sensibilisation = 0;
    internal float acceptation = 0;
    
    internal int distance_to_workplace = 0;
    
    internal transportMode transportMode;
    
    // Citizen
    internal Stat walkInfluence = new Stat();
    internal Stat carInfluence = new Stat();
    internal Stat bikeInfluence = new Stat();
    internal Stat busInfluence = new Stat();
    
    public Citizen(float health, float happiness, float sensibilisation, int distance_to_workplace) {
        this.health = health;
        this.happiness = happiness;
        this.sensibilisation = sensibilisation;
        this.distance_to_workplace = distance_to_workplace;
        Debug.Log("Citizen created. Health: " + health + ", Happiness: " + happiness + ", Sensibilisation: " + sensibilisation + ", Distance to workplace: " + distance_to_workplace);
        StatUtils.InitializeTransportsStats(walkInfluence, carInfluence, bikeInfluence, busInfluence);
    }
    
    public Stat GetStatsFromTransportMode() {
        float[] scores = new float[4];
        
        // Pondérations
        float[,] weights = new float[,] {
            { 1.0f, 0.5f, 1.0f, StatUtils.DistanceWeight(distance_to_workplace, 2.0f, 2.0f) },  // À pied
            { 0.2f, 0.2f, 0.1f, StatUtils.DistanceWeight(distance_to_workplace, 20.0f, 5.0f) }, // En voiture
            { 0.7f, 0.5f, 1.0f, StatUtils.DistanceWeight(distance_to_workplace, 5.0f, 2.0f) },  // En vélo
            { 0.5f, 0.5f, 0.6f, StatUtils.DistanceWeight(distance_to_workplace, 15.0f, 5.0f) }  // En bus
        };

        // Calcul des scores
        scores[0] = weights[0,0] * health + weights[0,1] * happiness + weights[0,2] * sensibilisation + weights[0,3];
        scores[1] = weights[1,0] * health + weights[1,1] * happiness + weights[1,2] * sensibilisation + weights[1,3];
        scores[2] = weights[2,0] * health + weights[2,1] * happiness + weights[2,2] * sensibilisation + weights[2,3];
        scores[3] = weights[3,0] * health + weights[3,1] * happiness + weights[3,2] * sensibilisation + weights[3,3];

        // Calcul des probabilités
        float totalScore = scores[0] + scores[1] + scores[2] + scores[3];
        float[] probabilities = new float[4];
        for (int i = 0; i < scores.Length; i++) {
            probabilities[i] = scores[i] / totalScore;
        }

        // Choisir un mode de transport basé sur les probabilités
        float randomValue = Random.Range(0f, 1f);
        if (randomValue < probabilities[0])
        {
            Debug.Log ("Walk");
            return walkInfluence;
        }
        else if (randomValue < probabilities[0] + probabilities[1])
        {
            Debug.Log ("Car");
            return carInfluence;
        }
        else if (randomValue < probabilities[0] + probabilities[1] + probabilities[2])
        {
            Debug.Log ("Bike");
            return bikeInfluence;
        }
        else
        {
            Debug.Log ("Bus");
            return busInfluence;
        }
    }
}
