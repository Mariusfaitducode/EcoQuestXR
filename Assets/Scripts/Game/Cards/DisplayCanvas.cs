using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayCanvas
{
    public static List<GameObject> GetPanels(Canvas canvas)
    {
        // Get all panels
        List<GameObject> cardLocationPanels = new List<GameObject>();
        foreach (Transform child in canvas.transform)
        {
            if (child.CompareTag("Location"))
            {
                cardLocationPanels.Add(child.gameObject);
            }
        }
        if (cardLocationPanels.Count == 0)
        {
            Debug.LogError("No location cards found in draw pile : you need to set it up in the scene");
        }
        return cardLocationPanels;
    }    
    public static List<GameObject> GetPanels(GameObject parent)
    {
        // Get all panels
        List<GameObject> cardLocationPanels = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            if (child.CompareTag("Location"))
            {
                cardLocationPanels.Add(child.gameObject);
            }
        }
        if (cardLocationPanels.Count == 0)
        {
            Debug.LogError("No location cards found in draw pile : you need to set it up in the scene");
        }
        return cardLocationPanels;
    }
    public static void DeleteCards(List<GameObject> cardLocationPanels)
    {
        foreach (GameObject panel in cardLocationPanels)
        {
            foreach (Transform child in panel.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
    
    // Instantiate cards in canvas
    public static void DrawCards(List<Card> cardsDeck, List<GameObject> cardLocationPanels, GameObject cardPrefab, CardManager cardManager, Canvas canvas, bool deck_mode = false) // par d�fault pour le draft, sinon en deck mode
    {
        int idx = 0;

        foreach (Card card in cardsDeck)
        {
            GameObject cardObject = GameObject.Instantiate(cardPrefab, cardLocationPanels[idx].transform);
            
            card.SetCardObject(cardObject);
            
            DisplayCard displayCard =  cardObject.GetComponentInChildren<DisplayCard>();
            
            

            displayCard.SetCard(card);
            displayCard.SetCanvas(canvas);

            if (deck_mode)
            {
                // TODO : set interaction card controller only on ovr Mode
                
                InteractionCardController interactionCardController = cardObject.GetComponent<InteractionCardController>();
                
                interactionCardController.initialPlaceTransform = cardLocationPanels[idx].transform;
                interactionCardController.cardManager = cardManager;
                interactionCardController.depotZone = cardManager.depot_zone;
                
                interactionCardController.controlMode = cardManager.gameManager.controlMode;
                
                interactionCardController.InitializeInteraction();


                if (cardManager.gameManager.controlMode == ControlMode.keyboard)
                {
                    displayCard._buttonBackground.enabled = true;
                    displayCard._buttonBackground.onClick.AddListener(delegate { cardManager.SelectUnselectEvent(displayCard); });
                }
            }
            else
            {
                displayCard._buttonBackground.enabled = true;
                displayCard._buttonBackground.onClick.AddListener(delegate { cardManager.SelectUnselectEvent(displayCard); });
            }
            
            idx++;
        }
    }
    public static void UpdateCards(List<Card> cardsDeck, List<GameObject> cardLocationPanels, GameObject grabbableCard, CardManager cardManager, Canvas canvas)
    {
        DeleteCards(cardLocationPanels);
        DrawCards(cardsDeck, cardLocationPanels, grabbableCard, cardManager, canvas, true);
    }
    public static void HideCanvas(Canvas canvas)
    {
        canvas.enabled = false;
    }
    public static void ShowCanvas(Canvas canvas)
    {
        canvas.enabled = true;
    }
    public static void UpdateCounterText(TextMeshProUGUI counterText, int nbrSelectedCards, int nbrMaxSelectedCards)
    {
        counterText.text = nbrSelectedCards + " / " + nbrMaxSelectedCards;
    }
    
    // public static
    // private List<Card> _cardsDeck;
    // private List<GameObject> _cardLocationPanels;
    //
    // public Canvas canvas;
    // public GameObject cardPrefab;
    //
    // // Start is called before the first frame update
    // void Start()
    // {
    //     _cardsDeck = new List<Card>();
    //     _cardLocationPanels = CardUtils.GetPanelsLocation(canvas);
    //     CardUtils.DeleteCards(_cardLocationPanels);
    // }
    
    // public void GetDraftedCards(List<Card> cardsDeck)
    // {
    //     foreach(Card card in cardsDeck)
    //     {
    //         _cardsDeck.Add(card);
    //     }
    //     CardUtils.DeleteCards(_cardLocationPanels);
    //     DrawCards();
    // }
    // public void UnSelectAll()
    // {
    //     foreach (GameObject panel in _cardLocationPanels)
    //     {
    //         DisplayCard displayCard = panel.transform.GetComponentInChildren<DisplayCard>();
    //         if (displayCard != null)
    //             displayCard.Unselect();
    //     }
    // }
    // private void DrawCards()
    // {
    //     int idx = 0;
    //     foreach (Card card in _cardsDeck)
    //     {
    //         GameObject cardObject = Instantiate(cardPrefab, _cardLocationPanels[idx].transform);
    //         DisplayCard displayCard =  cardObject.GetComponent<DisplayCard>();
    //         displayCard.SetCard(card);
    //         displayCard.SetLeftArmDeckCardManager(this);
    //         idx++;
    //     }
    // }
    // public void PlayCard()
    // {
    //     // TODO : implement action on map
    //     foreach (GameObject panel in _cardLocationPanels)
    //     {
    //         DisplayCard displayCard = panel.transform.GetComponentInChildren<DisplayCard>();
    //         if (displayCard != null)
    //         {
    //             if (displayCard.IsSelected())
    //             {
    //                 _cardsDeck.Remove(displayCard.GetCard());
    //                 Destroy(displayCard.gameObject);
    //             }
    //         }
    //     }
    //     CardUtils.DeleteCards(_cardLocationPanels);
    //     DrawCards();
    // }
}
