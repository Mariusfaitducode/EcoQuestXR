using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Interval
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
    
    public bool isTimePaused = false;
    
    public void TimeInitialization()
    {
        currentTime = DateTime.Now;
    }
    
    public void TimeIncrement()
    {
        currentTime = currentTime.AddSeconds(timeCounter);
        timeCounter = 0.0f; 
    }

    public bool IsCheckTime()
    {
        timeCounter += Time.deltaTime * timeScale;
        return timeCounter >= 1.0f;
    }
    
    public void PauseTime()
    {
        isTimePaused = true;
    }
    
    public void ResumeTime()
    {
        isTimePaused = false;
    }
}
