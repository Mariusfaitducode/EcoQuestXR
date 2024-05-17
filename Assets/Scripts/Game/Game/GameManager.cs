using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { get; private set; }
    
    public CardManager cardManager;
    public ObjectManager objectManager;

    private DateTime currentTime;
    public DateTime CurrentTime => currentTime;
    public float timeScale = 1.0f; // Facteur d'accélération du temps
    private float timeCounter = 0.0f;
    
    private bool hasTransferredObjectsPropertiesFromOMToCM = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Transfers objects properties from GM to CM when CM has initialized its cards
        GameInitialisation.TransfersObjectsPropertiesToCards(cardManager, objectManager, hasTransferredObjectsPropertiesFromOMToCM);
        
        timeCounter += Time.deltaTime * timeScale;
        if (timeCounter >= 1.0f) // 1 seconde réelle écoulée
        {
            currentTime = currentTime.AddSeconds(timeCounter); // Ajoute les secondes écoulées à l'heure actuelle
            timeCounter = 0.0f; // Réinitialise le compteur

            CheckMonthlyEvent();
        }
    }
    
    public void CheckMonthlyEvent()
    {
        // Check if a monthly event is triggered
    }

    public void ExecuteCardAction(Card card)
    {
        // Check if the card is a monthly event card
    }
}
