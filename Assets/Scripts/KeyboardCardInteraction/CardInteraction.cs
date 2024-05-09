using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardInteraction
{
    
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
    public static int Select(int nbrSelectedCards)
    {
        return nbrSelectedCards + 1;
    }
    
    public static bool CanSelect(int nbrSelectedCards, int nbrMaxSelectedCards)
    {
        return nbrSelectedCards < nbrMaxSelectedCards;
    }
}
