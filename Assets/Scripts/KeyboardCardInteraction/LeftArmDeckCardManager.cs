using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmDeckCardManager : MonoBehaviour
{
    private List<Card> _cardsDeck;
    private List<GameObject> _cardLocationPanels;
    
    public Canvas canvas;
    public GameObject cardPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        _cardsDeck = new List<Card>();
        _cardLocationPanels = CardUtils.GetPanelsLocation(canvas);
        CardUtils.DeleteCards(_cardLocationPanels);
    }
    
    public void GetDraftedCards(List<Card> cardsDeck)
    {
        foreach(Card card in cardsDeck)
        {
            _cardsDeck.Add(card);
        }
        CardUtils.DeleteCards(_cardLocationPanels);
        DrawCards();
    }
    public void UnSelectAll()
    {
        foreach (GameObject panel in _cardLocationPanels)
        {
            DisplayCard displayCard = panel.transform.GetComponentInChildren<DisplayCard>();
            if (displayCard != null)
                displayCard.Unselect();
        }
    }
    private void DrawCards()
    {
        int idx = 0;
        foreach (Card card in _cardsDeck)
        {
            GameObject cardObject = Instantiate(cardPrefab, _cardLocationPanels[idx].transform);
            DisplayCard displayCard =  cardObject.GetComponent<DisplayCard>();
            displayCard.SetCard(card);
            displayCard.SetLeftArmDeckCardManager(this);
            idx++;
        }
    }
    public void PlayCard()
    {
        // TODO : implement action on map
        foreach (GameObject panel in _cardLocationPanels)
        {
            DisplayCard displayCard = panel.transform.GetComponentInChildren<DisplayCard>();
            if (displayCard != null)
            {
                if (displayCard.IsSelected())
                {
                    _cardsDeck.Remove(displayCard.GetCard());
                    Destroy(displayCard.gameObject);
                }
            }
        }
        CardUtils.DeleteCards(_cardLocationPanels);
        DrawCards();
    }
}
