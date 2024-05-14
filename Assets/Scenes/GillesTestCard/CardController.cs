using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{

    public Card Card;


    private void OnMouseOver()
    {
        // Vérifie si le bouton droit de la souris est enfoncé
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.GetComponentInParent<DisplayCardController>().SelectCard(gameObject);
        }
    }

    public void DisplayCard()
    {
        
        this.transform.Find("Canvas").Find("choose_img").GetComponent<Image>().enabled = false;
        this.transform.Find("Canvas").Find("desactive_img").GetComponent<Image>().enabled = false;
        
    }



}
