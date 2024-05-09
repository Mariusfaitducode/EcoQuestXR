using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardInteraction
{
    public static int SelectUnselectDraftCard(DisplayCard displayCard, int nbrSelectedCards, int nbrMaxSelectedCards)
    {
        if (displayCard.IsSelected())
        {
            displayCard.Unselect();
            return nbrSelectedCards - 1;
        }
        
        if (nbrSelectedCards < nbrMaxSelectedCards)
        {
            displayCard.Select();
            return nbrSelectedCards + 1;
        }

        return nbrSelectedCards;
    }

    public static void SelectUnselectDeckCard(DisplayCard displayCard, List<Card> cards)
    {
        if (displayCard.IsSelected())
        {
            displayCard.Unselect();
        }
        else
        {
            foreach (Card card in cards)
            {
                DisplayCard displayCardInDeck = card.GetCardObject().GetComponent<DisplayCard>();
                if (displayCardInDeck.IsSelected())
                {
                    displayCardInDeck.Unselect();
                }
            }
            displayCard.Select();
        }
    }
    // public static void Validate()
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
    // public static void Play(Card card)
    // {
    //     // TODO : implement action on map
    //     
    // }
    // public static int GetNbrOfSelectedCard(List<Card> cards)
    // {
    //     int nbrSelectedCards = 0;
    //     foreach (Card card in cards)
    //     {
    //         DisplayCard displayCard = card.GetCardObject().GetComponent<DisplayCard>();
    //         if (displayCard.IsSelected())
    //         {
    //             nbrSelectedCards++;
    //         }
    //     }
    //     return nbrSelectedCards;
    // }
    
    
}
