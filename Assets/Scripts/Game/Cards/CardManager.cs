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
    internal GameManager gameManager;
    [FormerlySerializedAs("isCardsInitialized")] public bool areCardsInitialized = false;
    public string cardsCSVPath = "Csv/cards";

    public GameObject cardPrefab;
    public GameObject grabbableCard;

    public TextMeshProUGUI draftCounterSelectedCardsText;

    internal List<Card> cards;
    internal List<Card> pileCards = new List<Card>();
    internal List<Card> deckCards = new List<Card>();
    internal List<Card> selectedPileCards = new List<Card>();
    internal Card selectedDeckCard;
    
    public Canvas deckCanvas;
    public GameObject deck;
    public Canvas draftCanvas;
    public GameObject depot_zone; 
    
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
        cardsLocationDeckPanels = DisplayCanvas.GetPanels(deck);
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
    public void DraftEvent()
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
        
        gameManager.timer.PauseTime();
    }
    
    
    public void SelectUnselectEvent(DisplayCard displayCard)
    {

        if (displayCard.GetParentCanvas() == draftCanvas)
        {
            nbrSelectedCards = CardInteraction.SelectUnselectDraftCard(displayCard, nbrSelectedCards, nbrMaxSelectedCards, selectedPileCards);
            DisplayCanvas.UpdateCounterText(draftCounterSelectedCardsText, nbrSelectedCards, nbrMaxSelectedCards);
        }
        // else if (displayCard.GetParentCanvas() == deckCanvas && !draftTime)
        //else if (displayCard.GetParentCanvas() == deckCanvas)
        //{
        //    selectedDeckCard = CardInteraction.SelectUnselectDeckCard(displayCard, deckCards);
        //}
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
        DisplayCanvas.UpdateCards(deckCards, cardsLocationDeckPanels, grabbableCard, this, deckCanvas);
        //deck.GetComponent<DeckController>().list_cards = cardsLocationDeckPanels;


        // Hide Canvas
        DisplayCanvas.HideCanvas(draftCanvas);
        
        // Reset Counter
        nbrMaxSelectedCards = nbrMaxDeckCards - deckCards.Count;
        nbrSelectedCards = 0;
        DisplayCanvas.UpdateCounterText(draftCounterSelectedCardsText, nbrSelectedCards, nbrMaxSelectedCards);
        
        gameManager.timer.ResumeTime();
        gameManager.eventsGestion.GetEventByName("DraftEvent").EndEvent(gameManager.timer.currentTime);
        
        
    }

    public void PlayEvent(GameObject GrabbableCard)
    {

        selectedDeckCard = GrabbableCard.GetComponentInChildren<DisplayCard>().GetCard();

        // Remove Selected Card
        PileManager.RemoveSelectedCard(deckCards, selectedDeckCard);
        
        // Update Deck
        DisplayCanvas.UpdateCards(deckCards, cardsLocationDeckPanels, grabbableCard, this, deckCanvas);
        //deck.GetComponent<DeckController>().list_cards = cardsLocationDeckPanels;


        // TODO : Implement action on map
        gameManager.ExecuteCardEvent(selectedDeckCard);
        
        
        // Update Counter
        nbrMaxSelectedCards = nbrMaxDeckCards - deckCards.Count;
        DisplayCanvas.UpdateCounterText(draftCounterSelectedCardsText, nbrSelectedCards, nbrMaxSelectedCards);
    }
}
