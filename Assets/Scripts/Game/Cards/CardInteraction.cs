using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardInteraction
{
    public static int SelectUnselectDraftCard(DisplayCard displayCard, int nbrSelectedCards, int nbrMaxSelectedCards, List<Card> selectedCards)
    {
        if (displayCard.IsSelected())
        {
            displayCard.Unselect();
            selectedCards.Remove(displayCard.GetCard());
            return nbrSelectedCards - 1;
        }
        
        if (nbrSelectedCards < nbrMaxSelectedCards)
        {
            displayCard.Select();
            selectedCards.Add(displayCard.GetCard());
            return nbrSelectedCards + 1;
        }

        return nbrSelectedCards;
    }

    public static Card SelectUnselectDeckCard(DisplayCard displayCard, List<Card> cards)
    {
        Card selectedCard;
        if (displayCard.IsSelected())
        {
            displayCard.Unselect();
            selectedCard = null;
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
            selectedCard = displayCard.GetCard();
        }

        return selectedCard;
    }
}
