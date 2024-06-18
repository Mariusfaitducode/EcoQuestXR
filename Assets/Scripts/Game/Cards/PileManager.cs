using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public static class PileManager
{
    public static List<Card> DrawPile(List<Card> cards, int cardsQuantity, ObjectManager objectManager)
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
            selectedCards.Add(GetValidCard(cards, objectManager));
        }
        return selectedCards;
    }

    
    // Verif that the cards on the draft are valid (enough objects on the map)
    private static Card GetValidCard(List<Card> cards, ObjectManager objectManager)
    {
        Card card;

        int maxIteration = 1000;
        int i = 0;
        
        // TODO : choose card based on stats ?
        
        while (i < maxIteration)
        {
            int randomIndex = Random.Range(0, cards.Count);
            card = cards[randomIndex];

            float nbrObjectsOnMapById = 0;
            float cardQuantity = 0;
            
            if (card.cardType == CardType.Destruction || card.cardType == CardType.Replacement || card.cardType == CardType.Upgrade || card.cardType == CardType.Downgrade)
            {
                nbrObjectsOnMapById = objectManager.GetNumberOfObjectsById(card.objectProperties1.id);
                cardQuantity = card.quantityObject1;
            
                if (nbrObjectsOnMapById >= cardQuantity)
                {
                    Debug.Log("Card " + card.title + " valid");
                    return card;
                }
                Debug.Log("Card " + card.title + " not valid" + ", " + card.cardType + " - " + card.objectProperties1.name + " : " + cardQuantity + " > " + nbrObjectsOnMapById);
                
            }
            else
            {
                Debug.Log("Card " + card.title + " valid");
                return card;
            }

            i++;
        }

        Debug.LogError("No valid card found after " + maxIteration + " iterations");
        return null;
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
