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
    public static void TransferDraftedCards(List<Card> selectedCards, List<Card> deckCards)
    {
        foreach (Card card in selectedCards)
        {
            deckCards.Add(card);
        }
        selectedCards.Clear();
    }
    public static void RemoveSelectedCard(List<Card> cards, Card card)
    {
        cards.Remove(card);
    }
}
