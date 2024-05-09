using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    
    public string cardsCSVPath = "Csv/cards";

    public GameObject cardPrefab;
    
    internal List<Card> cards = new List<Card>();
    internal List<Card> pileCards = new List<Card>();
    
    public Canvas deckCanvas;
    public Canvas draftCanvas;
    
    internal List<GameObject> cardsLocationDeckPanels;
    internal List<GameObject> cardsLocationDraftPanels;
    
    internal int numberSelectedCards = 3;
    public int maxSelectedCards = 3;
    
    // public GameObject cardPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
        // Cards Initialization
        cards = CardsInitialization.InitializeCards(cardsCSVPath);
        Debug.Log(cards);
        
        // Canvas Initialization
        cardsLocationDraftPanels = DisplayCanvas.GetPanels(draftCanvas);
        cardsLocationDeckPanels = DisplayCanvas.GetPanels(deckCanvas);
        
        // Test
        DrawPileEvent();

    }
    
    // Tirage au sort de cartes dans la pile
    public void DrawPileEvent()
    {
        // Draw Pile
        pileCards = PileManager.DrawPile(cards, 3);
        
        // Display Canvas
        DisplayCanvas.ShowCanvas(draftCanvas);
        
        // Display Pile
        DisplayCanvas.DeleteCards(cardsLocationDraftPanels);
        DisplayCanvas.DrawCards(pileCards, cardsLocationDraftPanels, cardPrefab);
        
        // Enable Card Interaction
        // EnableCardInteraction();
    }
    
    

    // public void SelectUnselectEvent()
    // {
    //     if (CardInteraction.CanSelect(nbrSelectedCards, maxSelectedCards))
    //     {
    //         
    //     }
    // }
        
    // Regular Cards Choice Proposition Event
    
    // 1. Pile Manager -> DrawPile

    // 2. DisplayCardCanvas -> DisplayDraftCanvas -> DisplayCard
    
    // 3. Enable Card Interaction -> select, validate
    
    // Update Deck
    
   



}
