using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    
    public string cardsCSVPath = "Csv/cards";
    internal List<Card> cards = new List<Card>();
    
    public Canvas deckCanvas;
    public Canvas draftCanvas;
    
    // public GameObject cardPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
        // Cards Initialization
        
        cards = CardsInitialization.InitializeCards(cardsCSVPath);
        Debug.Log(cards);

    }
    
        
    // Regular Cards Choice Proposition Event
    
    // 1. Pile Manager -> DrawPile

    // 2. DisplayCardCanvas -> DisplayDraftCanvas -> DisplayCard
    
    // 3. Enable Card Interaction -> select, validate
    
    // Update Deck
    
   



}
