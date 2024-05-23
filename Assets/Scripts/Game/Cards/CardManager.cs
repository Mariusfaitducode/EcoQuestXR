using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public GameManager gameManager;
    [FormerlySerializedAs("isCardsInitialized")] public bool areCardsInitialized = false;
    public string cardsCSVPath = "Csv/cards";

    public GameObject cardPrefab;
    public TextMeshProUGUI draftCounterSelectedCardsText;

    internal List<Card> cards;
    internal List<Card> pileCards = new List<Card>();
    internal List<Card> deckCards = new List<Card>();
    internal List<Card> selectedPileCards = new List<Card>();
    internal Card selectedDeckCard;
    
    public Canvas deckCanvas;
    public Canvas draftCanvas;
    
    internal List<GameObject> cardsLocationDeckPanels;
    internal List<GameObject> cardsLocationDraftPanels;
    
    internal int nbrSelectedCards = 0;
    
    internal int nbrMaxSelectedCards = 3;
    internal int nbrMaxDeckCards = 5;
    internal int nbrDraftCards = 4;
    
    public float factorCostReductionDestruction = 0.2f;
    
    internal bool draftTime = false;
    
    // public GameObject cardPrefab;
    
    // Start is called before the first frame update

    public void CardsStartInitialization()
    {
        // Cards Initialization
        cards = CardsInitialization.InitializeCards(cardsCSVPath);
        areCardsInitialized = true;
        
        // Canvas Initialization
        cardsLocationDraftPanels = DisplayCanvas.GetPanels(draftCanvas);
        cardsLocationDeckPanels = DisplayCanvas.GetPanels(deckCanvas);
        DisplayCanvas.HideCanvas(draftCanvas);

        nbrDraftCards = cardsLocationDraftPanels.Count;
        nbrMaxDeckCards = cardsLocationDeckPanels.Count;
    
        nbrMaxSelectedCards = nbrMaxDeckCards - deckCards.Count;
        DisplayCanvas.UpdateCounterText(draftCounterSelectedCardsText, nbrSelectedCards, nbrMaxSelectedCards);

    }

    public void SetCardsProperties(List<ObjectProperties> objectsProperties)
    {
        CardsInitialization.MatchCardWithObjectProperties(cards, objectsProperties, factorCostReductionDestruction);
    }
    
    
    // Tirage au sort de cartes dans la pile
    public void Draft()
    {
        // TODO : Pile manager give only valid cards
        
        draftTime = true;
        // Draw Pile
        pileCards = PileManager.DrawPile(cards, nbrDraftCards);
        
        // Display Canvas
        DisplayCanvas.ShowCanvas(draftCanvas);
        
        // Display Pile
        DisplayCanvas.DeleteCards(cardsLocationDraftPanels);
        
        DisplayCanvas.DrawCards(pileCards, cardsLocationDraftPanels, cardPrefab, this, draftCanvas);
    }
    
    
    public void SelectUnselectEvent(DisplayCard displayCard)
    {
        if (displayCard.GetParentCanvas() == draftCanvas)
        {
            nbrSelectedCards = CardInteraction.SelectUnselectDraftCard(displayCard, nbrSelectedCards, nbrMaxSelectedCards, selectedPileCards);
            DisplayCanvas.UpdateCounterText(draftCounterSelectedCardsText, nbrSelectedCards, nbrMaxSelectedCards);
        }
        // else if (displayCard.GetParentCanvas() == deckCanvas && !draftTime)
        else if (displayCard.GetParentCanvas() == deckCanvas)
        {
            selectedDeckCard = CardInteraction.SelectUnselectDeckCard(displayCard, deckCards);
        }
        else
        {
            Debug.LogError("Parent Canvas not found");
        }
        
        
    }
    
    public void ValidateEvent()
    {
        
        
        // Transfer Drafted Cards
        PileManager.TransferDraftedCards(selectedPileCards, deckCards);
        
        // Update Deck
        DisplayCanvas.UpdateCards(deckCards, cardsLocationDeckPanels, cardPrefab, this, deckCanvas);
        
        // Hide Canvas
        DisplayCanvas.HideCanvas(draftCanvas);
        
        // Reset Counter
        nbrMaxSelectedCards = nbrMaxDeckCards - deckCards.Count;
        nbrSelectedCards = 0;
        DisplayCanvas.UpdateCounterText(draftCounterSelectedCardsText, nbrSelectedCards, nbrMaxSelectedCards);
        
        
        Debug.Log("Draft Finished");
        gameManager.DraftFinished();
        draftTime = false;
        
        
    }

    public void PlayEvent()
    {
        // Remove Selected Card
        PileManager.RemoveSelectedCard(deckCards, selectedDeckCard);
        
        // Update Deck
        DisplayCanvas.UpdateCards(deckCards, cardsLocationDeckPanels, cardPrefab, this, deckCanvas);
        
        // TODO : Implement action on map
        gameManager.ExecuteCardEvent(selectedDeckCard);
        
        
        
        // Update Counter
        nbrMaxSelectedCards = nbrMaxDeckCards - deckCards.Count;
        DisplayCanvas.UpdateCounterText(draftCounterSelectedCardsText, nbrSelectedCards, nbrMaxSelectedCards);
    }
}
