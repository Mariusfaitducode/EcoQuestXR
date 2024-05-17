using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { get; private set; }
    public CardManager cardManager;

    private DateTime currentTime;
    public DateTime CurrentTime => currentTime;
    public float timeScale = 1.0f; // Facteur d'accélération du temps
    private float timeCounter = 0.0f;
    
    List<ObjectProperties> objectsProperties;
    bool areObjectsPropertiesTransferedToCM = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        // Get all the objects properties from the CSV file
        objectsProperties = ObjectsInitialization.InitializeObjectsProperties("Csv/objects");
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
        
        // Transfers objects properties from GM to CM when CM has initialized its cards
        TransfersObjectsPropertiesToCards();
    }
    
    public void CheckMonthlyEvent()
    {
        // Check if a monthly event is triggered
    }

    public void ExecuteCardAction(Card card)
    {
        // Check if the card is a monthly event card
    }

    public void TransfersObjectsPropertiesToCards()
    {
        if (cardManager.areCardsInitialized && !areObjectsPropertiesTransferedToCM)
        {
            cardManager.SetCardsProperties(objectsProperties);
            areObjectsPropertiesTransferedToCM = true;
        }
    }
}
