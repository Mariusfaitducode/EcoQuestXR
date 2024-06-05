using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class HandCardController : MonoBehaviour
{

    private List<GameObject> cards = new List<GameObject>();

    public GameObject discard;


    public void AddCardToHand(List<GameObject> new_cards) {

        foreach (GameObject card in new_cards)
        {
            card.transform.parent = transform;
            card.transform.Find("Canvas").Find("choose_img").GetComponentInChildren<Image>().enabled = false;
        }

        cards.AddRange(new_cards);


        ReplaceCardsInHand();
    
    }


    public void ReplaceCardsInHand()
    {
        // Vérifiez s'il y a des cartes à placer
        if (cards.Count == 0)
            return;

        // Nombre total de cartes
        int cardCount = cards.Count;

        // Rayon du cercle sur lequel les cartes seront placées
        float radius = 1.5f;

        // Angle total couvert par les cartes
        float totalAngle = 120.0f;

        // Angle initial pour la première carte
        float startAngle = -totalAngle / 2.0f;

        // Angle entre chaque carte
        float angleIncrement = cardCount > 1 ? totalAngle / (cardCount - 1) : 0f; // Vérifie si cardCount > 1 pour éviter la division par zéro

        // Position du GameObject
        Vector3 centerPosition = transform.position;

        // Placez chaque carte en arc de cercle
        for (int i = 0; i < cardCount; i++)
        {
            // Calculez l'angle pour cette carte
            float angle = startAngle + i * angleIncrement;

            // Convertissez l'angle en radians
            float angleRad = angle * Mathf.Deg2Rad;

            // Calculez les coordonnées polaires pour cette carte
            float x = centerPosition.x + radius * Mathf.Sin(angleRad);
            float y = centerPosition.y + radius * Mathf.Cos(angleRad);

            // Placez la carte à cette position
            cards[i].transform.position = new Vector3(x, y, centerPosition.z);

            // Calculez la direction du vecteur qui pointe du centre du cercle vers la position de la carte
            Vector3 direction = new Vector3(centerPosition.x - x, centerPosition.y - y, 0f).normalized;

            // Calculez l'angle de rotation à partir de la direction
            float rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            cards[i].transform.rotation = Quaternion.identity;
            cards[i].transform.Rotate(Vector3.forward, -angle); // Tournez la carte autour de l'axe Z
            cards[i].transform.Rotate(Vector3.up, 90.0f); // Tournez la carte autour de l'axe Y pour qu'elle soit perpendiculaire au rayon
        }
    }


    public void RemoveCardFromHand(GameObject card)
    {
        
        discard.GetComponent<DiscardController>().AddCardToDiscard(card);
        cards.Remove(card);
        Destroy(card);

        ReplaceCardsInHand();


    }


}
