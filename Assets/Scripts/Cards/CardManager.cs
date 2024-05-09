using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    
    public string cardsCSVPath = "Csv/cards";
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
        CreateSomeCards();
        DrawPileEvent();

    }
    
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
    
    private void CreateSomeCards()
    {
        cards = new List<Card>();
        
        cards.Add(new Card(
            "Création d'un quartier moderne", 
            "1.4M", 
            "Construction de 4 maisons modernes et isolées",
            AreaType.City,
            ActionType.Construction,
            "-200/mois",
            "-100",
            "+16",
            "+100"));
        
        cards.Add(new Card(
            "Destruction d'un HLM", 
            "250K", 
            "Destruction de 1 HLM de 34 logements",
            AreaType.City,
            ActionType.Destruction,
            "+500/mois",
            "+200",
            "-100",
            "-300"));
        
        cards.Add(new Card(
            "Contruction d'éoliennes", 
            "2M", 
            "Construction d'un parc de 5 éoliennes",
            AreaType.Energy,
            ActionType.Construction,
            "+1000/mois",
            "-50",
            "+0",
            "+50"));
        
        cards.Add(new Card(
            "Destruction d'un centrale à charbon", 
            "600K", 
            "Destruction d'un centrale à charbon",
            AreaType.Energy,
            ActionType.Destruction,
            "-3000/mois",
            "+500",
            "+0",
            "-300"));
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
