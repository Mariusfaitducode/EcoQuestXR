using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { get; private set; }

    private DateTime currentTime;
    public DateTime CurrentTime => currentTime;
    public float timeScale = 1.0f; // Facteur d'accélération du temps
    private float timeCounter = 0.0f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime * timeScale;
        if (timeCounter >= 1.0f) // 1 seconde réelle écoulée
        {
            currentTime = currentTime.AddSeconds(timeCounter); // Ajoute les secondes écoulées à l'heure actuelle
            timeCounter = 0.0f; // Réinitialise le compteur

            CheckMonthlyEvent();
        }
    }
}
