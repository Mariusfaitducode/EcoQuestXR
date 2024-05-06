using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardController : MonoBehaviour
{

    private List<CardClass> cards = new List<CardClass>();


    public void AddCardToDiscard(List<GameObject> new_cards)
    {

        foreach (GameObject new_card in new_cards)
        {
            cards.Add(new_card.GetComponent<CardController>().cardClass);
        }

    }    
    
    public void AddCardToDiscard(GameObject new_card)
    {
        cards.Add(new_card.GetComponent<CardController>().cardClass);
    }



}
