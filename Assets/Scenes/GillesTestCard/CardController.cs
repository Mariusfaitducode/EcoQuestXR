using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class CardController : MonoBehaviour
{

    public CardClass cardClass;


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
