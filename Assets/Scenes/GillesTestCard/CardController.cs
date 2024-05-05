using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{

    public CardClass cardClass;


    private void OnMouseOver()
    {
        // Vérifie si le bouton droit de la souris est enfoncé
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.GetComponentInParent<DisplayCardController>().SelectCard(gameObject);
        }
    }



}
