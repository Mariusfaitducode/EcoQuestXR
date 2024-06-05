
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Xml;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{

    public Card Card;


    private void OnMouseOver()
    {

        // V�rifie si le bouton droit de la souris est enfonc�
        if (Input.GetMouseButtonDown(0) && this.transform.parent.name == "DisplayCard")
        {
            gameObject.GetComponentInParent<DisplayCardController>().SelectCard(gameObject);
        }
        else if (Input.GetMouseButtonDown(0) && this.transform.parent.name == "HandCard")
        {
            Action();
            gameObject.GetComponentInParent<HandCardController>().RemoveCardFromHand(gameObject);
        }

    }


    private void Action()
    {
        Debug.Log("card " + cardClass.id + " has been played");
    }



}
