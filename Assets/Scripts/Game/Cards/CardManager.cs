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
    public string cardsCSVPath = "Csv/cards";

    internal GameObject cardPrefab;
    internal GameObject grabbableCardPrefab;

    public TextMeshProUGUI draftCounterSelectedCardsText;

    internal List<Card> cards;
    internal List<Card> pileCards = new List<Card>();
    internal List<Card> deckCards = new List<Card>();
    internal List<Card> selectedPileCards = new List<Card>();
    internal Card selectedDeckCard;
    internal GameObject selectedGrabbableCard;
    
    internal Canvas deckCanvas;
    public GameObject deck;
    internal Canvas draftCanvas;
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
        else if (displayCard.GetParentCanvas() == deckCanvas && gameManager.controlMode == ControlMode.keyboard)
        {
            selectedDeckCard = displayCard.GetCard();
            Debug.Log("Keyboard : chosen card : " + selectedDeckCard.title);
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
        DisplayCanvas.UpdateCards(deckCards, cardsLocationDeckPanels, grabbableCardPrefab, this, deckCanvas);
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

    public void PlayEvent(GameObject selectedGrabbableCard = null)
    {
        if (selectedGrabbableCard != null)
        {
            selectedDeckCard = selectedGrabbableCard.GetComponentInChildren<DisplayCard>().GetCard();
        }
        
        if (selectedDeckCard == null)
        {
            Debug.LogError("No card selected");
            return;
        }
        
        Debug.Log("Card played : " + selectedDeckCard.title);

        // Remove Selected Card
        PileManager.RemoveSelectedCard(deckCards, selectedDeckCard);
        
        // Update Deck
        DisplayCanvas.UpdateCards(deckCards, cardsLocationDeckPanels, grabbableCardPrefab, this, deckCanvas);
        //deck.GetComponent<DeckController>().list_cards = cardsLocationDeckPanels;


        // TODO : Implement action on map
        gameManager.ExecuteCardEvent(selectedDeckCard);
        
        
        // Update Counter
        nbrMaxSelectedCards = nbrMaxDeckCards - deckCards.Count;
        DisplayCanvas.UpdateCounterText(draftCounterSelectedCardsText, nbrSelectedCards, nbrMaxSelectedCards);
    }
}
