using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Interval
{
    public int days;
    public int months;
    public int years;
}

[System.Serializable]
public class Timer
{
    internal DateTime currentTime;
    
    public float timeScale = 1.0f; // Facteur d'accélération du temps
    private float timeCounter = 0.0f;


    public bool stopTime;


    public void TimeInitialization()
    {
        currentTime = DateTime.Now;
    }


    public void TimeIncrement(EventsGestion eventsGestion)
    {
            currentTime = currentTime.AddSeconds(timeCounter);
            timeCounter = 0.0f; 
    }

    public bool IsCheckTime()
    {
        timeCounter += Time.deltaTime * timeScale;
        return timeCounter >= 1.0f;
    }
}
