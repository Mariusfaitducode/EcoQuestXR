using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public static class PileManager
{
    public static List<Card> DrawPile(List<Card> cards, int cardsQuantity)
    {
        List<Card> selectedCards = new List<Card>();
        
        if (cards == null || cards.Count == 0)
        {
            return selectedCards;
        }
        
        if (cardsQuantity >= cards.Count)
        {
            return cards;
        }
        
        for (int idx = 0; idx < cardsQuantity; idx++)
        {
            int randomIndex = Random.Range(0, cards.Count);
            selectedCards.Add(cards[randomIndex]);
        }
        return selectedCards;
    }
    // private List<Card> _cardsPile;
    // private List<GameObject> _cardLocationPanels;
    // private List<Card> _selectedCards;
    //
    //
    // public Canvas canvas;
    // public GameObject cardPrefab;
    // public GameObject counterTextObject;
    // public LeftArmDeckCardManager leftArmDeckCardManager;
    // private TextMeshProUGUI counterText;
    //
    // public int nbrMaxSelectedCards = 3;
    // private int _nbrSelectedCards = 0;
    //
    // // Start is called before the first frame update
    // void Start()
    // {
    //     GetCounterText();
    //     _cardLocationPanels = CardUtils.GetPanelsLocation(canvas);
    //     CardUtils.DeleteCards(_cardLocationPanels);
    //     DrawCards();
    //     UpdateCounter();
    // }
    // private void GetCounterText()
    // {
    //     counterText = counterTextObject.GetComponent<TextMeshProUGUI>();
    // }
    // public bool CanSelectCard()
    // {
    //     return _nbrSelectedCards < nbrMaxSelectedCards;
    // }
    //
    // public void Validate()
    // {
    //     Debug.Log("Validate");
    //     _selectedCards = new List<Card>();
    //     foreach (GameObject panel in _cardLocationPanels)
    //     {
    //         DisplayCard displayCard =  panel.transform.GetComponentInChildren<DisplayCard>();
    //         if (displayCard.IsSelected())
    //             _selectedCards.Add(displayCard.GetCard());
    //     }
    //     leftArmDeckCardManager.GetDraftedCards(_selectedCards);
    //     this.gameObject.SetActive(false);
    // }
    // public void AddSelectedCard()
    // {
    //     _nbrSelectedCards++;
    //     UpdateCounter();
    // }
    // public void RemoveSelectedCard()
    // {
    //     _nbrSelectedCards--;
    //     UpdateCounter();
    // }
    //
    // private void DrawCards()
    // {
    //     int idx = 0;
    //     foreach (GameObject panel in _cardLocationPanels)
    //     {
    //         GameObject card = Instantiate(cardPrefab, panel.transform);
    //         DisplayCard displayCard =  card.GetComponent<DisplayCard>();
    //         displayCard.SetCard(_cardsPile[idx]);
    //         displayCard.SetDrawPileCardManager(this);
    //         
    //         // card.GetComponent<DisplayCard>().SetCard(_cardsPile[Random.Range(0, _cardsPile.Count)]);
    //         idx++;
    //     }
    // }
    // private void UpdateCounter()
    // {
    //     counterText.text = _nbrSelectedCards + " / " + nbrMaxSelectedCards;
    // }
}
