using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandCardController : MonoBehaviour
{

    private List<GameObject> cards = new List<GameObject>();

    public void AddCardToHand(List<GameObject> new_cards) {

        cards.AddRange(new_cards);

        ReplaceCardsInHand();
    
    }


    public void ReplaceCardsInHand()
    {

        // Nombre total de cartes
        int cardCount = cards.Count;

        // Vérifiez s'il y a des cartes à placer
        if (cardCount == 0)
            return;

        // Rayon du cercle sur lequel les cartes seront placées
        float radius = 2.0f;

        // Angle total couvert par les cartes
        float totalAngle = 180.0f;

        // Angle initial pour la première carte
        float startAngle = -totalAngle / 2.0f;

        // Angle entre chaque carte
        float angleIncrement = totalAngle / (cardCount - 1);

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
            float x = centerPosition.x + radius * Mathf.Cos(angleRad);
            float y = centerPosition.y + radius * Mathf.Sin(angleRad);

            // Placez la carte à cette position
            cards[i].transform.position = new Vector3(x, y, centerPosition.z);
            cards[i].SetActive(true);
        }
    }



}
