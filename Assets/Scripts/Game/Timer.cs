using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Interval
{
    public int days;
    public int months;
    public int years;
}

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
        if (stopTime)
        {
            return false;
        }
        timeCounter += Time.deltaTime * timeScale;
        return timeCounter >= 1.0f;
    }
}
