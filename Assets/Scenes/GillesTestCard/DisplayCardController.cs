using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class DisplayCardController : MonoBehaviour
{


    private float cardPrefabWidth = 1f;
    private float cardPrefabHeight = 1.5f;


    public int nbCardByRow = 4;


    public int nbCardToChoose = 3;

    private List<GameObject> cardsDisplay = new List<GameObject>();
    private List<GameObject> cardChoose = new List<GameObject>();


    public GameObject cardPrefab; // Le pr�fabriqu� de carte � instancier


    public GameObject handCard;
    public GameObject discard;


    //public int GetNbCardChoose()
    //{
    //    return CardChoose.Count;
    //}


    public bool IsEnoughtCardSelected()
    {
        return cardChoose.Count == nbCardToChoose;
    }


    public void DisplayCards(List<Card> cards)
    {
        // Espace entre chaque carte
        float cardSpacing = 0.5f; // Espacement arbitraire entre les cartes

        // Largeur totale de la ligne de cartes
        float totalWidth = Mathf.Min(cards.Count, nbCardByRow) * (cardPrefabWidth + cardSpacing);

        // Hauteur totale de la pile de cartes
        int numRows = (int)Mathf.Ceil((float)cards.Count / nbCardByRow);
        float totalHeight = numRows * (cardPrefabHeight + cardSpacing);

        // Position de d�part pour afficher les cartes
        Vector3 startPosition = transform.position - Vector3.right * (totalWidth / 2f) + Vector3.right * (cardPrefabWidth / 2f) + Vector3.up * (totalHeight / 2f) - Vector3.up * (cardPrefabHeight / 2f);

        // Instancie les pr�fabriqu�s des cartes
        for (int i = 0; i < cards.Count; i++)
        {
            // Calcule la position de la carte en fonction de la position de d�part et de l'espacement
            Vector3 cardPosition = startPosition + Vector3.right * ((i % nbCardByRow) * (cardPrefabWidth + cardSpacing)) - Vector3.up * ((i / nbCardByRow) * (cardPrefabHeight + cardSpacing));

            // Instancie le pr�fabriqu� de la carte � la position calcul�e
            GameObject newCard = Instantiate(cardPrefab, cardPosition, Quaternion.identity, this.transform);
            newCard.transform.Rotate(0f, 90f, 0f); // Tourne la carte d'un quart de tour autour de son axe Y

            CardController carteComponent = newCard.GetComponent<CardController>(); // Obtenez le composant de carte attach� � l'objet instanci�
            //carteComponent.id = cards[i].id; // D�finissez l'ID de la carte
            //carteComponent.price = cards[i].price; // D�finissez le prix de la carte

            //newCard.GetComponentInChildren<TextMeshProUGUI>().text = "Prix : " + carteComponent.price.ToString();

            carteComponent.Card = cards[i];
            
            newCard.GetComponent<CardController>().DisplayCard();
            
            // newCard.GetComponentInChildren<TextMeshProUGUI>().text = "Prix : " + carteComponent.Card.price.ToString();



            cardsDisplay.Add(newCard);

        }

        this.transform.Find("validate_canva").gameObject.SetActive(true);

    }



    public void SelectCard(GameObject card)
    {


        bool isEnabled = card.transform.Find("Canvas").Find("choose_img").GetComponentInChildren<Image>().enabled;
        int nbCardChoisie = cardChoose.Count;

        if (isEnabled)
        {

            card.transform.Find("Canvas").Find("choose_img").GetComponentInChildren<Image>().enabled = false;
            cardChoose.Remove(card);

            if (nbCardChoisie == nbCardToChoose)
            {

                foreach (GameObject card_display in cardsDisplay)
                {
                    card_display.transform.Find("Canvas").Find("desactive_img").GetComponentInChildren<Image>().enabled = false;
                }
            }

        }
        else if (nbCardChoisie < nbCardToChoose)
        {

            card.transform.Find("Canvas").Find("choose_img").GetComponentInChildren<Image>().enabled = true;
            cardChoose.Add(card);

            if (cardChoose.Count == nbCardToChoose)
            {
                foreach (GameObject card_display in cardsDisplay)
                {

                    if (!card_display.transform.Find("Canvas").Find("choose_img").GetComponentInChildren<Image>().enabled)
                    {
                        card_display.transform.Find("Canvas").Find("desactive_img").GetComponentInChildren<Image>().enabled = true;
                    }

                }
            }

        }


    }



    public void ValidateChoice()
    {

        if (IsEnoughtCardSelected())
        {

            handCard.GetComponent<HandCardController>().AddCardToHand(cardChoose);

            List<GameObject> cardsDiscard = new List<GameObject>();

            foreach (GameObject card in cardsDisplay)
            {
                if (!cardChoose.Contains(card))
                {
                    cardsDiscard.Add(card);
                }
            }

            discard.GetComponent<DiscardController>().AddCardToDiscard(cardsDiscard);

            foreach (GameObject card in cardsDisplay)
            {

                card.SetActive(false);
            
            }

            this.transform.Find("validate_canva").gameObject.SetActive(false);

        }

    }

}
